using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace wasp.WebApi.Data.Dto
{
    [SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Needs implementation")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Needs implementation")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global", Justification = "Needs implementation")]
    public class DtpRecord
    {
        public string? DataTable { get; set; }

        public DataItem[] DataItems { get; set; } = Array.Empty<DataItem>();

        public DataItem this[string pythonId]
        {
            get { return DataItems.First(x => x.PythonId == pythonId); }
        }
    }
}
