using Microsoft.Extensions.DependencyInjection;

namespace XReports.Tests.Assertions
{
    public static class ServiceCollectionAssertionsExtensions
    {
        public static ServiceCollectionAssertions Should(this IServiceCollection serviceCollection)
        {
            return new ServiceCollectionAssertions(serviceCollection);
        }
    }
}
