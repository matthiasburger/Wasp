namespace wasp.WebApi.Services.Configuration
{
    public class PythonSettings
    {
        public static string SectionName => nameof(PythonSettings);
        
        public string? Path { get; set; }
        public string? PythonHome { get; set; }
        public string? PythonPath { get; set; }
        
        public string? PythonNetPyDll { get; set; }
    }
}