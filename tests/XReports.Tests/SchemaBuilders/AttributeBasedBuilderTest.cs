using System;
using Microsoft.Extensions.DependencyInjection;

namespace XReports.Tests.SchemaBuilders
{
    public partial class AttributeBasedBuilderTest
    {
        private readonly IServiceProvider serviceProvider = new ServiceCollection().BuildServiceProvider();
    }
}
