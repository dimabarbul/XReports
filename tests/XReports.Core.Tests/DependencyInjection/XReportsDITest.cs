using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using XReports.DependencyInjection;
using XReports.Interfaces;
using XReports.Models;
using Xunit;

namespace XReports.Core.Tests.DependencyInjection
{
    public partial class XReportsDITest
    {
        private const string Name = "name";

        [Fact]
        public void AddReportConverterWithTransientLifetimeShouldRegister()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>(lifetime: ServiceLifetime.Transient)
                .BuildServiceProvider(validateScopes: true);

            IReportConverter<HtmlCell> converter1 = serviceProvider.GetService<IReportConverter<HtmlCell>>();
            IReportConverter<HtmlCell> converter2 = serviceProvider.GetService<IReportConverter<HtmlCell>>();

            converter1.Should().NotBeNull();
            converter2.Should().NotBeNull();
            converter1.Should().NotBe(converter2);
        }

        [Fact]
        public void AddReportConverterWithSingletonLifetimeShouldRegister()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>(lifetime: ServiceLifetime.Singleton)
                .BuildServiceProvider(validateScopes: true);

            IReportConverter<HtmlCell> converter1 = serviceProvider.GetService<IReportConverter<HtmlCell>>();
            IReportConverter<HtmlCell> converter2 = serviceProvider.GetService<IReportConverter<HtmlCell>>();

            converter1.Should().NotBeNull();
            converter2.Should().NotBeNull();
            converter1.Should().Be(converter2);
        }

        [Fact]
        public void AddReportConverterWithScopedLifetimeShouldRegister()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>(lifetime: ServiceLifetime.Scoped)
                .BuildServiceProvider(validateScopes: true);

            using (IServiceScope scope1 = serviceProvider.CreateScope())
            using (IServiceScope scope2 = serviceProvider.CreateScope())
            {
                IReportConverter<HtmlCell> converter1FromScope1 =
                    scope1.ServiceProvider.GetService<IReportConverter<HtmlCell>>();
                IReportConverter<HtmlCell> converter2FromScope1 =
                    scope1.ServiceProvider.GetService<IReportConverter<HtmlCell>>();

                IReportConverter<HtmlCell> converter1FromScope2 =
                    scope2.ServiceProvider.GetService<IReportConverter<HtmlCell>>();
                IReportConverter<HtmlCell> converter2FromScope2 =
                    scope2.ServiceProvider.GetService<IReportConverter<HtmlCell>>();

                converter1FromScope1.Should().NotBeNull().And.Be(converter2FromScope1);
                converter1FromScope2.Should().NotBeNull().And.Be(converter2FromScope2);
                converter1FromScope1.Should().NotBe(converter1FromScope2);
            }
        }

        [Fact]
        public void AddReportConverterWithNameAndTransientLifetimeShouldRegister()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>(Name, lifetime: ServiceLifetime.Transient)
                .BuildServiceProvider(validateScopes: true);

            IReportConverterFactory<HtmlCell> converterFactory1 = serviceProvider.GetService<IReportConverterFactory<HtmlCell>>();
            IReportConverterFactory<HtmlCell> converterFactory2 = serviceProvider.GetService<IReportConverterFactory<HtmlCell>>();

            converterFactory1.Should().NotBeNull();
            converterFactory2.Should().NotBeNull();
            converterFactory1.Should().NotBe(converterFactory2);
        }

        [Fact]
        public void AddReportConverterWithNameAndDifferentLifetimesShouldThrow()
        {
            Action action = () => new ServiceCollection()
                .AddReportConverter<HtmlCell>("name", lifetime: ServiceLifetime.Transient)
                .AddReportConverter<HtmlCell>("name2", lifetime: ServiceLifetime.Scoped);

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddReportConverterWithNameAndSingletonLifetimeShouldRegister()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>(Name, lifetime: ServiceLifetime.Singleton)
                .BuildServiceProvider(validateScopes: true);

            IReportConverterFactory<HtmlCell> converterFactory1 = serviceProvider.GetService<IReportConverterFactory<HtmlCell>>();
            IReportConverterFactory<HtmlCell> converterFactory2 = serviceProvider.GetService<IReportConverterFactory<HtmlCell>>();

            converterFactory1.Should().NotBeNull();
            converterFactory2.Should().NotBeNull();
            converterFactory1.Should().Be(converterFactory2);
        }

        [Fact]
        public void AddReportConverterWithNameAndScopedLifetimeShouldRegister()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>(Name, lifetime: ServiceLifetime.Scoped)
                .BuildServiceProvider(validateScopes: true);

            using (IServiceScope scope1 = serviceProvider.CreateScope())
            using (IServiceScope scope2 = serviceProvider.CreateScope())
            {
                IReportConverterFactory<HtmlCell> converterFactory1FromScope1 =
                    scope1.ServiceProvider.GetService<IReportConverterFactory<HtmlCell>>();
                IReportConverterFactory<HtmlCell> converterFactory2FromScope1 =
                    scope1.ServiceProvider.GetService<IReportConverterFactory<HtmlCell>>();

                IReportConverterFactory<HtmlCell> converterFactory1FromScope2 =
                    scope2.ServiceProvider.GetService<IReportConverterFactory<HtmlCell>>();
                IReportConverterFactory<HtmlCell> converterFactory2FromScope2 =
                    scope2.ServiceProvider.GetService<IReportConverterFactory<HtmlCell>>();

                converterFactory1FromScope1.Should().NotBeNull().And.Be(converterFactory2FromScope1);
                converterFactory1FromScope2.Should().NotBeNull().And.Be(converterFactory2FromScope2);
                converterFactory1FromScope1.Should().NotBe(converterFactory1FromScope2);
            }
        }

        [Fact]
        public void AddReportConverterShouldRegisterWhenNoHandlersProvided()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>()
                .BuildServiceProvider();

            IReportConverter<HtmlCell> converter = serviceProvider.GetService<IReportConverter<HtmlCell>>();

            converter.Should().NotBeNull();
            IPropertyHandler<HtmlCell>[] handlers = this.GetPropertyHandlers(converter);
            handlers.Should().BeEmpty();
        }

        [Fact]
        public void AddReportConverterShouldRegisterWithHandlersWhenInterfaceProvidedAsGenericArgument()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell, IHtmlPropertyHandler>()
                .BuildServiceProvider();

            IReportConverter<HtmlCell> converter = serviceProvider.GetService<IReportConverter<HtmlCell>>();

            converter.Should().NotBeNull();
            IPropertyHandler<HtmlCell>[] handlers = this.GetPropertyHandlers(converter);
            handlers.Select(h => h.GetType())
                .Should().BeEquivalentTo(typeof(HtmlCellHandlerCustomInterface), typeof(HtmlCellAnotherHandlerCustomInterface));
        }

        [Fact]
        public void AddReportConverterShouldRegisterWithHandlersWhenInterfaceProvidedInOptions()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>(o =>
                {
                    o.AddHandlersByBaseType<IHtmlPropertyHandler>();
                })
                .BuildServiceProvider();

            IReportConverter<HtmlCell> converter = serviceProvider.GetService<IReportConverter<HtmlCell>>();

            converter.Should().NotBeNull();
            IPropertyHandler<HtmlCell>[] handlers = this.GetPropertyHandlers(converter);
            handlers.Select(h => h.GetType())
                .Should().BeEquivalentTo(typeof(HtmlCellHandlerCustomInterface), typeof(HtmlCellAnotherHandlerCustomInterface));
        }

        [Fact]
        public void AddReportConverterWithNameShouldRegisterWhenNoHandlersProvided()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>(Name)
                .BuildServiceProvider();

            IReportConverterFactory<HtmlCell> converterFactory =
                serviceProvider.GetService<IReportConverterFactory<HtmlCell>>();

            converterFactory.Should().NotBeNull();
            IReportConverter<HtmlCell> converter = converterFactory.Get(Name);

            converter.Should().NotBeNull();
            IPropertyHandler<HtmlCell>[] handlers = this.GetPropertyHandlers(converter);
            handlers.Should().BeEmpty();
        }

        [Fact]
        public void AddReportConverterWithNameShouldRegisterWithHandlersWhenInterfaceProvidedAsGenericArgument()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell, IHtmlPropertyHandler>(Name)
                .BuildServiceProvider();

            IReportConverterFactory<HtmlCell> converterFactory =
                serviceProvider.GetRequiredService<IReportConverterFactory<HtmlCell>>();
            IReportConverter<HtmlCell> converter = converterFactory.Get(Name);

            converter.Should().NotBeNull();
            IPropertyHandler<HtmlCell>[] handlers = this.GetPropertyHandlers(converter);
            handlers.Select(h => h.GetType())
                .Should().BeEquivalentTo(typeof(HtmlCellHandlerCustomInterface), typeof(HtmlCellAnotherHandlerCustomInterface));
        }

        [Fact]
        public void AddReportConverterWithNameShouldRegisterWithHandlersWhenInterfaceProvidedInOptions()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>(Name, o =>
                {
                    o.AddHandlersByBaseType<IHtmlPropertyHandler>();
                })
                .BuildServiceProvider();

            IReportConverterFactory<HtmlCell> converterFactory =
                serviceProvider.GetRequiredService<IReportConverterFactory<HtmlCell>>();
            IReportConverter<HtmlCell> converter = converterFactory.Get(Name);

            converter.Should().NotBeNull();
            IPropertyHandler<HtmlCell>[] handlers = this.GetPropertyHandlers(converter);
            handlers.Select(h => h.GetType())
                .Should().BeEquivalentTo(typeof(HtmlCellHandlerCustomInterface), typeof(HtmlCellAnotherHandlerCustomInterface));
        }

        [Fact]
        public void AddReportConverterWithNameShouldThrowExceptionWhenTryingToGetConverterWithWrongName()
        {
            const string correctName = "name";
            const string wrongName = "another";
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>(correctName)
                .BuildServiceProvider();

            IReportConverterFactory<HtmlCell> converterFactory =
                serviceProvider.GetRequiredService<IReportConverterFactory<HtmlCell>>();
            Action action = () => converterFactory.Get(wrongName);

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddReportConverterWithNameShouldReturnsSameConverterWhenRequestedTwice()
        {
            const string name = "name";

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>(name)
                .BuildServiceProvider();

            IReportConverterFactory<HtmlCell> converterFactory =
                serviceProvider.GetService<IReportConverterFactory<HtmlCell>>();

            IReportConverter<HtmlCell> firstConverter = converterFactory.Get(name);
            IReportConverter<HtmlCell> secondConverter = converterFactory.Get(name);

            firstConverter.Should().BeSameAs(secondConverter);
        }

        [Fact]
        public void AddReportConverterWithNameShouldThrowExceptionWhenRegisteredWithExistingNameAndType()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>(Name)
                .AddReportConverter<HtmlCell>(Name)
                .BuildServiceProvider();

            Action action = () => serviceProvider.GetService<IReportConverterFactory<HtmlCell>>();

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddReportConverterWithNameShouldRegisterWhenRegisteredWithExistingNameButDifferentType()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>(Name)
                .AddReportConverter<ReportCell>(Name)
                .BuildServiceProvider();

            IReportConverter<HtmlCell> htmlConverter = serviceProvider.GetRequiredService<IReportConverterFactory<HtmlCell>>().Get(Name);
            IReportConverter<ReportCell> reportConverter = serviceProvider.GetService<IReportConverterFactory<ReportCell>>().Get(Name);

            htmlConverter.Should().NotBeNull();
            reportConverter.Should().NotBeNull();
            htmlConverter.Should().NotBe(reportConverter);
        }

        private IPropertyHandler<TReportCell>[] GetPropertyHandlers<TReportCell>(IReportConverter<TReportCell> converter)
            where TReportCell : BaseReportCell, new()
        {
            ReportConverter<TReportCell> reportConverter = (ReportConverter<TReportCell>)converter;

            FieldInfo fieldInfo = typeof(ReportConverter<TReportCell>).GetField("handlers", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo == null)
            {
                throw new ArgumentException("Field \"handlers\" is not found in converter.", nameof(converter));
            }

            return fieldInfo
                .GetValue(reportConverter) as IPropertyHandler<TReportCell>[];
        }
    }
}
