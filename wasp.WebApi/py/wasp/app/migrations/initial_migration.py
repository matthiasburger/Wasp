from wasp.core.migration.migration_package import DtpMigrationPackage

class CreateBaseDatatables(DtpMigrationPackage):

    def up(self):

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
            'Name': 'SqlId',
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

        (data_table_dtp, primary_key_dtps, relation_dtp) = self.create_datatable(datatable, (id_column,))
        name_column_dtp = self.create_dataitem(name_column)
        sql_column_dtp = self.create_dataitem(sqlid_column)

        dtp_records_to_create = []
        dtp_records_to_create.append(data_table_dtp)
        dtp_records_to_create.extend([*primary_key_dtps])
        dtp_records_to_create.extend([*relation_dtp])
        dtp_records_to_create.append(name_column_dtp)
        dtp_records_to_create.append(sql_column_dtp)



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

        (dataitem_table_dtp, dataitem_primary_key_dtps, dataitem_relation_dtp) = self.create_datatable(dataitemtable, (dataitem_id_column,))
        dtp_records_to_create.append(dataitem_table_dtp)
        dtp_records_to_create.extend([*dataitem_primary_key_dtps])
        dtp_records_to_create.extend([*dataitem_relation_dtp])

        dataitem_datatable_dtp = self.create_dataitem(dataitem_datatable_column)
        dtp_records_to_create.append(dataitem_datatable_dtp)
        dtp_records_to_create.append(self.create_dataitem(dataitem_name_column))
        dtp_records_to_create.append(self.create_dataitem(dataitem_sqlid_column))
        dtp_records_to_create.append(self.create_dataitem(dataitem_pythonid_column))
        dtp_records_to_create.append(self.create_dataitem(dataitem_dbtype_column))
        dtp_records_to_create.append(self.create_dataitem(dataitem_dblength_column))
        dtp_records_to_create.append(self.create_dataitem(dataitem_precision_column))
        dtp_records_to_create.append(self.create_dataitem(dataitem_scale_column))
        dtp_records_to_create.append(self.create_dataitem(dataitem_isvirtual_column))
        dtp_records_to_create.append(self.create_dataitem(dataitem_applicationtype_column))
        dtp_records_to_create.append(self.create_dataitem(dataitem_isnullable_column))
        dtp_records_to_create.append(self.create_dataitem(dataitem_sqldefaultvalue_column))


        ## todo: relation_dataitem_datatable = self.create_relation(dataitem_datatable_dtp, primary_key_dtps[0]);

        
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
        
        (relationship_table_dtp, relationship_primary_key_dtps, relationship_relation_dtp) = self.create_datatable(relationshiptable, (relationship_id_column,))
        dtp_records_to_create.append(relationship_table_dtp)
        dtp_records_to_create.extend([*relationship_primary_key_dtps])
        dtp_records_to_create.extend([*relationship_relation_dtp])

        dtp_records_to_create.append(self.create_dataitem(relationship_dataitem_column))
        dtp_records_to_create.append(self.create_dataitem(relationship_foreignkey_column))
        dtp_records_to_create.append(self.create_dataitem(relationship_name_column))
