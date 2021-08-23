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
        public void AddReportConverter_WithCellType_CanBeRequested()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<ReportCell>()
                .BuildServiceProvider();

            IReportConverter<ReportCell> converter = serviceProvider.GetService<IReportConverter<ReportCell>>();

            converter.Should().NotBeNull();
        }

        [Fact]
        public void AddReportConverter_WithHandler_HasHandler()
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
        public void AddReportConverter_WithInterface_HasHandlersImplementingInterface()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell, IHtmlPropertyHandler>()
                .BuildServiceProvider();

            IReportConverter<HtmlCell> converter = serviceProvider.GetService<IReportConverter<HtmlCell>>();

            converter.Should().NotBeNull()
                .And.BeOfType<ReportConverter<HtmlCell>>();
            ((ReportConverter<HtmlCell>)converter).Handlers.Should()
                .HaveCount(2)
                .And.AllBeAssignableTo<IHtmlPropertyHandler>();
        }

        [Fact]
        public void AddReportConverter_WithHandlerAndInterface_HasHandlerAndAllHandlersImplementingInterface()
        {
            HtmlCellHandler cellHandler = new HtmlCellHandler();

            ServiceProvider serviceProvider = new ServiceCollection()
                .AddReportConverter<HtmlCell, IHtmlPropertyHandler>(cellHandler)
                .BuildServiceProvider();

            IReportConverter<HtmlCell> converter = serviceProvider.GetService<IReportConverter<HtmlCell>>();

            converter.Should().NotBeNull()
                .And.BeOfType<ReportConverter<HtmlCell>>();
            ((ReportConverter<HtmlCell>)converter).Handlers.Should()
                .HaveCount(3)
                .And.Contain(h =>
                    h == cellHandler
                    || h is IHtmlPropertyHandler);
        }

        [Fact]
        public void AddReportConverter_WithoutName_FactoryIsNotRegistered()
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
