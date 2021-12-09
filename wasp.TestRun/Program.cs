using System.Collections.Generic;
using SqlKata;
using SqlKata.Compilers;
using wasp.Core.DynamicQuery;

namespace wasp.TestRun
{
    
    public class DataArea : IDataArea
    {
        public DataTable Table { get; set; }

        public IEnumerable<DataItem> KeyColumns { get; set; } = new List<DataItem>();
        public string Columns { get; set; } = "*";
        public IEnumerable<DataField> DataFields { get; set; }
        public IEnumerable<IDataArea> SubAreas { get; set; } = new List<IDataArea>();
        public IEnumerable<DataItem> ReferenceColumns { get; set; }
    }
    
    public class Program
    {
        public static void Main()
        {
            DataTable project = new()
            {
                Alias = "p1",
                SqlId = "Project"
            };
            DataTable task = new()
            {
                Alias = "t1",
                SqlId = "Task"
            };
            
            IDataArea da = new DataArea
            {
                Table = project,
                KeyColumns = new List<DataItem>
                {
                    new()
                    {
                        Table = project,
                        SqlId = "Id"
                    }
                },
                SubAreas = new List<IDataArea>
                {
                    new DataArea
                    {
                        Table = task,
                        ReferenceColumns = new []
                        {
                            new DataItem
                            {
                                Table = task,
                                SqlId = "ProjectId"
                            }
                        },
                        DataFields = new List<DataField>
                        {
                            new()
                            {
                                DataItem = new DataItem
                                {
                                    Table = task,
                                    SqlId = "Id"
                                }
                            },
                            new()
                            {
                                DataItem = new DataItem
                                {
                                    Table = project,
                                    SqlId = "Name"
                                }
                            }
                            
                        }
                    }
                },
                
                DataFields = new List<DataField>
                {
                    new()
                    {
                        DataItem = new DataItem
                        {
                            Table = project,
                            SqlId = "Id",
                        },         
                        FilterFrom = "1"

                    },
                    new()
                    {
                        DataItem = new DataItem
                        {
                            Table = project,
                            SqlId = "Name"
                        }
                    }
                }
            };

            QueryBuilder query = da.BuildQuery();
            string sqlQuery = query.GetQuery();
        }
    }
}