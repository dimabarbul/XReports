using XReports.AttributeHandlers;
using XReports.Attributes;
using XReports.Enums;
using XReports.Interfaces;
using XReports.Models;
using XReports.Properties;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
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
        public void BuildSchemaShouldApplyGlobalPropertiesWhenOverwrittenForHeader()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                new CommonAttributeHandler(),
            });

            IReportSchema<WithOverwrittenForHeaderGlobalProperties> schema = builder.BuildSchema<WithOverwrittenForHeaderGlobalProperties>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
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
            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "ID", "From PostBuilder" },
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
                    string.Empty,
                },
            });
        }

        [Bold]
        private class WithGlobalProperties
        {
            [ReportVariable(1, "ID")]
            [Alignment(Alignment.Center)]
            public int Id { get; set; }
        }

        [Bold]
        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        private class WithPostBuilder
        {
            [ReportVariable(1, "ID")]
            [Alignment(Alignment.Center)]
            public int Id { get; set; }

            private class PostBuilder : IVerticalReportPostBuilder<WithPostBuilder>
            {
                public void Build(IVerticalReportSchemaBuilder<WithPostBuilder> builder)
                {
                    builder.AddColumn("From PostBuilder", new EmptyCellsProvider<WithPostBuilder>());
                }
            }
        }

        [DecimalPrecision(2)]
        private class WithOverwrittenGlobalProperties
        {
            [ReportVariable(1, "ID")]
            [DecimalPrecision(0)]
            public int Id { get; set; }
        }

        [Alignment(Alignment.Center)]
        private class WithOverwrittenForHeaderGlobalProperties
        {
            [ReportVariable(1, "ID")]
            [Alignment(Alignment.Right, IsHeader = true)]
            public int Id { get; set; }
        }
    }
}
