using Microsoft.Extensions.DependencyInjection;
using XReports.DependencyInjection;
using XReports.Interfaces;
using XReports.Tests.Assertions;
using XReports.Writers;
using Xunit;

namespace XReports.Tests.DependencyInjection
{
    public partial class HtmlStreamWriterDITest
    {
        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddHtmlStreamWriterShouldRegister(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddHtmlStreamWriter(lifetime);

            serviceCollection.Should().ContainDescriptor<IHtmlStreamWriter, HtmlStreamWriter>(lifetime);
            serviceCollection.Should().ContainDescriptor<IHtmlStreamCellWriter, HtmlStreamCellWriter>(lifetime);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddHtmlStreamWriterWithCustomWriterImplementationShouldRegister(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddHtmlStreamWriter<CustomHtmlStreamWriter>(lifetime);

            serviceCollection.Should().ContainDescriptor<IHtmlStreamWriter, CustomHtmlStreamWriter>(lifetime);
            serviceCollection.Should().ContainDescriptor<IHtmlStreamCellWriter, HtmlStreamCellWriter>(lifetime);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddHtmlStreamWriterWithCustomCellWriterImplementationShouldRegister(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddHtmlStreamWriter<HtmlStreamWriter, CustomHtmlStreamCellWriter>(lifetime);

            serviceCollection.Should().ContainDescriptor<IHtmlStreamWriter, HtmlStreamWriter>(lifetime);
            serviceCollection.Should().ContainDescriptor<IHtmlStreamCellWriter, CustomHtmlStreamCellWriter>(lifetime);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddHtmlStreamWriterWithCustomWriteAndCellWriterImplementationsShouldRegister(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddHtmlStreamWriter<CustomHtmlStreamWriter, CustomHtmlStreamCellWriter>(lifetime);

            serviceCollection.Should().ContainDescriptor<IHtmlStreamWriter, CustomHtmlStreamWriter>(lifetime);
            serviceCollection.Should().ContainDescriptor<IHtmlStreamCellWriter, CustomHtmlStreamCellWriter>(lifetime);
        }
    }
}
