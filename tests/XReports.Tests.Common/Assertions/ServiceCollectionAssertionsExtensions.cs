using Microsoft.Extensions.DependencyInjection;

namespace XReports.Tests.Common.Assertions
{
    public static class ServiceCollectionAssertionsExtensions
    {
        public static ServiceCollectionAssertions Should(this IServiceCollection serviceCollection)
        {
            return new ServiceCollectionAssertions(serviceCollection);
        }
    }
}
