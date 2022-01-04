using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wasp.WebApi.Data;
using wasp.WebApi.Data.Extensions;
using wasp.WebApi.Data.Models;
using wasp.WebApi.Data.Models.Schema;
using Index = wasp.WebApi.Data.Models.Index;

namespace wasp.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BaseDataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BaseDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Run()
        {
            await _context.Database.ExecuteSqlRawAsync(@"delete from [Relation]");
            await _context.Database.ExecuteSqlRawAsync(@"delete from [Index]");
            await _context.Database.ExecuteSqlRawAsync(@"delete from [DataItem]");
            await _context.Database.ExecuteSqlRawAsync(@"delete from [DataTable]");
            
            await _createDataTableData();
            await _createColumnData();
            await _createForeignKeyData();
            await _createPrimaryKeyData();
            
            return Ok();
        }

        private async Task _createPrimaryKeyData()
        {
            const string query = @"
 select schema_name(tab.schema_id) as [schema_name], 
    pk.[name] as pk_name,
    substring(column_names, 1, len(column_names)-1) as [columns],
    tab.[name] as table_name
from sys.tables tab
    inner join sys.indexes pk
        on tab.object_id = pk.object_id 
        and pk.is_primary_key = 1
   cross apply (select col.[name] + ', '
                    from sys.index_columns ic
                        inner join sys.columns col
                            on ic.object_id = col.object_id
                            and ic.column_id = col.column_id
                    where ic.object_id = tab.object_id
                        and ic.index_id = pk.index_id
                            order by col.column_id
                            for xml path ('') ) D (column_names)
order by schema_name(tab.schema_id),
    pk.[name]
";
            IEnumerable<Data.Models.Schema.PrimaryKey> primaryKeys = await _context.SqlQuery<Data.Models.Schema.PrimaryKey>(query);
            IList<Index> indexes = primaryKeys.Select(primaryKey => new Index()
                {
                    Id = primaryKey.PrimaryKeyName,
                    Type = IndexType.PrimaryKey
                })
                .ToList();
            await _context.Indexes.AddRangeAsync(indexes);
            await _context.SaveChangesAsync();

            foreach (Data.Models.Schema.PrimaryKey primaryKey in primaryKeys)
            {
                string[] columns = primaryKey.GetColumns().ToArray();

                foreach (string column in columns)
                {
                    await _context.Relations.AddAsync(new Relation
                    {
                        IndexId = primaryKey.PrimaryKeyName,
                        KeyDataTableId = primaryKey.TableName,
                        KeyDataItemId = column,
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task _createForeignKeyData()
        {
            const string query = @"
SELECT
   FK.[name] AS ForeignKeyConstraintName
  ,SCHEMA_NAME(FT.schema_id) + '.' + FT.[name] AS ForeignTable
  ,STUFF(ForeignColumns.ForeignColumns, 1, 2, '') AS ForeignColumns
  ,SCHEMA_NAME(RT.schema_id) + '.' + RT.[name] AS ReferencedTable
  ,STUFF(ReferencedColumns.ReferencedColumns, 1, 2, '') AS ReferencedColumns
FROM
  sys.foreign_keys FK
  INNER JOIN sys.tables FT
  ON FT.object_id = FK.parent_object_id
  INNER JOIN sys.tables RT
  ON RT.object_id = FK.referenced_object_id
  CROSS APPLY
  (
    SELECT
      ', ' + iFC.[name] AS [text()]
    FROM
      sys.foreign_key_columns iFKC
      INNER JOIN sys.columns iFC
      ON iFC.object_id = iFKC.parent_object_id
        AND iFC.column_id = iFKC.parent_column_id
    WHERE
      iFKC.constraint_object_id = FK.object_id
    ORDER BY
      iFC.[name]
    FOR XML PATH('')
  ) ForeignColumns (ForeignColumns)
  CROSS APPLY
  (
    SELECT
      ', ' + iRC.[name]AS [text()]
    FROM
      sys.foreign_key_columns iFKC
      INNER JOIN sys.columns iRC
      ON iRC.object_id = iFKC.referenced_object_id
        AND iRC.column_id = iFKC.referenced_column_id
    WHERE
      iFKC.constraint_object_id = FK.object_id
    ORDER BY
      iRC.[name]
    FOR XML PATH('')
  ) ReferencedColumns (ReferencedColumns)
";

            IEnumerable<ForeignKey> foreignKeyData = await _context.SqlQuery<ForeignKey>(query);
            IList<Index> indexes = foreignKeyData.Select(foreignKey => new Index()
                {
                    Id = foreignKey.ForeignKeyConstraintName,
                    Type = foreignKey.ForeignTable == foreignKey.ReferencedTable
                        ? IndexType.RecursiveRelation
                        : IndexType.Relation
                })
                .ToList();
            await _context.Indexes.AddRangeAsync(indexes);
            await _context.SaveChangesAsync();

            foreach (ForeignKey foreignKey in foreignKeyData)
            {
                string[] foreignKeyColumns = foreignKey.GetForeignColumns().ToArray();
                string[] referencedKeyColumns = foreignKey.GetReferencedColumns().ToArray();

                if (foreignKeyColumns.Length != referencedKeyColumns.Length)
                    throw new Exception($"foreignKeyColumns.Length {foreignKeyColumns.Length} != referencedKeyColumns.Length {referencedKeyColumns.Length}");
                
                for (int i = 0; i < foreignKeyColumns.Length; i++)
                {
                    string foreignKeyTableId = foreignKey.ForeignTable.Split('.').Last();
                    string referenceTableId = foreignKey.ReferencedTable.Split('.').Last();
                    string keyDataItem = foreignKeyColumns[i];
                    string referenceDataItem = referencedKeyColumns[i];
                    await _context.Relations.AddAsync(new Relation
                    {
                        IndexId = foreignKey.ForeignKeyConstraintName,
                        KeyDataTableId = foreignKeyTableId,
                        ReferenceDataTableId = referenceTableId,
                        KeyDataItemId = keyDataItem,
                        ReferenceDataItemId = referenceDataItem
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task _createColumnData()
        {
            IEnumerable<TableColumn> columns = await _context.SqlQuery<TableColumn>(@"
SELECT COLUMN_NAME, TABLE_NAME
FROM INFORMATION_SCHEMA.COLUMNS
");

            IEnumerable<DataItem> dataItems = columns.Select(x => new DataItem
            {
                Id = x.ColumnName,
                Name = x.ColumnName,
                DataTableId = x.TableName,
                Required = x.IsNullable
            });
            await _context.DataItems.AddRangeAsync(dataItems);
            await _context.SaveChangesAsync();
        }

        private async Task _createDataTableData()
        {
            IEnumerable<Table> tables = await _context.SqlQuery<Table>(@"
SELECT * 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
");
            IEnumerable<DataTable> dataTables = tables.Select(x => new DataTable
            {
                Id = x.TableName,
                SqlId = x.TableName,
                Name = x.TableName
            });

            await _context.DataTables.AddRangeAsync(dataTables);
            await _context.SaveChangesAsync();
        }
    }
}