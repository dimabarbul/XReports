using System;
using System.Reflection;

namespace XReports.Tests.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsSameOrEqualsOrHasSameTypeAndProperties(this object actual, object expected)
        {
            if (actual == null)
            {
                throw new ArgumentNullException(nameof(actual));
            }

            return expected != null &&
                (actual == expected ||
                    actual.Equals(expected) ||
                    HaveSameTypeAndProperties(actual, expected));
        }

        private static bool HaveSameTypeAndProperties(object actual, object expected)
        {
            Type type = actual.GetType();
            if (type != expected.GetType())
            {
                return false;
            }

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (!IsSameOrEqualsOrHasSameTypeAndProperties(propertyInfo.GetValue(actual), propertyInfo.GetValue(expected)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
