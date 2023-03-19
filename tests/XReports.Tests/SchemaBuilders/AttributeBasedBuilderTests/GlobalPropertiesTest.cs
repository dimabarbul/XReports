using XReports.ReportCellProperties;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.AttributeHandlers;
using XReports.SchemaBuilders.Attributes;
using XReports.SchemaBuilders.ReportCellProviders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public class GlobalPropertiesTest
    {
        [Fact]
        public void BuildSchemaShouldApplyGlobalProperties()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                new CommonAttributeHandler(),
            });

            IReportSchema<WithGlobalProperties> schema = builder.BuildSchema<WithGlobalProperties>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new WithGlobalProperties() { Id = 1 },
            });
            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[] { ReportCellHelper.CreateReportCell("ID") },
            });
            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell(
                        1, new BoldProperty(), new AlignmentProperty(Alignment.Center)),
                },
            });
        }

        [Fact]
        public void BuildSchemaShouldNotApplyGlobalPropertiesWhenOverwritten()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                new CommonAttributeHandler(),
            });

            IReportSchema<WithOverwrittenGlobalProperties> schema = builder.BuildSchema<WithOverwrittenGlobalProperties>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new WithOverwrittenGlobalProperties() { Id = 1 },
            });
            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[] { ReportCellHelper.CreateReportCell("ID") },
            });
            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell(1, new DecimalPrecisionProperty(0)),
                },
            });
        }

        [Fact]
        public void BuildSchemaShouldApplyGlobalPropertiesWhenOverwrittenForHeaderForVertical()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                new CommonAttributeHandler(),
            });

            IReportSchema<VerticalWithOverwrittenForHeaderGlobalProperties> schema = builder.BuildSchema<VerticalWithOverwrittenForHeaderGlobalProperties>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new VerticalWithOverwrittenForHeaderGlobalProperties() { Id = 1 },
            });
            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID", new AlignmentProperty(Alignment.Right)),
                },
            });
            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell(1, new AlignmentProperty(Alignment.Center)),
                },
            });
        }

        [Fact]
        public void BuildSchemaShouldApplyGlobalPropertiesWhenOverwrittenForHeaderForHorizontal()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                new CommonAttributeHandler(),
            });

            IReportSchema<HorizontalWithOverwrittenForHeaderGlobalProperties> schema = builder.BuildSchema<HorizontalWithOverwrittenForHeaderGlobalProperties>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new HorizontalWithOverwrittenForHeaderGlobalProperties() { Id = 1 },
            });
            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID", new AlignmentProperty(Alignment.Right)),
                    ReportCellHelper.CreateReportCell(1, new AlignmentProperty(Alignment.Center)),
                },
            });
        }

        [Fact]
        public void BuildSchemaShouldNotApplyGlobalPropertiesToColumnAddedInPostBuilder()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                new CommonAttributeHandler(),
            });

            IReportSchema<WithPostBuilder> schema = builder.BuildSchema<WithPostBuilder>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new WithPostBuilder() { Id = 1 },
            });
            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID"),
                    ReportCellHelper.CreateReportCell("From PostBuilder"),
                },
            });
            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell(
                        1, new BoldProperty(), new AlignmentProperty(Alignment.Center)),
                    ReportCellHelper.CreateReportCell(string.Empty),
                },
            });
        }

        [Bold]
        private class WithGlobalProperties
        {
            [ReportColumn(1, "ID")]
            [Alignment(Alignment.Center)]
            public int Id { get; set; }
        }

        [Bold]
        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        private class WithPostBuilder
        {
            [ReportColumn(1, "ID")]
            [Alignment(Alignment.Center)]
            public int Id { get; set; }

            private class PostBuilder : IReportPostBuilder<WithPostBuilder>
            {
                public void Build(IReportSchemaBuilder<WithPostBuilder> builder, BuildOptions options)
                {
                    builder.AddColumn("From PostBuilder", new EmptyCellProvider<WithPostBuilder>());
                }
            }
        }

        [DecimalPrecision(2)]
        private class WithOverwrittenGlobalProperties
        {
            [ReportColumn(1, "ID")]
            [DecimalPrecision(0)]
            public int Id { get; set; }
        }

        [Alignment(Alignment.Center)]
        private class VerticalWithOverwrittenForHeaderGlobalProperties
        {
            [ReportColumn(1, "ID")]
            [Alignment(Alignment.Right, IsHeader = true)]
            public int Id { get; set; }
        }

        [HorizontalReport]
        [Alignment(Alignment.Center)]
        private class HorizontalWithOverwrittenForHeaderGlobalProperties
        {
            [ReportColumn(1, "ID")]
            [Alignment(Alignment.Right, IsHeader = true)]
            public int Id { get; set; }
        }
    }
}
