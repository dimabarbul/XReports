using Microsoft.Extensions.DependencyInjection;
using XReports.DependencyInjection;
using XReports.Html.Writers;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Tests.DependencyInjection
{
    public partial class HtmlStringWriterDITest
    {
        [Fact]
        public void AddHtmlStringWriterWithDefaultLifetimeShouldRegister()
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddHtmlStringWriter();

            serviceCollection.Should().ContainDescriptor<IHtmlStringWriter, HtmlStringWriter>(ServiceLifetime.Singleton);
            serviceCollection.Should().ContainDescriptor<IHtmlStringCellWriter, HtmlStringCellWriter>(ServiceLifetime.Singleton);
        }

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

        [Fact]
        public void AddHtmlStringWriterWithCustomWriterImplementationAndDefaultLifetimeShouldRegister()
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddHtmlStringWriter<CustomHtmlStringWriter>();

            serviceCollection.Should().ContainDescriptor<IHtmlStringWriter, CustomHtmlStringWriter>(ServiceLifetime.Singleton);
            serviceCollection.Should().ContainDescriptor<IHtmlStringCellWriter, HtmlStringCellWriter>(ServiceLifetime.Singleton);
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

        [Fact]
        public void AddHtmlStringWriterWithCustomCellWriterImplementationAndDefaultLifetimeShouldRegister()
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddHtmlStringWriter<HtmlStringWriter, CustomHtmlStringCellWriter>();

            serviceCollection.Should().ContainDescriptor<IHtmlStringWriter, HtmlStringWriter>(ServiceLifetime.Singleton);
            serviceCollection.Should().ContainDescriptor<IHtmlStringCellWriter, CustomHtmlStringCellWriter>(ServiceLifetime.Singleton);
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
