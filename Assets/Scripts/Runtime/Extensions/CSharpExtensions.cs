using System;

namespace Core.Other
{
    public static class CSharpExtensions
    {
        public static bool HasFlag<T>(this T value, T flag)
            where T : Enum
        {
            return (Convert.ToInt32(value) & Convert.ToInt32(flag)) == Convert.ToInt32(flag);
        }
    }
}
