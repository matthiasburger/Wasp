using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace wasp.WebApi.Data.Models
{
    [SuppressMessage("ReSharper", "UnusedType.Global", Justification = "needs implementation")]
    public class DtpRecord
    {
        public string DataTable { get; set; }

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Needs implementation")] 
        public DataItem[] DataItems { get; set; }

        public DataItem this[string pythonId]
        {
            get { return DataItems.First(x => x.PythonId == pythonId); }
        }
    }
}
