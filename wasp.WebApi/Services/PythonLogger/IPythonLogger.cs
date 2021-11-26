using System.Diagnostics.CodeAnalysis;

namespace wasp.WebApi.Services.PythonLogger
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface IPythonLogger
    {
        // flushes the buffer
        void flush();

        // writes str to the logger's buffer
        void write(string str);

        // writes multiple lines of strings to the logger's buffer
        [SuppressMessage("ReSharper", "IdentifierTypo")]
        void writelines(string[] str);

        // closes the buffer
        void close();

        // Reads the stream
        string ReadStream();
    }
}