from wasp.WebApi.Services.StaticServiceProvider import ServiceLocator
from wasp.WebApi.Services.DataDefinition import IDataDefinitionService

class BaseMigrationPackage:
    def __init__(self):
        pass

    def up(self):
        pass

    def down(self):
        pass

class DtpMigrationPackage(BaseMigrationPackage):
    def __init__(self):
        self.data_service = ServiceLocator.ServiceProvider.GetService[IDataDefinitionService]()

    def create_datatable(self, datatable, primary_keys, columns = []):
        return self.data_service.CreateDataTable(datatable, primary_keys, columns)

    def create_dataitem(self, dataitem):
        return self.data_service.CreateDataItem(dataitem)
        
    def create_dtp_record(self, datatable, primary_keys, properties):
        return self.data_service.CreateDtpRecord(datatable, primary_keys, properties)