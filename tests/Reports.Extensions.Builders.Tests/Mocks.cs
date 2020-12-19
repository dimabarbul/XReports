using System;
using Microsoft.Extensions.DependencyInjection;

namespace Reports.Extensions.Builders.Tests
{
    public static class Mocks
    {
        public static IServiceProvider ServiceProvider => new ServiceCollection().BuildServiceProvider();
    }
}
