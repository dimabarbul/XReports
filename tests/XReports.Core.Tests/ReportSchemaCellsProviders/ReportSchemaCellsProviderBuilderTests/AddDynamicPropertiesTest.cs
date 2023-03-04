using System;
using XReports.Extensions;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.ReportSchemaCellsProviders;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
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

            headerCell.Should().Equal(ReportCellHelper.CreateReportCell("Column"));
            zeroCell.Should().Equal(ReportCellHelper.CreateReportCell(0, new CustomProperty1()));
            oneCell.Should().Equal(ReportCellHelper.CreateReportCell(1, new CustomProperty2()));
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

            cell.Should().Equal(ReportCellHelper.CreateReportCell(0, new CustomProperty1(), new CustomProperty2()));
        }

        private class CustomProperty1 : ReportCellProperty
        {
        }

        private class CustomProperty2 : ReportCellProperty
        {
        }
    }
}
