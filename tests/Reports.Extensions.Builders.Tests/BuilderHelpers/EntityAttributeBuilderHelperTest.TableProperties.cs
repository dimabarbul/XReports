using FluentAssertions;
using Reports.Core.Enums;
using Reports.Core.Interfaces;
using Reports.Core.Models;
using Reports.Extensions.AttributeBasedBuilder.Attributes;
using Reports.Extensions.Properties;
using Reports.Extensions.Properties.AttributeHandlers;
using Reports.Extensions.Properties.Attributes;
using Xunit;

namespace Reports.Extensions.Builders.Tests.BuilderHelpers
{
    public partial class EntityAttributeBuilderHelperTest
    {
        [VerticalReport]
        [Bold]
        private class WithTableProperties
        {
            [ReportVariable(1, "ID")]
            [Alignment(AlignmentType.Center)]
            public int Id { get; set; }
        }

        [Fact]
        public void BuildVerticalReport_TableProperties_Applied()
        {
            AttributeBasedBuilder.AttributeBasedBuilder helper = new AttributeBasedBuilder.AttributeBasedBuilder(Mocks.ServiceProvider);
            helper.AddAttributeHandler(new CommonAttributeHandler());

            IReportSchema<WithTableProperties> schema = helper.BuildSchema<WithTableProperties>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
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

        [VerticalReport]
        [DecimalFormat(2)]
        private class WithOverwrittenTableProperties
        {
            [ReportVariable(1, "ID")]
            [DecimalFormat(0)]
            public int Id { get; set; }
        }

        [Fact]
        public void BuildVerticalReport_TablePropertiesWhenOverwritten_NotApplied()
        {
            AttributeBasedBuilder.AttributeBasedBuilder helper = new AttributeBasedBuilder.AttributeBasedBuilder(Mocks.ServiceProvider);
            helper.AddAttributeHandler(new CommonAttributeHandler());

            IReportSchema<WithOverwrittenTableProperties> schema = helper.BuildSchema<WithOverwrittenTableProperties>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new WithOverwrittenTableProperties() { Id = 1 },
            });

            ReportCell[][] cells = this.GetCellsAsArray(reportTable.Rows);

            cells[0][0].Properties.Should().HaveCount(1)
                .And.ContainSingle(p => p is DecimalFormatProperty && ((DecimalFormatProperty)p).Precision == 0);
        }

        [VerticalReport]
        [Alignment(AlignmentType.Center)]
        private class WithOverwrittenForHeaderTableProperties
        {
            [ReportVariable(1, "ID")]
            [Alignment(AlignmentType.Right, IsHeader = true)]
            public int Id { get; set; }
        }

        [Fact]
        public void BuildVerticalReport_TablePropertiesWhenOverwrittenForHeader_Applied()
        {
            AttributeBasedBuilder.AttributeBasedBuilder helper = new AttributeBasedBuilder.AttributeBasedBuilder(Mocks.ServiceProvider);
            helper.AddAttributeHandler(new CommonAttributeHandler());

            IReportSchema<WithOverwrittenForHeaderTableProperties> schema = helper.BuildSchema<WithOverwrittenForHeaderTableProperties>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
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
    }
}
