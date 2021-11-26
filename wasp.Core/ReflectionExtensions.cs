using System;
using System.Diagnostics.CodeAnalysis;

namespace wasp.Core
{
    [SuppressMessage("ReSharper", "UnusedType.Global", Justification = "TODO: move this to IronSphere.Extensions")]
    public static class ReflectionExtensions
    {
        public static bool IsNullableType(this Type @this)
        {
            return @this.IsGenericType && @this.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        
        public static Type GetNullableUnderlyingType(this Type @this)
        {
            return Nullable.GetUnderlyingType(@this);
        }
    }
}