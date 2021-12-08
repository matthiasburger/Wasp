using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlKata;

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
    }

    public class DataField
    {
        public DataItem DataItem { get; set; }
        public string FilterFrom { get; set; }
    }
    
    public interface IDataArea
    {
        DataTable Table { get; }
        
        IEnumerable<DataItem> KeyColumns { get; }
        IEnumerable<DataItem> ReferenceColumns { get; set; }
        
        IEnumerable<DataField> DataFields { get; }

        IEnumerable<IDataArea> SubAreas { get; }

        public Query BuildQuery()
        {
            Query q = new(Table.SqlId + " as " + Table.Alias);

            foreach (IDataArea area in SubAreas)
                area.JoinTo(q, this);

            q.Select(DataFields.Select(x=>x.DataItem.GetSqlId()).ToArray());

            foreach (DataField dataField in DataFields)
            {
                if (!string.IsNullOrWhiteSpace(dataField.FilterFrom))
                    q.Where(dataField.DataItem.GetSqlId(), ">=", dataField.FilterFrom);
            }

            return q;
        }

        public void JoinTo(Query query, IDataArea dataArea)
        {
            DataItem[]? keyColumns = dataArea.KeyColumns.ToArray();
            DataItem[]? referenceColumns = ReferenceColumns.ToArray();
            
            query.Join(Table.SqlId + " as " + Table.Alias, join =>
            {
                for(int index = 0; index < dataArea.KeyColumns.Count(); index++)
                    join = join.On(keyColumns[index].GetSqlId(), referenceColumns[index].GetSqlId());
                return join;
            },"inner join");
            
            foreach (IDataArea area in SubAreas)
                area.JoinTo(query, this);
            
            
            query.Select(DataFields.Select(x=>x.DataItem.GetSqlId()).ToArray());
            
            foreach (DataField dataField in DataFields)
            {
                if (!string.IsNullOrWhiteSpace(dataField.FilterFrom))
                    query.Where(dataField.DataItem.GetSqlId(), ">=", dataField.FilterFrom);
            }
        }
    }
}