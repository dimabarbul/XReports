using System;

namespace XReports.Extensions
{
    public static class ArrayExtensions
    {
        public static int FindIndex<T>(this T[] array, Predicate<T> predicate)
        {
            return Array.FindIndex(array, predicate);
        }
    }
}
