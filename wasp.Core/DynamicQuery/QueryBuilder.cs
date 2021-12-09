using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IronSphere.Extensions;

using SqlKata;
using SqlKata.Compilers;

namespace wasp.Core.DynamicQuery
{
    public class DataItem
    {
        public DataTable Table { get; set; }

        // public string Name { get; set; }
        public string SqlId { get; set; }

        public string GetSqlId() => $"{Table.Alias}.{SqlId}";
    }

    public class DataTable
    {
        public string SqlId { get; set; }
        public string Alias { get; set; }

        public string GetSqlId()
        {
            return $"{SqlId} as {Alias}";
        }
    }

    public class DataField
    {
        public DataItem DataItem { get; set; }
        public string FilterFrom { get; set; }
    }

    public class QueryBuilder
    {
        private readonly Query _query;

        private readonly HashSet<string> _usedTableAliases = new ();

        public QueryBuilder(DataTable dataTable)
        {
            _setTableAlias(dataTable);
            
            _query = new Query(dataTable.GetSqlId());
        }

        public string GetQuery()
        {
            SqlServerCompiler compiler = new ();
            SqlResult? result = compiler.Compile(_query);
            return result.Sql;
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

        public void AddDataArea(IDataArea dataArea)
        {
            _query.Select(dataArea.DataFields.Select(x=>x.DataItem.GetSqlId()).ToArray());

            foreach (DataField dataField in dataArea.DataFields)
            {
                if (!string.IsNullOrWhiteSpace(dataField.FilterFrom))
                    _query.Where(dataField.DataItem.GetSqlId(), ">=", dataField.FilterFrom);
            }
        }

        public void Join(IDataArea area, IDataArea subArea)
        {
            _setTableAlias(subArea.Table);

            DataItem[] keyColumns = area.KeyColumns.ToArray();
            DataItem[] referenceColumns = subArea.ReferenceColumns.ToArray();

            _query.Join(subArea.Table.GetSqlId(), join =>
            {
                for(int index = 0; index < keyColumns.Length; index++)
                    join = join.On(keyColumns[index].GetSqlId(), referenceColumns[index].GetSqlId());
                return join;
            },"inner join");
            
            foreach (IDataArea da in subArea.SubAreas)
                Join(area, da);

            AddDataArea(subArea);
        }
    }
    
    public interface IDataArea
    {
        DataTable Table { get; }
        
        IEnumerable<DataItem> KeyColumns { get; }
        IEnumerable<DataItem> ReferenceColumns { get; set; }
        
        IEnumerable<DataField> DataFields { get; }

        IEnumerable<IDataArea> SubAreas { get; }

        public QueryBuilder BuildQuery()
        {
            QueryBuilder queryBuilder = new (Table);
            foreach (IDataArea area in SubAreas)
            {
                queryBuilder.Join(this, area);
            }
            queryBuilder.AddDataArea(this);
            
            return queryBuilder;
        }
    }
}