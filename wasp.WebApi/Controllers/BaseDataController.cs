using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using wasp.WebApi.Data;
using wasp.WebApi.Data.Extensions;
using wasp.WebApi.Data.Models;
using wasp.WebApi.Data.Models.Schema;

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
            await _createDataTableData();
            await _createColumnData();
            
            return Ok();
        }

        private async Task _createColumnData()
        {
            IEnumerable<TableColumn> columns = await _context.SqlQuery<TableColumn>($@"
SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
");

            IEnumerable<DataItem> dataItems = columns.Select(x => new DataItem
            {
                Id = x.ColumnName,
                Name = x.ColumnName,
                DataTableId = x.TableName
            });
            await _context.DataItems.AddRangeAsync(dataItems);
            await _context.SaveChangesAsync();
        }

        private async Task _createDataTableData()
        {
            IEnumerable<Table> tables = await _context.SqlQuery<Table>($@"
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