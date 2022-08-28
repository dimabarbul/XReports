using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using XReports.DependencyInjection;
using XReports.Interfaces;
using Xunit;

namespace XReports.Core.Tests.DependencyInjection
{
    public partial class XReportsDITest
    {
        [Fact]
        public void AddReportConverter_ConverterWithName_CanBeRequested()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>("name")
                .BuildServiceProvider();

            IReportConverterFactory<HtmlCell> converterFactory =
                serviceProvider.GetService<IReportConverterFactory<HtmlCell>>();

            converterFactory.Should().NotBeNull();
            IReportConverter<HtmlCell> converter = converterFactory.Get("name");

            converter.Should().NotBeNull();
        }

        [Fact(Skip = "Find another way to validate handlers")]
        public void AddReportConverter_ConverterWithHandlerAndName_HasHandler()
        {
            HtmlCellHandler cellHandler = new HtmlCellHandler();

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>("name", cellHandler)
                .BuildServiceProvider();

            IReportConverterFactory<HtmlCell> converterFactory =
                serviceProvider.GetService<IReportConverterFactory<HtmlCell>>();

            converterFactory.Should().NotBeNull();
            IReportConverter<HtmlCell> _ = converterFactory.Get("name");
            // (converter as ReportConverter<HtmlCell>)?.Handlers.Should()
            //     .Equal(cellHandler);
        }

        [Fact(Skip = "Find another way to validate handlers")]
        public void AddReportConverter_ConverterWithInterfaceAndName_HasHandlersImplementingInterface()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell, IHtmlPropertyHandler>("name")
                .BuildServiceProvider();

            IReportConverterFactory<HtmlCell> converterFactory =
                serviceProvider.GetService<IReportConverterFactory<HtmlCell>>();

            converterFactory.Should().NotBeNull();
            IReportConverter<HtmlCell> _ = converterFactory.Get("name");
            // (converter as ReportConverter<HtmlCell>)?.Handlers.Should()
            //     .HaveCount(2)
            //     .And.AllBeAssignableTo<IHtmlPropertyHandler>();
        }

        [Fact(Skip = "Find another way to validate handlers")]
        public void AddReportConverter_ConverterWithHandlerAndInterfaceAndName_HasHandlerAndAllHandlersImplementingInterface()
        {
            HtmlCellHandler cellHandler = new HtmlCellHandler();

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell, IHtmlPropertyHandler>("name", cellHandler)
                .BuildServiceProvider();

            IReportConverterFactory<HtmlCell> converterFactory =
                serviceProvider.GetService<IReportConverterFactory<HtmlCell>>();

            converterFactory.Should().NotBeNull();
            IReportConverter<HtmlCell> _ = converterFactory.Get("name");
            // (converter as ReportConverter<HtmlCell>)?.Handlers.Should()
            //     .HaveCount(3)
            //     .And.Contain(h =>
            //         h == cellHandler
            //         || h is IHtmlPropertyHandler);
        }

        [Fact]
        public void AddReportConverter_ConverterWithWrongName_ThrowsException()
        {
            const string correctName = "name";
            const string wrongName = "another";
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>(correctName)
                .BuildServiceProvider();

            IReportConverterFactory<HtmlCell> converterFactory =
                serviceProvider.GetService<IReportConverterFactory<HtmlCell>>();

            converterFactory.Should().NotBeNull();
            Action action = () => converterFactory.Get(wrongName);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddReportConverter_ConverterWithNameRequestedTwice_ReturnsSameConverter()
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
    }
}
