using JetBrains.Annotations;

namespace wasp.Core.Extensions
{
    public static class StringExtensions
    {
        public static string? CamelCase(this string? str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
                return str;

            return $"{char.ToLower(str[0])}{str[1..]}";
        }
        
        public static string? PascalCase(this string? str)
        {
            if (string.IsNullOrEmpty(str) || char.IsUpper(str[0]))
                return str;

            return $"{char.ToUpper(str[0])}{str[1..]}";
        }
    }
}