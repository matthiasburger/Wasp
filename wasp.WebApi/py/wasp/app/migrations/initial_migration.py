from wasp.core.migration.migration_package import DtpMigrationPackage

class CreateBaseDatatables(DtpMigrationPackage):

    def create_table_datatable(self):

        datatable  = {
            'SqlId': 'DataTable',
            'Name': 'DataTable'
        }

        id_column = {
            'DataTable': 'DataTable',
            'Name': 'Id',
            'SqlId': 'Id',
            'PythonId': 'id',
            'DbType': 'bigint',
            'IsVirtual': False,
            'ApplicationType': 'long',
            'IsNullable': False,
            'IdentitySeed': 1,
            'IdentityIncrement': 1
        }
        sqlid_column = {
            'DataTable': 'DataTable',
            'Name': 'Sql Id',
            'SqlId': 'SqlId',
            'PythonId': 'sql_id',
            'DbType': 'varchar',
            'DbLength': 20,
            'IsVirtual': False,
            'ApplicationType': 'string',
            'IsNullable': False
        }
        name_column = {
            'DataTable': 'DataTable',
            'Name': 'Name',
            'SqlId': 'Name',
            'PythonId': 'name',
            'DbType': 'varchar',
            'DbLength': 50,
            'IsVirtual': False,
            'ApplicationType': 'string',
            'IsNullable': False
        }

        self.create_datatable(datatable, (id_column,), [sqlid_column, name_column])
        
    def create_table_data_item(self):
    
        dataitemtable  = {
            'SqlId': 'DataItem',
            'Name': 'DataItem'
        }
        
        dataitem_id_column = {
            'DataTable': 'DataItem',
            'Name': 'Id',
            'SqlId': 'Id',
            'PythonId': 'id',
            'DbType': 'bigint',
            'IsVirtual': False,
            'ApplicationType': 'long',
            'IsNullable': False,
            'IdentitySeed': 1,
            'IdentityIncrement': 1
        }

        dataitem_datatable_column = {
            'DataTable': 'DataItem',
            'Name': 'DataTable Id',
            'SqlId': 'DataTableId',
            'PythonId': 'datatable_id',
            'DbType': 'bigint',
            'IsVirtual': False,
            'ApplicationType': 'long',
            'IsNullable': False
        }
        dataitem_name_column = {
            'DataTable': 'DataItem',
            'Name': 'Name',
            'SqlId': 'Name',
            'PythonId': 'name',
            'DbType': 'varchar',
            'DbLength': 200,
            'IsVirtual': False,
            'ApplicationType': 'string',
            'IsNullable': False
        }
        dataitem_sqlid_column = {
            'DataTable': 'DataItem',
            'Name': 'SQL Id',
            'SqlId': 'SqlId',
            'PythonId': 'sql_id',
            'DbType': 'varchar',
            'DbLength': 100,
            'IsVirtual': False,
            'ApplicationType': 'string',
            'IsNullable': False
        }
        dataitem_pythonid_column = {
            'DataTable': 'DataItem',
            'Name': 'Python Id',
            'SqlId': 'PythonId',
            'PythonId': 'python_id',
            'DbType': 'varchar',
            'DbLength': 200,
            'IsVirtual': False,
            'ApplicationType': 'string',
            'IsNullable': False
        }
        dataitem_dbtype_column = {
            'DataTable': 'DataItem',
            'Name': 'DB-Type',
            'SqlId': 'DbType',
            'PythonId': 'db_type',
            'DbType': 'varchar',
            'DbLength': 30,
            'IsVirtual': False,
            'ApplicationType': 'string',
            'IsNullable': False
        }
        dataitem_dblength_column = {
            'DataTable': 'DataItem',
            'Name': 'DB-Length',
            'SqlId': 'DbLength',
            'PythonId': 'db_length',
            'DbType': 'int',
            'IsVirtual': False,
            'ApplicationType': 'int',
            'IsNullable': False
        }
        dataitem_precision_column = {
            'DataTable': 'DataItem',
            'Name': 'DB-Precision',
            'SqlId': 'DbPrecision',
            'PythonId': 'db_precision',
            'DbType': 'int',
            'IsVirtual': False,
            'ApplicationType': 'int',
            'IsNullable': False
        }
        dataitem_scale_column = {
            'DataTable': 'DataItem',
            'Name': 'DB-Scale',
            'SqlId': 'DbScale',
            'PythonId': 'db_scale',
            'DbType': 'int',
            'IsVirtual': False,
            'ApplicationType': 'int',
            'IsNullable': False
        }
        dataitem_isvirtual_column = {
            'DataTable': 'DataItem',
            'Name': 'Is Virtual',
            'SqlId': 'IsVirtual',
            'PythonId': 'is_virtual',
            'DbType': 'bit',
            'IsVirtual': False,
            'ApplicationType': 'bool',
            'IsNullable': False
        }
        dataitem_applicationtype_column = {
            'DataTable': 'DataItem',
            'Name': 'Application-Type',
            'SqlId': 'ApplicationType',
            'PythonId': 'application_type',
            'DbType': 'varchar',
            'DbLength': 200,
            'IsVirtual': False,
            'ApplicationType': 'string',
            'IsNullable': False
        }
        dataitem_isnullable_column = {
            'DataTable': 'DataItem',
            'Name': 'Is Nullable',
            'SqlId': 'IsNullable',
            'PythonId': 'is_nullable',
            'DbType': 'bit',
            'IsVirtual': False,
            'ApplicationType': 'bool',
            'IsNullable': False
        }
        dataitem_sqldefaultvalue_column = {
            'DataTable': 'DataItem',
            'Name': 'SQL Defaultvalue',
            'SqlId': 'SqlDefaultValue',
            'PythonId': 'sql_default_value',
            'DbType': 'varchar',
            'DbLength': 200,
            'IsVirtual': False,
            'ApplicationType': 'string',
            'IsNullable': False
        }

        self.create_datatable(dataitemtable, (dataitem_id_column,), [
            dataitem_datatable_column, dataitem_name_column, dataitem_sqlid_column, 
            dataitem_pythonid_column, dataitem_dbtype_column, dataitem_dblength_column,
            dataitem_precision_column, dataitem_scale_column, dataitem_isvirtual_column,
            dataitem_applicationtype_column, dataitem_isnullable_column, dataitem_sqldefaultvalue_column
        ])


    def create_table_relationship(self):
        
        relationshiptable  = {
            'SqlId': 'Relationship',
            'Name': 'Relationship'
            }

        relationship_id_column = {
            'DataTable': 'Relationship',
            'Name': 'Id',
            'SqlId': 'Id',
            'PythonId': 'id',
            'DbType': 'bigint',
            'IsVirtual': False,
            'ApplicationType': 'long',
            'IsNullable': False,
            'IdentitySeed': 1,
            'IdentityIncrement': 1
        }

        relationship_dataitem_column = {
            'DataTable': 'Relationship',
            'Name': 'Dataitem Id',
            'SqlId': 'DataitemId',
            'PythonId': 'dataitem_id',
            'DbType': 'bigint',
            'IsVirtual': False,
            'ApplicationType': 'long',
            'IsNullable': False
        }
        relationship_foreignkey_column = {
            'DataTable': 'Relationship',
            'Name': 'Foreign-Key Id',
            'SqlId': 'ForeignKeyId',
            'PythonId': 'foreignkey_id',
            'DbType': 'bigint',
            'IsVirtual': False,
            'ApplicationType': 'long',
            'IsNullable': True
        }
        relationship_name_column = {
            'DataTable': 'Relationship',
            'Name': 'Name',
            'SqlId': 'Name',
            'PythonId': 'name',
            'DbType': 'varchar',
            'DbLength': 80,
            'IsVirtual': False,
            'ApplicationType': 'long',
            'IsNullable': False
        }
        
        self.create_datatable(relationshiptable, (relationship_id_column,), [relationship_dataitem_column, relationship_foreignkey_column, relationship_name_column])
    
    def create_datatable_dtp_records(self):
        self.create_dtp_record('DataTable', {'SqlId': 'DataTable', 'Name': 'Data-Table'})
        self.create_dtp_record('DataTable', {'SqlId': 'DataItem', 'Name': 'Data-Item'})
        self.create_dtp_record('DataTable', {'SqlId': 'Relationship', 'Name': 'Relationship'})
    
    def up(self):
        
        self.create_table_datatable()
        self.create_table_data_item()
        self.create_table_relationship()
        
        self.create_datatable_dtp_records()