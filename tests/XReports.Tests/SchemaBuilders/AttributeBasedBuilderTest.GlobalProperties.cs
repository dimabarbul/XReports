using FluentAssertions;
using XReports.AttributeHandlers;
using XReports.Attributes;
using XReports.Enums;
using XReports.Interfaces;
using XReports.Models;
using XReports.Properties;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class AttributeBasedBuilderTest
    {
        [Fact]
        public void BuildVerticalReportShouldApplyGlobalProperties()
        {
            AttributeBasedBuilder helper = new AttributeBasedBuilder(
                this.serviceProvider,
                new[] { new CommonAttributeHandler() });

            IReportSchema<WithGlobalProperties> schema = helper.BuildSchema<WithGlobalProperties>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(
                new[]
                {
                    new WithGlobalProperties() { Id = 1 },
                });

            ReportCell[][] headerCells = this.GetCellsAsArray(reportTable.HeaderRows);

            headerCells[0][0].Properties.Should().BeEmpty();

            ReportCell[][] cells = this.GetCellsAsArray(reportTable.Rows);

            cells[0][0].Properties.Should().HaveCount(2)
                .And.ContainSingle(p => p is BoldProperty)
                .And.ContainSingle(p => p is AlignmentProperty && ((AlignmentProperty)p).Alignment == Alignment.Center);
        }

        [Fact]
        public void BuildVerticalReportShouldNotApplyGlobalPropertiesWhenOverwritten()
        {
            AttributeBasedBuilder helper = new AttributeBasedBuilder(
                this.serviceProvider,
                new[] { new CommonAttributeHandler() });

            IReportSchema<WithOverwrittenGlobalProperties> schema = helper.BuildSchema<WithOverwrittenGlobalProperties>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new WithOverwrittenGlobalProperties() { Id = 1 },
            });

            ReportCell[][] cells = this.GetCellsAsArray(reportTable.Rows);

            cells[0][0].Properties.Should().HaveCount(1)
                .And.ContainSingle(p => p is DecimalPrecisionProperty && ((DecimalPrecisionProperty)p).Precision == 0);
        }

        [Fact]
        public void BuildVerticalReportShouldApplyGlobalPropertiesWhenOverwrittenForHeader()
        {
            AttributeBasedBuilder helper = new AttributeBasedBuilder(
                this.serviceProvider,
                new[] { new CommonAttributeHandler() });

            IReportSchema<WithOverwrittenForHeaderGlobalProperties> schema = helper.BuildSchema<WithOverwrittenForHeaderGlobalProperties>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(
                new[]
                {
                    new WithOverwrittenForHeaderGlobalProperties() { Id = 1 },
                });

            ReportCell[][] headerCells = this.GetCellsAsArray(reportTable.HeaderRows);

            headerCells[0][0].Properties.Should().HaveCount(1)
                .And.ContainSingle(p => p is AlignmentProperty && ((AlignmentProperty)p).Alignment == Alignment.Right);

            ReportCell[][] cells = this.GetCellsAsArray(reportTable.Rows);

            cells[0][0].Properties.Should().HaveCount(1)
                .And.ContainSingle(p => p is AlignmentProperty && ((AlignmentProperty)p).Alignment == Alignment.Center);
        }

        [VerticalReport]
        [Bold]
        private class WithGlobalProperties
        {
            [ReportVariable(1, "ID")]
            [Alignment(Alignment.Center)]
            public int Id { get; set; }
        }

        [VerticalReport]
        [DecimalPrecision(2)]
        private class WithOverwrittenGlobalProperties
        {
            [ReportVariable(1, "ID")]
            [DecimalPrecision(0)]
            public int Id { get; set; }
        }

        [VerticalReport]
        [Alignment(Alignment.Center)]
        private class WithOverwrittenForHeaderGlobalProperties
        {
            [ReportVariable(1, "ID")]
            [Alignment(Alignment.Right, IsHeader = true)]
            public int Id { get; set; }
        }
    }
}
