using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using XReports.DependencyInjection;
using XReports.Interfaces;
using Xunit;

namespace XReports.Tests.DependencyInjection
{
    public partial class EpplusWriterDITest
    {
        [Fact]
        public void AddEpplusWriterWithTransientLifetimeShouldRegister()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddEpplusWriter(ServiceLifetime.Transient)
                .BuildServiceProvider(validateScopes: true);

            IEpplusWriter epplusWriter = serviceProvider.GetService<IEpplusWriter>();
            IEpplusWriter epplusWriter2 = serviceProvider.GetService<IEpplusWriter>();

            epplusWriter.Should().NotBeNull();
            epplusWriter2.Should().NotBeNull();
            epplusWriter.Should().NotBe(epplusWriter2);
        }

        [Fact]
        public void AddEpplusWriterWithScopedLifetimeShouldRegister()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddEpplusWriter(ServiceLifetime.Scoped)
                .BuildServiceProvider(validateScopes: true);

            using (IServiceScope scope1 = serviceProvider.CreateScope())
            using (IServiceScope scope2 = serviceProvider.CreateScope())
            {
                IEpplusWriter epplusWriter1FromScope1 =
                    scope1.ServiceProvider.GetService<IEpplusWriter>();
                IEpplusWriter epplusWriter2FromScope1 =
                    scope1.ServiceProvider.GetService<IEpplusWriter>();

                IEpplusWriter epplusWriter1FromScope2 =
                    scope2.ServiceProvider.GetService<IEpplusWriter>();
                IEpplusWriter epplusWriter2FromScope2 =
                    scope2.ServiceProvider.GetService<IEpplusWriter>();

                epplusWriter1FromScope1.Should().NotBeNull().And.Be(epplusWriter2FromScope1);
                epplusWriter1FromScope2.Should().NotBeNull().And.Be(epplusWriter2FromScope2);
                epplusWriter1FromScope1.Should().NotBe(epplusWriter1FromScope2);
            }
        }

        [Fact]
        public void AddEpplusWriterWithSingletonLifetimeShouldRegister()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddEpplusWriter(ServiceLifetime.Singleton)
                .BuildServiceProvider(validateScopes: true);

            IEpplusWriter epplusWriter1 = serviceProvider.GetService<IEpplusWriter>();
            IEpplusWriter epplusWriter2 = serviceProvider.GetService<IEpplusWriter>();

            epplusWriter1.Should().NotBeNull();
            epplusWriter2.Should().NotBeNull();
            epplusWriter1.Should().Be(epplusWriter2);
        }

        [Fact]
        public void AddEpplusWriterWithSingletonLifetimeShouldRegisterFormattersAsSingleton()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddEpplusWriter(ServiceLifetime.Singleton)
                .BuildServiceProvider(validateScopes: true);

            IEnumerable<IEpplusFormatter> formatters = serviceProvider.GetService<IEnumerable<IEpplusFormatter>>();
            IEnumerable<IEpplusFormatter> formatters2 = serviceProvider.GetService<IEnumerable<IEpplusFormatter>>();

            formatters.Should().BeSameAs(formatters2);
        }

        [Fact]
        public void AddEpplusWriterWithSingletonLifetimeShouldNotRegisterFormatterAsSingletonIfItHasBeenRegisteredBefore()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddTransient<IEpplusFormatter, Formatter>()
                .AddEpplusWriter(ServiceLifetime.Singleton, o => o.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .BuildServiceProvider(validateScopes: true);

            IEnumerable<IEpplusFormatter> formatters = serviceProvider
                .GetService<IEnumerable<IEpplusFormatter>>()
                .ToArray();
            IEpplusFormatter formatter = formatters.FirstOrDefault(f => f is Formatter);
            IEpplusFormatter boldFormatter = formatters.FirstOrDefault(f => f is BoldFormatter);
            IEnumerable<IEpplusFormatter> formatters2 = serviceProvider
                .GetService<IEnumerable<IEpplusFormatter>>()
                .ToArray();
            IEpplusFormatter formatter2 = formatters2.FirstOrDefault(f => f is Formatter);
            IEpplusFormatter boldFormatter2 = formatters2.FirstOrDefault(f => f is BoldFormatter);

            formatter.Should().NotBeNull();
            formatter2.Should().NotBeNull();
            formatter.Should().NotBe(formatter2);
            boldFormatter.Should().NotBeNull();
            boldFormatter2.Should().NotBeNull();
            boldFormatter.Should().Be(boldFormatter2);
        }

        [Fact]
        public void AddEpplusWriterShouldRegisterWhenNoFormattersProvided()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddEpplusWriter(ServiceLifetime.Transient)
                .BuildServiceProvider(validateScopes: true);

            IEpplusWriter epplusWriter = serviceProvider.GetService<IEpplusWriter>();

            epplusWriter.Should().NotBeNull();
        }

        [Fact]
        public void AddEpplusWriterShouldRegisterWhenCorrectFormattersProvided()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddEpplusWriter(ServiceLifetime.Singleton, o => o.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .BuildServiceProvider(validateScopes: true);

            IEpplusWriter epplusWriter = serviceProvider.GetService<IEpplusWriter>();
            IEnumerable<IEpplusFormatter> formatters = serviceProvider.GetServices<IEpplusFormatter>();

            epplusWriter.Should().NotBeNull();
            formatters.Select(f => f.GetType()).Should().BeEquivalentTo(typeof(Formatter), typeof(BoldFormatter));
        }

        [Fact]
        public void AddEpplusWriterShouldRegisterCustomImplementation()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddEpplusWriter<CustomEpplusWriter>(ServiceLifetime.Singleton)
                .BuildServiceProvider(validateScopes: true);

            IEpplusWriter epplusWriter = serviceProvider.GetService<IEpplusWriter>();

            epplusWriter.Should().NotBeNull().And.BeOfType<CustomEpplusWriter>();
        }

        [Fact]
        public void AddEpplusWriterShouldRegisterCustomImplementationAndFormatters()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddEpplusWriter<CustomEpplusWriter>(ServiceLifetime.Singleton, configure: o => o.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .BuildServiceProvider(validateScopes: true);

            IEpplusWriter epplusWriter = serviceProvider.GetService<IEpplusWriter>();
            IEnumerable<IEpplusFormatter> formatters = serviceProvider.GetServices<IEpplusFormatter>();

            epplusWriter.Should().NotBeNull().And.BeOfType<CustomEpplusWriter>();
            formatters.Select(f => f.GetType()).Should().BeEquivalentTo(typeof(Formatter), typeof(BoldFormatter));
        }

        [Fact]
        public void AddEpplusWriterShouldRegisterCustomImplementationWithCustomInterface()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddEpplusWriter<IMyEpplusWriter, MyEpplusWriter>(ServiceLifetime.Singleton)
                .BuildServiceProvider(validateScopes: true);

            IEpplusWriter epplusWriter = serviceProvider.GetService<IEpplusWriter>();
            IEpplusWriter myWriter = serviceProvider.GetService<IMyEpplusWriter>();

            epplusWriter.Should().NotBeNull();
            myWriter.Should().NotBeNull();
            epplusWriter.Should().Be(myWriter);
        }

        [Fact]
        public void AddEpplusWriterShouldRegisterCustomImplementationWithCustomInterfaceAndFormatters()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddEpplusWriter<IMyEpplusWriter, MyEpplusWriter>(
                    ServiceLifetime.Singleton,
                    o => o.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .BuildServiceProvider(validateScopes: true);

            IEpplusWriter epplusWriter = serviceProvider.GetService<IEpplusWriter>();
            IEpplusWriter myWriter = serviceProvider.GetService<IMyEpplusWriter>();
            IEnumerable<IEpplusFormatter> formatters = serviceProvider.GetServices<IEpplusFormatter>();

            epplusWriter.Should().NotBeNull();
            myWriter.Should().NotBeNull();
            epplusWriter.Should().Be(myWriter);
            formatters.Select(f => f.GetType()).Should().BeEquivalentTo(typeof(Formatter), typeof(BoldFormatter));
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

        [Fact]
        public void AddEpplusWriterShouldRegisterCustomImplementationWithFormattersAndDependency()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<Dependency>()
                .AddEpplusWriter<EpplusWriterWithDependency>(ServiceLifetime.Singleton, configure: o => o.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .BuildServiceProvider(validateScopes: true);

            IEpplusWriter epplusWriter = serviceProvider.GetService<IEpplusWriter>();
            IEnumerable<IEpplusFormatter> formatters = serviceProvider.GetServices<IEpplusFormatter>();

            epplusWriter.Should().NotBeNull().And.BeOfType<EpplusWriterWithDependency>();
            formatters.Select(f => f.GetType()).Should().BeEquivalentTo(typeof(Formatter), typeof(BoldFormatter));
        }
    }
}
