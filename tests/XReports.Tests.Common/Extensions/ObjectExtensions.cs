using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;

namespace XReports.Tests.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsSameOrEqualsOrHasSameTypeAndProperties(this object actual, object expected)
        {
            if (actual == null)
            {
                return expected == null;
            }

            return expected != null &&
                (actual == expected ||
                    actual.Equals(expected) ||
                    HaveSameTypeAndProperties(actual, expected));
        }

        private static bool HaveSameTypeAndProperties(object actual, object expected)
        {
            if (actual is IEnumerable actualEnumerable)
            {
                if (!(expected is IEnumerable expectedEnumerable))
                {
                    return false;
                }

                return ContainSameOrEqualElements(actualEnumerable, expectedEnumerable);
            }

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

        private static bool ContainSameOrEqualElements(IEnumerable actualEnumerable, IEnumerable expectedEnumerable)
        {
            List<object> actualList = ToList(actualEnumerable);
            List<object> expectedList = ToList(expectedEnumerable);

            actualList.Should().HaveSameCount(expectedList);

            foreach (object actual in actualList)
            {
                int expectedIndex = expectedList.FindIndex(
                    e => actual.IsSameOrEqualsOrHasSameTypeAndProperties(e));

                if (expectedIndex != -1)
                {
                    expectedList.RemoveAt(expectedIndex);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private static List<object> ToList(IEnumerable enumerable)
        {
            List<object> result = new List<object>();
            foreach (object o in enumerable)
            {
                result.Add(o);
            }

            return result;
        }
    }
}
