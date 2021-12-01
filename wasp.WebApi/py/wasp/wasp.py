from wasp.WebApi.Services.StaticServiceProvider import ServiceLocator
from wasp.WebApi.Services.DataDefinition import IDataDefinitionService

from enum import Enum 

class BaseDataService:
    def __init__(self):
        '''
        Initializes a new object from type BaseDataService.
        This class is abstract and may not be initialized.
        '''
        if type(self) is BaseDataService:
            raise Exception('%s is an abstract class and cannot be instantiated directly' % self.__class__.__name__)

class DataManipulationService(BaseDataService):
    '''
    This service-class provides functions for handling DTP records.
    '''
    
    def create_record(self, data_table, data_items):
        '''
        Creates a new DTP record.
            
            Parameters:
                data_table (string): 
                    The name of the datatable the record is created on.
                    
                data_items (dict): 
                    A dictionary containing at least all primary keys (not the auto-generated ones) and further columns with their values.
                    The key is the Python-ID and the value is the value of the column.
                    
            Example:
                data_manipulation_service.create_record('DataTable', {'SqlId': 'DataTable', 'Name': 'Data-Table'})
            
            Returns:
                dtp_record_state (IEntityState[DtpRecord]): 
                    An IEntityState-object that contains the DTP record just created, or None on failure.
                    Use the state-property (DtpRecordCreationState) to check if it was created.
                    Use the error-property (string) to receive the error-message.
                                      
        '''
        pass
        
    def search_record(self, data_table, keys, data_items):
        '''
        Searches for a DTP record.
            
            Parameters:
                data_table (string): 
                    The name of the datatable the record is stored.
                    
                keys (dict): 
                    A dictionary containing all primary keys. The key is the Python-ID and the value is the value of the column.
                    
                data_items (list): 
                    A list of Python-IDs containing all data-items that have to be returned. If empty, no column is fetched but the primary keys.
                    
            Example:
                record = data_manipulation_service.search_record('DataTable', {'id': '001'}, ['name', 'sql_id'])
                sql_id = record.sql_id.get_value()
                name = record.name.get_value()
            
            Returns:
                dtp_record (DtpRecord): 
                    The DTP record that was found, or None if no record was found.                                      
        '''
        pass
        

class DataDefinitionService(BaseDataService):
    def __init__(self):
        self.data_service = ServiceLocator.ServiceProvider.GetService[IDataDefinitionService]()
        
    def create_datatable(self, datatable, primary_keys, columns = []):
        return self.data_service.CreateDataTable(datatable, primary_keys, columns)
    
    def create_data_item(self, data_item):
        return self.data_service.CreateDataItem(data_item)
