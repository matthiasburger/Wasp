namespace wasp.WebApi.Services.Configuration
{
    public class PythonSettings
    {
        public static string SectionName => nameof(PythonSettings);
        
        public string Path { get; set; }
        public string PythonDll { get; set; }
    }
}