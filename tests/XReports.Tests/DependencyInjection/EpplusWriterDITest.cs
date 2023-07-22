using System;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using XReports.DependencyInjection;
using XReports.Excel.Writers;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Tests.DependencyInjection
{
    public partial class EpplusWriterDITest
    {
        [Fact]
        public void AddEpplusWriterWithDefaultLifetimeShouldRegisterScoped()
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddEpplusWriter();

            serviceCollection.Should().ContainDescriptor<IEpplusWriter>(ServiceLifetime.Scoped);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddEpplusWriterWithLifetimeShouldRegister(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddEpplusWriter(lifetime: lifetime);

            serviceCollection.Should().ContainDescriptor<IEpplusWriter>(lifetime);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddEpplusWriterWithLifetimeAndFormattersShouldNotRegisterFormatters(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddEpplusWriter(configure: o => o.AddFromAssembly(Assembly.GetExecutingAssembly()), lifetime: lifetime);

            serviceCollection.Should().NotContainDescriptors<IEpplusFormatter>(lifetime, typeof(Formatter), typeof(BoldFormatter));
        }

        [Fact]
        public void AddEpplusWriterWithDefaultLifetimeShouldRegisterCustomImplementation()
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddEpplusWriter<CustomEpplusWriter>();

            serviceCollection.Should().ContainDescriptor<IEpplusWriter>(ServiceLifetime.Scoped);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddEpplusWriterShouldRegisterCustomImplementation(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddEpplusWriter<CustomEpplusWriter>(lifetime: lifetime);

            serviceCollection.Should().ContainDescriptor<IEpplusWriter>(lifetime);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddEpplusWriterShouldRegisterCustomImplementationAndNotFormatters(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddEpplusWriter<CustomEpplusWriter>(configure: o => o.AddFromAssembly(Assembly.GetExecutingAssembly()), lifetime: lifetime);

            serviceCollection.Should().ContainDescriptor<IEpplusWriter>(lifetime);
            serviceCollection.Should().NotContainDescriptors<IEpplusFormatter>(lifetime, typeof(Formatter), typeof(BoldFormatter));
        }

        [Fact]
        public void AddEpplusWriterShouldRegisterCustomImplementationWithDependency()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<Dependency>()
                .AddEpplusWriter<EpplusWriterWithDependency>(lifetime: ServiceLifetime.Singleton)
                .BuildServiceProvider(validateScopes: true);

            Action action = () => serviceProvider.GetService<IEpplusWriter>();

            action.Should().NotThrow();
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddEpplusWriterShouldRegisterCustomImplementationWithDependencyAndNotFormatters(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddSingleton<Dependency>()
                .AddEpplusWriter<EpplusWriterWithDependency>(configure: o => o.AddFromAssembly(Assembly.GetExecutingAssembly()), lifetime: lifetime);

            serviceCollection.Should().ContainDescriptor<IEpplusWriter>(lifetime);
            serviceCollection.Should().NotContainDescriptors<IEpplusFormatter>(lifetime, typeof(Formatter), typeof(BoldFormatter));
        }

        [Fact]
        public void AddEpplusWriterWithCustomOptionsShouldRegisterOptions()
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddEpplusWriter(o => o.WorksheetName = "Test");

            serviceCollection.Should().ContainDescriptor<IEpplusWriter>(ServiceLifetime.Scoped);
            serviceCollection.Should().ContainDescriptor<IConfigureOptions<EpplusWriterOptions>>(ServiceLifetime.Singleton);
        }

        [Fact]
        public void AddEpplusWriterWithCustomImplementationTypeAndCustomOptionsShouldRegisterOptions()
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddEpplusWriter<CustomEpplusWriter>(o => o.WorksheetName = "Test");

            serviceCollection.Should().ContainDescriptor<IEpplusWriter>(ServiceLifetime.Scoped);
            serviceCollection.Should().ContainDescriptor<IConfigureOptions<EpplusWriterOptions>>(ServiceLifetime.Singleton);
        }
    }
}
