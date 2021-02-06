using FluentAssertions;
using XReports.AttributeHandlers;
using XReports.Attributes;
using XReports.Enums;
using XReports.Interfaces;
using XReports.Models;
using XReports.Properties;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Extensions.Builders.Tests.BuilderHelpers
{
    public partial class EntityAttributeBuilderHelperTest
    {
        [Fact]
        public void BuildVerticalReport_TableProperties_Applied()
        {
            AttributeBasedBuilder helper = new AttributeBasedBuilder(
                Mocks.ServiceProvider,
                new[] { new CommonAttributeHandler() });

            IReportSchema<WithTableProperties> schema = helper.BuildSchema<WithTableProperties>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(
                new[]
                {
                    new WithTableProperties() { Id = 1 },
                });

            ReportCell[][] headerCells = this.GetCellsAsArray(reportTable.HeaderRows);

            headerCells[0][0].Properties.Should().BeEmpty();

            ReportCell[][] cells = this.GetCellsAsArray(reportTable.Rows);

            cells[0][0].Properties.Should().HaveCount(2)
                .And.ContainSingle(p => p is BoldProperty)
                .And.ContainSingle(p => p is AlignmentProperty && ((AlignmentProperty)p).AlignmentType == AlignmentType.Center);
        }

        [Fact]
        public void BuildVerticalReport_TablePropertiesWhenOverwritten_NotApplied()
        {
            AttributeBasedBuilder helper = new AttributeBasedBuilder(
                Mocks.ServiceProvider,
                new[] { new CommonAttributeHandler() });

            IReportSchema<WithOverwrittenTableProperties> schema = helper.BuildSchema<WithOverwrittenTableProperties>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new WithOverwrittenTableProperties() { Id = 1 },
            });

            ReportCell[][] cells = this.GetCellsAsArray(reportTable.Rows);

            cells[0][0].Properties.Should().HaveCount(1)
                .And.ContainSingle(p => p is DecimalPrecisionProperty && ((DecimalPrecisionProperty)p).Precision == 0);
        }

        [Fact]
        public void BuildVerticalReport_TablePropertiesWhenOverwrittenForHeader_Applied()
        {
            AttributeBasedBuilder helper = new AttributeBasedBuilder(
                Mocks.ServiceProvider,
                new[] { new CommonAttributeHandler() });

            IReportSchema<WithOverwrittenForHeaderTableProperties> schema = helper.BuildSchema<WithOverwrittenForHeaderTableProperties>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(
                new[]
                {
                    new WithOverwrittenForHeaderTableProperties() { Id = 1 },
                });

            ReportCell[][] headerCells = this.GetCellsAsArray(reportTable.HeaderRows);

            headerCells[0][0].Properties.Should().HaveCount(1)
                .And.ContainSingle(p => p is AlignmentProperty && ((AlignmentProperty)p).AlignmentType == AlignmentType.Right);

            ReportCell[][] cells = this.GetCellsAsArray(reportTable.Rows);

            cells[0][0].Properties.Should().HaveCount(1)
                .And.ContainSingle(p => p is AlignmentProperty && ((AlignmentProperty)p).AlignmentType == AlignmentType.Center);
        }

        [VerticalReport]
        [Bold]
        private class WithTableProperties
        {
            [ReportVariable(1, "ID")]
            [Alignment(AlignmentType.Center)]
            public int Id { get; set; }
        }

        [VerticalReport]
        [DecimalPrecision(2)]
        private class WithOverwrittenTableProperties
        {
            [ReportVariable(1, "ID")]
            [DecimalPrecision(0)]
            public int Id { get; set; }
        }

        [VerticalReport]
        [Alignment(AlignmentType.Center)]
        private class WithOverwrittenForHeaderTableProperties
        {
            [ReportVariable(1, "ID")]
            [Alignment(AlignmentType.Right, IsHeader = true)]
            public int Id { get; set; }
        }
    }
}
