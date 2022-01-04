using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.Xml;

using IronSphere.Extensions;

using SqlKata;
using SqlKata.Compilers;

using wasp.WebApi.Data.Entity;
using wasp.WebApi.Data.Mts;
using wasp.WebApi.Data.SurrogateKeyGenerator;

namespace wasp.WebApi.Data.Models
{
    
    public class DataArea : SurrogateBaseEntity<string, DefaultSurrogateKeyGenerator>, IRecursiveEntity<DataArea, string>, IDataArea
    {
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.Column("Id", Order = 1, TypeName = "nvarchar(10)"), Required]
        public override string Id { get; set; } = null!;

        [System.ComponentModel.DataAnnotations.Schema.Column("ModuleId", Order = 2, TypeName = "nvarchar(10)")]
        public string? ModuleId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("Name", Order = 3, TypeName = "nvarchar(300)")]
        public string? Name { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("ParentId", Order = 4, TypeName = "nvarchar(10)")]
        public string? ParentId { get; set; }
        
        [System.ComponentModel.DataAnnotations.Schema.Column("DataTableId", Order = 5, TypeName = "nvarchar(100)"), Required]
        public string DataTableId { get; set; } = null!;

        [ForeignKey("ModuleId")]
        public Module? Module { get; set; }
        
        [ForeignKey("ParentId")]
        public DataArea? Parent { get; set; }

        [ForeignKey("DataTableId")] 
        public DataTable DataTable { get; set; } = null!;
        
        public ICollection<DataField> DataFields { get; set; } = new List<DataField>();
        
        public ICollection<DataArea> Children { get; set; } = new List<DataArea>();

        public MtsDataAreaInfo GetDataAreaInfo()
        {
            return new MtsDataAreaInfo
            {
                Id = Id,
                Name = Name,
                DataTableId = DataTableId,
                ModuleId = ModuleId
            };
        }

        public string GetSqlId()
        {
            return Alias.IsNullOrWhiteSpace() ? DataTable.Name : $"{DataTable.Name} as {Alias}";
        }

        public ICollection<DataAreaReference> DataAreaReferences { get; set; } = new List<DataAreaReference>();
        
        [NotMapped]
        public string Alias { get; set; }
    }
    
    public interface IDataArea
    {
        DataTable DataTable { get; }
        
        public ICollection<DataAreaReference> DataAreaReferences { get; set; }

        
        ICollection<DataField> DataFields { get; }

        ICollection<DataArea> Children { get; }
        string Name { get; set; }
        string Alias { get; set; }


        public QueryBuilder BuildQuery()
        {
            QueryBuilder queryBuilder = new (this);
            foreach (DataArea area in Children)
            {
                queryBuilder.Join(area);
            }
            queryBuilder.AddDataArea(this);
            
            return queryBuilder;
        }

        MtsDataAreaInfo GetDataAreaInfo();
        string GetSqlId();
    }
    
    public class QueryBuilder
    {
        public record QueryResult(string Sql, IDictionary<string, object> Bindings);
        
        private readonly Query _query;

        private readonly HashSet<string> _usedTableAliases = new ();
        private int ordinal = 0;
        
        public QueryBuilder(IDataArea dataArea)
        {
            _setAreaAlias(dataArea);
            _query = new Query(dataArea.GetSqlId());
        }

        public QueryResult GetQuery()
        {
            SqlServerCompiler compiler = new ();
            SqlResult? result = compiler.Compile(_query);
            return new QueryResult(result.Sql, result.NamedBindings);
        }
        
        private void _setAreaAlias(IDataArea dataArea)
        {
            if (dataArea is null)
                throw new ArgumentNullException(nameof(dataArea));
            
            string alias = dataArea.DataTable.SqlId.CamelCase();

            int count = 0;
            string newAlias = alias;
            while (_usedTableAliases.Contains(newAlias))
                newAlias = $"{alias}_{++count}";

            dataArea.Alias = newAlias;
            _usedTableAliases.Add(newAlias);
        }

        private void _setOrdinal(DataField dataField)
        {
            dataField.Ordinal = ordinal++;
        }

        public void AddDataArea(IDataArea dataArea)
        {
            foreach (DataField dataField in dataArea.DataFields)
            {
                _setOrdinal(dataField);
                _query.Select(dataField.GetSqlId());
            }

            foreach (DataField dataField in dataArea.DataFields)
            {
                if (!string.IsNullOrWhiteSpace(dataField.FilterFrom))
                    _query.Where(dataField.GetSqlId(), ">=", dataField.FilterFrom);
            }
        }

        public void Join(IDataArea subArea)
        {
            _setAreaAlias(subArea);
            
            _query.Join(subArea.GetSqlId(), join =>
            {
                Join result = @join;
                foreach (DataAreaReference reference in subArea.DataAreaReferences)
                {
                    DataArea area = reference.DataArea;
                    DataArea parent = area.Parent!;

                    string referenceDataItemId = parent.DataFields.First(w => w.DataItemId == reference.ReferenceDataItemId && w.DataTableId == reference.ReferenceDataTableId).GetSqlId(area);
                    string keyDataItemId = area.DataFields.First(w => w.DataItemId == reference.KeyDataItemId && w.DataTableId == reference.KeyDataItemDataTableId).GetSqlId(parent);
                    
                    result = result.On(referenceDataItemId, keyDataItemId);
                }
                return result;
            },"left join");
            
            foreach (DataArea da in subArea.Children)
                Join(da);

            AddDataArea(subArea);
        }
    }
}