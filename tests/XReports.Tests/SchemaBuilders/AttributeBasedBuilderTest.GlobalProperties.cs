using XReports.AttributeHandlers;
using XReports.Attributes;
using XReports.Enums;
using XReports.Interfaces;
using XReports.Models;
using XReports.Properties;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class AttributeBasedBuilderTest
    {
        [Fact]
        public void BuildVerticalReportShouldApplyGlobalProperties()
        {
            AttributeBasedBuilder helper = new(
                this.serviceProvider,
                new[] { new CommonAttributeHandler() });

            IReportSchema<WithGlobalProperties> schema = helper.BuildSchema<WithGlobalProperties>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(
                new[]
                {
                    new WithGlobalProperties() { Id = 1 },
                });

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "ID" },
            });
            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData(1)
                    {
                        Properties = new ReportCellProperty[]
                        {
                            new BoldProperty(),
                            new AlignmentProperty(Alignment.Center),
                        },
                    },
                },
            });
        }

        [Fact]
        public void BuildVerticalReportShouldNotApplyGlobalPropertiesWhenOverwritten()
        {
            AttributeBasedBuilder helper = new(
                this.serviceProvider,
                new[] { new CommonAttributeHandler() });

            IReportSchema<WithOverwrittenGlobalProperties> schema = helper.BuildSchema<WithOverwrittenGlobalProperties>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new WithOverwrittenGlobalProperties() { Id = 1 },
            });

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "ID" },
            });
            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData(1)
                    {
                        Properties = new[]
                        {
                            new DecimalPrecisionProperty(0),
                        },
                    },
                },
            });
        }

        [Fact]
        public void BuildVerticalReportShouldApplyGlobalPropertiesWhenOverwrittenForHeader()
        {
            AttributeBasedBuilder helper = new(
                this.serviceProvider,
                new[] { new CommonAttributeHandler() });

            IReportSchema<WithOverwrittenForHeaderGlobalProperties> schema = helper.BuildSchema<WithOverwrittenForHeaderGlobalProperties>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(
                new[]
                {
                    new WithOverwrittenForHeaderGlobalProperties() { Id = 1 },
                });

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[]
                {
                    new ReportCellData("ID")
                    {
                        Properties = new[]
                        {
                            new AlignmentProperty(Alignment.Right),
                        },
                    },
                },
            });
            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData(1)
                    {
                        Properties = new[]
                        {
                            new AlignmentProperty(Alignment.Center),
                        },
                    },
                },
            });
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
