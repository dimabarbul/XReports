using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
using XReports.Tests.Common.Extensions;

namespace XReports.Tests.Common.Assertions
{
    public static class GenericCollectionAssertionsExtensions
    {
        public static GenericCollectionAssertions<T> ContainSameOrEqualElements<T>(this GenericCollectionAssertions<T> assertions, IEnumerable<T> expected)
        {
            List<T> actualList = assertions.Subject.ToList();
            List<T> expectedList = expected.ToList();

            actualList.Should().HaveSameCount(expectedList);

            for (int i = 0; i < actualList.Count; i++)
            {
                int expectedIndex = expectedList.FindIndex(
                    e => actualList[i].IsSameOrEqualsOrHasSameTypeAndProperties(e));

                if (expectedIndex != -1)
                {
                    expectedList.RemoveAt(expectedIndex);
                }
                else
                {
                    Execute.Assertion
                        .ForCondition(false)
                        .FailWith("element {0} at index {1} does not exist in collection {2}", actualList[i], i, expected);
                }
            }

            return assertions;
        }
    }
}
