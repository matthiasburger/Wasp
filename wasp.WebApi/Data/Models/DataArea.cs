using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using IronSphere.Extensions;

using SqlKata;
using SqlKata.Compilers;

using wasp.WebApi.Data.Entity;
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

        [ForeignKey("TableId")] 
        public DataTable DataTable { get; set; } = null!;
        
        public ICollection<DataField> DataFields { get; set; } = new List<DataField>();
        
        public ICollection<DataArea> Children { get; set; } = new List<DataArea>();
        
        public ICollection<DataAreaReference> DataAreaReferences { get; set; } = new List<DataAreaReference>();
    }
    
    public interface IDataArea
    {
        DataTable DataTable { get; }
        
        public ICollection<DataAreaReference> DataAreaReferences { get; set; }

        
        ICollection<DataField> DataFields { get; }

        ICollection<DataArea> Children { get; }

        public QueryBuilder BuildQuery()
        {
            QueryBuilder queryBuilder = new (DataTable);
            foreach (DataArea area in Children)
            {
                queryBuilder.Join(area);
            }
            queryBuilder.AddDataArea(this);
            
            return queryBuilder;
        }
    }
    
    public class QueryBuilder
    {
        public record QueryResult(string Sql, IDictionary<string, object> Bindings);
        
        private readonly Query _query;

        private readonly HashSet<string> _usedTableAliases = new ();
        private int ordinal = 0;

        public QueryBuilder(DataTable dataTable)
        {
            _setTableAlias(dataTable);
            
            _query = new Query(dataTable.GetSqlId());
        }

        public QueryResult GetQuery()
        {
            SqlServerCompiler compiler = new ();
            SqlResult? result = compiler.Compile(_query);
            return new QueryResult(result.Sql, result.NamedBindings);
        }
        
        private void _setTableAlias(DataTable dataTable)
        {
            if (dataTable is null)
                throw new ArgumentNullException(nameof(dataTable));
            
            string alias = dataTable.SqlId.CamelCase()!;

            int count = 0;
            string newAlias = alias;
            while (_usedTableAliases.Contains(newAlias))
                newAlias = $"{alias}_{++count}";

            dataTable.Alias = newAlias;
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
                _query.Select(dataField.DataItem.GetSqlId());
            }

            foreach (DataField dataField in dataArea.DataFields)
            {
                if (!string.IsNullOrWhiteSpace(dataField.FilterFrom))
                    _query.Where(dataField.DataItem.GetSqlId(), ">=", dataField.FilterFrom);
            }
        }

        public void Join(IDataArea subArea)
        {
            _setTableAlias(subArea.DataTable);

            _query.Join(subArea.DataTable.GetSqlId(), join =>
            {
                return subArea.DataAreaReferences.Aggregate(join, (current, reference) => current.On(reference.ReferenceDataItem.GetSqlId(), reference.KeyDataItem.GetSqlId()));
            },"inner join");
            
            foreach (DataArea da in subArea.Children)
                Join(da);

            AddDataArea(subArea);
        }
    }
}