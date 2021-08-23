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
        public void ConverterWithName()
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

        [Fact]
        public void ConverterWithHandlerAndName()
        {
            HtmlCellHandler cellHandler = new HtmlCellHandler();

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>("name", cellHandler)
                .BuildServiceProvider();

            IReportConverterFactory<HtmlCell> converterFactory =
                serviceProvider.GetService<IReportConverterFactory<HtmlCell>>();

            converterFactory.Should().NotBeNull();
            IReportConverter<HtmlCell> converter = converterFactory.Get("name");
            (converter as ReportConverter<HtmlCell>)?.Handlers.Should()
                .Equal(cellHandler);
        }

        [Fact]
        public void ConverterWithInterfaceAndName()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell, IHtmlPropertyHandler>("name")
                .BuildServiceProvider();

            IReportConverterFactory<HtmlCell> converterFactory =
                serviceProvider.GetService<IReportConverterFactory<HtmlCell>>();

            converterFactory.Should().NotBeNull();
            IReportConverter<HtmlCell> converter = converterFactory.Get("name");
            (converter as ReportConverter<HtmlCell>)?.Handlers.Should()
                .HaveCount(1)
                .And.AllBeAssignableTo<IHtmlPropertyHandler>();
        }

        [Fact]
        public void ConverterWithHandlerAndInterfaceAndName()
        {
            HtmlCellHandler cellHandler = new HtmlCellHandler();

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell, IHtmlPropertyHandler>("name", cellHandler)
                .BuildServiceProvider();

            IReportConverterFactory<HtmlCell> converterFactory =
                serviceProvider.GetService<IReportConverterFactory<HtmlCell>>();

            converterFactory.Should().NotBeNull();
            IReportConverter<HtmlCell> converter = converterFactory.Get("name");
            (converter as ReportConverter<HtmlCell>)?.Handlers.Should()
                .HaveCount(2)
                .And.Contain(cellHandler)
                .And.ContainSingle(h => h is IHtmlPropertyHandler);
        }

        [Fact]
        public void ConverterWithWrongName()
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
        public void ConverterWithNameRequestedTwice()
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
