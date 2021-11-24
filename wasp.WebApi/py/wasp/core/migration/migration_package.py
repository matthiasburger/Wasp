import builtins

class BaseMigrationPackage:
    def __init__(self):
        pass

    def up(self):
        pass

    def down(self):
        pass

class DtpMigrationPackage:
    def __init__(self):
        self.data_service = builtins.di_resolver.Resolve('wasp.WebApi.Services.DataDefinition.IDataDefinitionService')

    def create_datatable(self, datatable, primary_keys):
        return self.data_service.CreateDataTable(datatable, primary_keys)

    def create_dataitem(self, dataitem):
        return self.data_service.CreateDataItem(dataitem)