using Microsoft.Extensions.DependencyInjection;
using XReports.DependencyInjection;
using XReports.Html.Writers;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Tests.DependencyInjection
{
    public partial class HtmlStreamWriterDITest
    {
        [Fact]
        public void AddHtmlStreamWriterWithDefaultLifetimeShouldRegister()
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddHtmlStreamWriter();

            serviceCollection.Should().ContainDescriptor<IHtmlStreamWriter, HtmlStreamWriter>(ServiceLifetime.Singleton);
            serviceCollection.Should().ContainDescriptor<IHtmlStreamCellWriter, HtmlStreamCellWriter>(ServiceLifetime.Singleton);
        }

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

        [Fact]
        public void AddHtmlStreamWriterWithCustomWriterImplementationAndDefaultLifetimeShouldRegister()
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddHtmlStreamWriter<CustomHtmlStreamWriter>();

            serviceCollection.Should().ContainDescriptor<IHtmlStreamWriter, CustomHtmlStreamWriter>(ServiceLifetime.Singleton);
            serviceCollection.Should().ContainDescriptor<IHtmlStreamCellWriter, HtmlStreamCellWriter>(ServiceLifetime.Singleton);
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

        [Fact]
        public void AddHtmlStreamWriterWithCustomCellWriterImplementationAndDefaultLifetimeShouldRegister()
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddHtmlStreamWriter<HtmlStreamWriter, CustomHtmlStreamCellWriter>();

            serviceCollection.Should().ContainDescriptor<IHtmlStreamWriter, HtmlStreamWriter>(ServiceLifetime.Singleton);
            serviceCollection.Should().ContainDescriptor<IHtmlStreamCellWriter, CustomHtmlStreamCellWriter>(ServiceLifetime.Singleton);
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
