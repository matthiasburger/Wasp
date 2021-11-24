using System;

namespace Wasp.Core
{
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