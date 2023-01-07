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

            foreach (T actual in actualList)
            {
                int expectedIndex = expectedList.FindIndex(
                    e => actual.IsSameOrEqualsOrHasSameTypeAndProperties(e));

                if (expectedIndex != -1)
                {
                    expectedList.RemoveAt(expectedIndex);
                }
                else
                {
                    Execute.Assertion
                        .ForCondition(false)
                        .FailWith("element {0} does not exist in expected collection", actual);
                }
            }

            return assertions;
        }
    }
}
