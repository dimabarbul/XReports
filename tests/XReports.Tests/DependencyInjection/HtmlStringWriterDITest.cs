using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using XReports.DependencyInjection;
using XReports.Interfaces;
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

            this.ValidateLifetimeAndImplementations<HtmlStringWriter, HtmlStringCellWriter>(serviceCollection, lifetime);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddHtmlStringWriterWithCustomWriterImplementationShouldRegister(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddHtmlStringWriter<CustomHtmlStringWriter>(lifetime);

            this.ValidateLifetimeAndImplementations<CustomHtmlStringWriter, HtmlStringCellWriter>(serviceCollection, lifetime);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddHtmlStringWriterWithCustomCellWriterImplementationShouldRegister(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddHtmlStringWriter<HtmlStringWriter, CustomHtmlStringCellWriter>(lifetime);

            this.ValidateLifetimeAndImplementations<HtmlStringWriter, CustomHtmlStringCellWriter>(serviceCollection, lifetime);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddHtmlStringWriterWithCustomWriteAndCellWriterImplementationsShouldRegister(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddHtmlStringWriter<CustomHtmlStringWriter, CustomHtmlStringCellWriter>(lifetime);

            this.ValidateLifetimeAndImplementations<CustomHtmlStringWriter, CustomHtmlStringCellWriter>(serviceCollection, lifetime);
        }

        private void ValidateLifetimeAndImplementations<THtmlStringWriter, THtmlStringCellWriter>(
            IServiceCollection serviceCollection, ServiceLifetime lifetime)
            where THtmlStringWriter : IHtmlStringWriter
            where THtmlStringCellWriter : IHtmlStringCellWriter
        {
            ServiceDescriptor writerServiceDescriptor =
                serviceCollection.FirstOrDefault(sd => sd.ServiceType == typeof(IHtmlStringWriter));
            ServiceDescriptor cellWriterServiceDescriptor =
                serviceCollection.FirstOrDefault(sd => sd.ServiceType == typeof(IHtmlStringCellWriter));

            writerServiceDescriptor.Should().NotBeNull();
            writerServiceDescriptor.Lifetime.Should().Be(lifetime);
            writerServiceDescriptor.ImplementationType.Should().Be<THtmlStringWriter>();
            cellWriterServiceDescriptor.Should().NotBeNull();
            cellWriterServiceDescriptor.Lifetime.Should().Be(lifetime);
            cellWriterServiceDescriptor.ImplementationType.Should().Be<THtmlStringCellWriter>();
        }
    }
}
