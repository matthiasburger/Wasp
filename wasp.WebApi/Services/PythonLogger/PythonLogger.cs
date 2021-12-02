using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;

namespace wasp.WebApi.Services.PythonLogger
{
    public class PythonLogger : IPythonLogger
    {
        private readonly List<string> buffer;
        private readonly ILogger<PythonLogger> _logger;

        public PythonLogger(ILogger<PythonLogger> logger)
        {
            buffer = new List<string>();
            _logger = logger;
        }

        public void close()
        {
            buffer.Clear();
        }

        public void flush()
        {
            buffer.Clear();
        }

        public string ReadStream()
        {
            string str = string.Join("\n", buffer);
            _logger.LogDebug("{str}",str);
            return str;
        }

        public void write(string str)
        {
            if (str == "\n")
            {
                return;
            }

            buffer.Add(str);
            _logger.LogDebug("{str}", str);
            Trace.WriteLine(str);
        }

        [SuppressMessage("ReSharper", "IdentifierTypo")]
        public void writelines(string[] str)
        {
            foreach(string message in str)
                _logger.LogDebug("{message}", message);

            buffer.AddRange(str);
        }
    }
}