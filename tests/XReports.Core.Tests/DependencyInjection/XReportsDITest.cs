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
                    o.AddHandlersByInterface<IHtmlPropertyHandler>();
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
                    o.AddHandlersByInterface<IHtmlPropertyHandler>();
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
