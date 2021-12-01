from wasp.wasp import DataDefinitionService, DataManipulationService

class BaseMigrationPackage:
    def __init__(self):
        pass

    def up(self):
        pass

    def down(self):
        pass

class DtpMigrationPackage(BaseMigrationPackage):
    def __init__(self):
        self.data_service = DataDefinitionService()
        self.dml_service = DataManipulationService()

    def create_datatable(self, datatable, primary_keys, columns = []):
        return self.data_service.create_datatable(datatable, primary_keys, columns)

    def create_dataitem(self, dataitem):
        return self.data_service.create_data_item(dataitem)
        
    def create_dtp_record(self, datatable, primary_keys, properties):
        return self.dml_service.create_record(datatable, primary_keys, properties)