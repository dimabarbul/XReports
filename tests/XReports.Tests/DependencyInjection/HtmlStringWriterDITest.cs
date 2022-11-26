using Microsoft.Extensions.DependencyInjection;
using XReports.DependencyInjection;
using XReports.Interfaces;
using XReports.Tests.Assertions;
using XReports.Writers;
using Xunit;

namespace XReports.Tests.DependencyInjection
{
    public partial class HtmlStringWriterDITest
    {
        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddHtmlStringWriterShouldRegister(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddHtmlStringWriter(lifetime);

            serviceCollection.Should().ContainDescriptor<IHtmlStringWriter, HtmlStringWriter>(lifetime);
            serviceCollection.Should().ContainDescriptor<IHtmlStringCellWriter, HtmlStringCellWriter>(lifetime);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddHtmlStringWriterWithCustomWriterImplementationShouldRegister(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddHtmlStringWriter<CustomHtmlStringWriter>(lifetime);

            serviceCollection.Should().ContainDescriptor<IHtmlStringWriter, CustomHtmlStringWriter>(lifetime);
            serviceCollection.Should().ContainDescriptor<IHtmlStringCellWriter, HtmlStringCellWriter>(lifetime);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddHtmlStringWriterWithCustomCellWriterImplementationShouldRegister(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddHtmlStringWriter<HtmlStringWriter, CustomHtmlStringCellWriter>(lifetime);

            serviceCollection.Should().ContainDescriptor<IHtmlStringWriter, HtmlStringWriter>(lifetime);
            serviceCollection.Should().ContainDescriptor<IHtmlStringCellWriter, CustomHtmlStringCellWriter>(lifetime);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddHtmlStringWriterWithCustomWriteAndCellWriterImplementationsShouldRegister(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddHtmlStringWriter<CustomHtmlStringWriter, CustomHtmlStringCellWriter>(lifetime);

            serviceCollection.Should().ContainDescriptor<IHtmlStringWriter, CustomHtmlStringWriter>(lifetime);
            serviceCollection.Should().ContainDescriptor<IHtmlStringCellWriter, CustomHtmlStringCellWriter>(lifetime);
        }
    }
}
