using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using XReports.DependencyInjection;
using XReports.Interfaces;
using XReports.Tests.Assertions;
using XReports.Writers;
using Xunit;

namespace XReports.Tests.DependencyInjection
{
    public partial class EpplusWriterDITest
    {
        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddEpplusWriterWithLifetimeShouldRegister(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddEpplusWriter(lifetime);

            serviceCollection.Should().ContainDescriptor<IEpplusWriter, EpplusWriter>(lifetime);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddEpplusWriterWithLifetimeAndFormattersShouldRegisterFormattersAsWithTheLifetime(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddEpplusWriter(lifetime, o => o.AddFromAssembly(Assembly.GetExecutingAssembly()));

            serviceCollection.Should().ContainDescriptors<IEpplusFormatter>(lifetime, typeof(Formatter), typeof(BoldFormatter));
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped, ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton, ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Transient, ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton, ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient, ServiceLifetime.Singleton)]
        [InlineData(ServiceLifetime.Scoped, ServiceLifetime.Singleton)]
        public void AddEpplusWriterWithLifetimeShouldNotReregisterFormatterIfItHasBeenRegisteredBefore(ServiceLifetime writerLifetime, ServiceLifetime formatterLifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .Add(new ServiceDescriptor(typeof(IEpplusFormatter), typeof(Formatter), formatterLifetime))
                .AddEpplusWriter(writerLifetime, o => o.AddFromAssembly(Assembly.GetExecutingAssembly()));

            IEnumerable<ServiceDescriptor> formatterDescriptors = serviceCollection
                .Where(sd => sd.ServiceType == typeof(IEpplusFormatter));

            formatterDescriptors.Should()
                .OnlyContain(sd =>
                    sd.Lifetime == (
                        sd.ImplementationType == typeof(Formatter) ?
                            formatterLifetime :
                            writerLifetime));
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddEpplusWriterShouldRegisterCustomImplementation(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddEpplusWriter<CustomEpplusWriter>(lifetime);

            serviceCollection.Should().ContainDescriptor<IEpplusWriter, CustomEpplusWriter>(lifetime);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddEpplusWriterShouldRegisterCustomImplementationAndFormatters(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddEpplusWriter<CustomEpplusWriter>(lifetime, configure: o => o.AddFromAssembly(Assembly.GetExecutingAssembly()));

            serviceCollection.Should().ContainDescriptor<IEpplusWriter, CustomEpplusWriter>(lifetime);
            serviceCollection.Should().ContainDescriptors<IEpplusFormatter>(lifetime, typeof(Formatter), typeof(BoldFormatter));
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddEpplusWriterShouldRegisterCustomImplementationWithCustomInterface(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddEpplusWriter<IMyEpplusWriter, MyEpplusWriter>(lifetime);

            serviceCollection.Should().ContainDescriptor<IMyEpplusWriter, MyEpplusWriter>(lifetime);
            serviceCollection.Should().NotContainDescriptor<IEpplusWriter>();
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddEpplusWriterShouldRegisterCustomImplementationWithCustomInterfaceAndFormatters(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddEpplusWriter<IMyEpplusWriter, MyEpplusWriter>(
                    lifetime,
                    o => o.AddFromAssembly(Assembly.GetExecutingAssembly()));

            serviceCollection.Should().ContainDescriptor<IMyEpplusWriter, MyEpplusWriter>(lifetime);
            serviceCollection.Should().ContainDescriptors<IEpplusFormatter>(lifetime, typeof(Formatter), typeof(BoldFormatter));
        }

        [Fact]
        public void AddEpplusWriterShouldRegisterCustomImplementationWithDependency()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<Dependency>()
                .AddEpplusWriter<EpplusWriterWithDependency>(ServiceLifetime.Singleton)
                .BuildServiceProvider(validateScopes: true);

            Action action = () => serviceProvider.GetService<IEpplusWriter>();

            action.Should().NotThrow();
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddEpplusWriterShouldRegisterCustomImplementationWithDependencyAndFormatters(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddSingleton<Dependency>()
                .AddEpplusWriter<EpplusWriterWithDependency>(lifetime, configure: o => o.AddFromAssembly(Assembly.GetExecutingAssembly()));

            serviceCollection.Should().ContainDescriptor<IEpplusWriter, EpplusWriterWithDependency>(lifetime);
            serviceCollection.Should().ContainDescriptors<IEpplusFormatter>(lifetime, typeof(Formatter), typeof(BoldFormatter));
        }
    }
}
