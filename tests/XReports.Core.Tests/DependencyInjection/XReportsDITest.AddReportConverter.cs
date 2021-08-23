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
        [Fact]
        public void ConverterToReportCell()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<ReportCell>()
                .BuildServiceProvider();

            IReportConverter<ReportCell> converter = serviceProvider.GetService<IReportConverter<ReportCell>>();

            converter.Should().NotBeNull();
        }

        [Fact]
        public void ConverterWithHandler()
        {
            HtmlCellHandler cellHandler = new HtmlCellHandler();

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell>(cellHandler)
                .BuildServiceProvider();

            IReportConverter<HtmlCell> converter = serviceProvider.GetService<IReportConverter<HtmlCell>>();

            converter.Should().NotBeNull()
                .And.BeOfType<ReportConverter<HtmlCell>>();
            ((ReportConverter<HtmlCell>)converter).Handlers.Should()
                .Equal(cellHandler);
        }

        [Fact]
        public void ConverterWithInterface()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell, IHtmlPropertyHandler>()
                .BuildServiceProvider();

            IReportConverter<HtmlCell> converter = serviceProvider.GetService<IReportConverter<HtmlCell>>();

            converter.Should().NotBeNull()
                .And.BeOfType<ReportConverter<HtmlCell>>();
            ((ReportConverter<HtmlCell>)converter).Handlers.Should()
                .HaveCount(1)
                .And.AllBeAssignableTo<IHtmlPropertyHandler>();
        }

        [Fact]
        public void ConverterWithHandlerAndInterface()
        {
            HtmlCellHandler cellHandler = new HtmlCellHandler();

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell, IHtmlPropertyHandler>(cellHandler)
                .BuildServiceProvider();

            IReportConverter<HtmlCell> converter = serviceProvider.GetService<IReportConverter<HtmlCell>>();

            converter.Should().NotBeNull()
                .And.BeOfType<ReportConverter<HtmlCell>>();
            ((ReportConverter<HtmlCell>)converter).Handlers.Should()
                .HaveCount(2)
                .And.Contain(cellHandler)
                .And.ContainSingle(h => h is IHtmlPropertyHandler);
        }

        [Fact]
        public void ConverterWithoutNameThroughFactory()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell, IHtmlPropertyHandler>()
                .BuildServiceProvider();

            IReportConverterFactory<HtmlCell> converterFactory =
                serviceProvider.GetService<IReportConverterFactory<HtmlCell>>();

            converterFactory.Should().BeNull();
        }
    }
}
