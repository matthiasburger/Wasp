using System.Collections.Generic;
using System.Linq;
using DynamicLinqCore;


using SqlKata;
using SqlKata.Compilers;

using wasp.WebApi.Data.Models;


namespace wasp.TestRun
{
    public class Program
    {
        public static void Main()
        {
            DataTable project = new()
            {
                SqlId = "Project"
            };
            DataTable task = new()
            {
                SqlId = "Task"
            };
            DataTable resource = new()
            {
                SqlId = "Resource"
            };

            DataItem projectId = new()
            {
                DataTable = project,
                Id = "Id"
            };
            DataItem projectName = new()
            {
                DataTable = project,
                Id = "Name"
            };
            DataItem taskProjectId = new()
            {
                DataTable = task,
                Id = "ProjectId"
            };
            DataItem taskName = new()
            {
                DataTable = task,
                Id = "Name"
            };
            DataItem taskId = new()
            {
                DataTable = task,
                Id = "Id"
            };
            DataItem resourceId = new()
            {
                DataTable = resource,
                Id = "Id"
            };
            DataItem resourceFullName = new()
            {
                DataTable = resource,
                Id = "FullName"
            };
            DataItem projectProjectManagerId = new()
            {
                DataTable = project,
                Id = "ProjectManagerId"
            };

            DataItem[] items = new List<DataItem>
            {
                projectId, projectName, projectProjectManagerId,
                taskId, taskName, taskProjectId,
                resourceId, resourceFullName
            }.ToArray();

            IDataArea da = new DataArea
            {
                DataTable = project,
                Children = new List<DataArea>
                {
                    new DataArea
                    {
                        DataTable = task,
                        DataAreaReferences = new List<DataAreaReference>()
                        {
                            new DataAreaReference
                            {
                                KeyDataItem = projectId,
                                ReferenceDataItem = taskProjectId
                            }
                        },
                        DataFields = new List<DataField>
                        {
                            new()
                            {
                                DataItem = taskId
                            },
                            new()
                            {
                                DataItem = taskName
                            },
                            new()
                            {
                                DataItem = projectName
                            }
                        }
                    },
                    new DataArea
                    {
                        DataTable = resource,
                        DataAreaReferences = new List<DataAreaReference>()
                        {
                            new DataAreaReference
                            {
                                KeyDataItem = projectProjectManagerId,
                                ReferenceDataItem = resourceId
                            }
                        },
                        DataFields = new List<DataField>
                        {
                            new()
                            {
                                DataItem = resourceFullName
                            }
                        }
                    }
                },

                DataFields = new List<DataField>
                {
                    new()
                    {
                        DataItem = projectId,
                        FilterFrom = "1"
                    },
                    new()
                    {
                        DataItem = projectName,
                        FilterFrom = "Pr%"
                    }
                }
            };

            QueryBuilder query = da.BuildQuery();
            QueryBuilder.QueryResult sqlQuery = query.GetQuery();

            

        }
    }
}