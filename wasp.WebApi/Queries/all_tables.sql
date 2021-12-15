-- all tables
SELECT * 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG='wasp_test'

-- all columns
SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_CATALOG='wasp_test'

-- all foreign-keys
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


-- all primary-keys
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