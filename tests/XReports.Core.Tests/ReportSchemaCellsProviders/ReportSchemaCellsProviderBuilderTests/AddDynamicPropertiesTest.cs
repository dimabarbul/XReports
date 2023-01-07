using System;
using XReports.Extensions;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.ReportSchemaCellsProviders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests
{
    public class AddDynamicPropertiesTest
    {
        [Fact]
        public void AddDynamicPropertiesShouldAddProperties()
        {
            ReportSchemaCellsProviderBuilder<int> builder = new ReportSchemaCellsProviderBuilder<int>(
                "Column", new ComputedValueReportCellsProvider<int, int>(x => x));

            builder.AddDynamicProperties(x => x > 0 ? (ReportCellProperty)new CustomProperty2() : new CustomProperty1());

            ReportSchemaCellsProvider<int> provider = builder.Build(Array.Empty<ReportCellProperty>());
            ReportCell headerCell = provider.CreateHeaderCell();
            ReportCell zeroCell = (ReportCell)provider.CreateCell(0).Clone();
            ReportCell oneCell = (ReportCell)provider.CreateCell(1).Clone();

            headerCell.Should().Be(new ReportCellData("Column"));
            zeroCell.Should().Be(new ReportCellData(0)
            {
                Properties = new[] { new CustomProperty1() },
            });
            oneCell.Should().Be(new ReportCellData(1)
            {
                Properties = new[] { new CustomProperty2() },
            });
        }

        [Fact]
        public void AddDynamicPropertiesShouldIgnoreNulls()
        {
            ReportSchemaCellsProviderBuilder<int> builder = new ReportSchemaCellsProviderBuilder<int>("Column", new ComputedValueReportCellsProvider<int, int>(x => x));

            builder.AddDynamicProperties(_ => new ReportCellProperty[]
            {
                new CustomProperty1(),
                new CustomProperty2(),
                null,
            });

            ReportSchemaCellsProvider<int> provider = builder.Build(Array.Empty<ReportCellProperty>());
            ReportCell cell = provider.CreateCell(0);

            cell.Should().Be(new ReportCellData(0)
            {
                Properties = new ReportCellProperty[]
                {
                    new CustomProperty1(),
                    new CustomProperty2(),
                },
            });
        }

        private class CustomProperty1 : ReportCellProperty
        {
        }

        private class CustomProperty2 : ReportCellProperty
        {
        }
    }
}
