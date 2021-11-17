using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Python.Runtime;

namespace wasp.WebApi.Data.Models
{
    public class DtpRecord
    {
        public string DataTable { get; set; }

        public DataItem[] DataItems { get; set; }

        public DataItem this[string pythonId]
        {
            get { return DataItems.First(x => x.PythonId == pythonId); }
        }
    }
}
