using System;
using System.Linq;
using FluentAssertions;
using XReports.Interfaces;
using XReports.Schema;
using XReports.SchemaBuilder;
using XReports.SchemaBuilders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    /// <summary>
    /// Adding complex header using attributes is just a wrapper around adding
    /// complex header using <see cref="ReportSchemaBuilder{TSourceEntity}"/>
    /// , so these tests are not covering all scenarios.
    /// </summary>
    public partial class ComplexHeaderTest
    {
        [Fact]
        public void BuildReportTableShouldBuildComplexHeaderWhenColumnsAreReferredByIndexesForVerticalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<VerticalOneComplexHeaderByIndexes> schema = builder.BuildSchema<VerticalOneComplexHeaderByIndexes>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<VerticalOneComplexHeaderByIndexes>());

            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID", rowSpan: 2),
                    ReportCellHelper.CreateReportCell("Personal", columnSpan: 2),
                    null,
                },
                new[]
                {
                    null,
                    ReportCellHelper.CreateReportCell("Name"),
                    ReportCellHelper.CreateReportCell("Age"),
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldBuildComplexHeaderWhenRowsAreReferredByIndexesForHorizontalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<HorizontalOneComplexHeaderByIndexes> schema = builder.BuildSchema<HorizontalOneComplexHeaderByIndexes>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<HorizontalOneComplexHeaderByIndexes>());

            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID", columnSpan: 2),
                    null,
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Personal", rowSpan: 2),
                    ReportCellHelper.CreateReportCell("Name"),
                },
                new[]
                {
                    null,
                    ReportCellHelper.CreateReportCell("Age"),
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldBuildComplexHeaderWhenColumnsAreReferredByTitlesForVerticalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<VerticalOneComplexHeaderByTitles> schema = builder.BuildSchema<VerticalOneComplexHeaderByTitles>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<VerticalOneComplexHeaderByTitles>());

            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID", rowSpan: 2),
                    ReportCellHelper.CreateReportCell("Personal", columnSpan: 2),
                    null,
                },
                new[]
                {
                    null,
                    ReportCellHelper.CreateReportCell("Name"),
                    ReportCellHelper.CreateReportCell("Age"),
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldBuildComplexHeaderWhenRowsAreReferredByTitlesForHorizontalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<HorizontalOneComplexHeaderByTitles> schema = builder.BuildSchema<HorizontalOneComplexHeaderByTitles>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<HorizontalOneComplexHeaderByTitles>());

            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID", columnSpan: 2),
                    null,
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Personal", rowSpan: 2),
                    ReportCellHelper.CreateReportCell("Name"),
                },
                new[]
                {
                    null,
                    ReportCellHelper.CreateReportCell("Age"),
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldBuildComplexHeaderWhenColumnsAreReferredByPropertyNamesForVerticalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<VerticalOneComplexHeaderByPropertyNames> schema = builder.BuildSchema<VerticalOneComplexHeaderByPropertyNames>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<VerticalOneComplexHeaderByPropertyNames>());

            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID", rowSpan: 2),
                    ReportCellHelper.CreateReportCell("Personal", columnSpan: 2),
                    null,
                },
                new[]
                {
                    null,
                    ReportCellHelper.CreateReportCell("Name"),
                    ReportCellHelper.CreateReportCell("Age"),
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldBuildComplexHeaderWhenRowsAreReferredByPropertyNamesForHorizontalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<HorizontalOneComplexHeaderByPropertyNames> schema = builder.BuildSchema<HorizontalOneComplexHeaderByPropertyNames>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<HorizontalOneComplexHeaderByPropertyNames>());

            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID", columnSpan: 2),
                    null,
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Personal", rowSpan: 2),
                    ReportCellHelper.CreateReportCell("Name"),
                },
                new[]
                {
                    null,
                    ReportCellHelper.CreateReportCell("Age"),
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldBuildComplexHeaderWhenGapInColumnIndexesForVerticalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<VerticalByIndexesWithGapsInIndexes> schema = builder.BuildSchema<VerticalByIndexesWithGapsInIndexes>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<VerticalByIndexesWithGapsInIndexes>());

            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID", rowSpan: 2),
                    ReportCellHelper.CreateReportCell("Personal", columnSpan: 2),
                    null,
                },
                new[]
                {
                    null,
                    ReportCellHelper.CreateReportCell("Name"),
                    ReportCellHelper.CreateReportCell("Age"),
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldBuildComplexHeaderWhenGapInRowIndexesForHorizontalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<HorizontalByIndexesWithGapsInIndexes> schema = builder.BuildSchema<HorizontalByIndexesWithGapsInIndexes>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<HorizontalByIndexesWithGapsInIndexes>());

            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID", columnSpan: 2),
                    null,
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Personal", rowSpan: 2),
                    ReportCellHelper.CreateReportCell("Name"),
                },
                new[]
                {
                    null,
                    ReportCellHelper.CreateReportCell("Age"),
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldSupportMultipleLevelsOfComplexHeaderForVertical()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<VerticalMultipleLevelsOfComplexHeader> schema = builder.BuildSchema<VerticalMultipleLevelsOfComplexHeader>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<VerticalMultipleLevelsOfComplexHeader>());

            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Common"),
                    ReportCellHelper.CreateReportCell("Employee Info", columnSpan: 4),
                    null,
                    null,
                    null,
                    ReportCellHelper.CreateReportCell("Dept. Info", rowSpan: 2),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID", rowSpan: 3),
                    ReportCellHelper.CreateReportCell("Personal", columnSpan: 2),
                    null,
                    ReportCellHelper.CreateReportCell("Job Info", columnSpan: 2),
                    null,
                    null,
                },
                new[]
                {
                    null,
                    ReportCellHelper.CreateReportCell("Name", rowSpan: 2),
                    ReportCellHelper.CreateReportCell("Age", rowSpan: 2),
                    ReportCellHelper.CreateReportCell("Job Title", rowSpan: 2),
                    ReportCellHelper.CreateReportCell("Sensitive"),
                    ReportCellHelper.CreateReportCell("Employee # in Department", rowSpan: 2),
                },
                new[]
                {
                    null,
                    null,
                    null,
                    null,
                    ReportCellHelper.CreateReportCell("Salary"),
                    null,
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldSupportMultipleLevelsOfComplexHeaderForHorizontal()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<HorizontalMultipleLevelsOfComplexHeader> schema = builder.BuildSchema<HorizontalMultipleLevelsOfComplexHeader>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<HorizontalMultipleLevelsOfComplexHeader>());

            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Common"),
                    ReportCellHelper.CreateReportCell("ID", columnSpan: 3),
                    null,
                    null,
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Employee Info", rowSpan: 4),
                    ReportCellHelper.CreateReportCell("Personal", rowSpan: 2),
                    ReportCellHelper.CreateReportCell("Name", columnSpan: 2),
                    null,
                },
                new[]
                {
                    null,
                    null,
                    ReportCellHelper.CreateReportCell("Age", columnSpan: 2),
                    null,
                },
                new[]
                {
                    null,
                    ReportCellHelper.CreateReportCell("Job Info", rowSpan: 2),
                    ReportCellHelper.CreateReportCell("Job Title", columnSpan: 2),
                    null,
                },
                new[]
                {
                    null,
                    null,
                    ReportCellHelper.CreateReportCell("Sensitive"),
                    ReportCellHelper.CreateReportCell("Salary"),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Dept. Info", columnSpan: 2),
                    null,
                    ReportCellHelper.CreateReportCell("Employee # in Department", columnSpan: 2),
                    null,
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldNotSpanColumnsAddedInPostBuilderWhenColumnsAreReferredByIndexesForVertical()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<VerticalByIndexesWithColumnFromPostBuilder> schema = builder.BuildSchema<VerticalByIndexesWithColumnFromPostBuilder>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<VerticalByIndexesWithColumnFromPostBuilder>());

            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Complex Header", columnSpan: 2),
                    null,
                    ReportCellHelper.CreateReportCell("Age", rowSpan: 2),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID"),
                    ReportCellHelper.CreateReportCell("Name"),
                    null,
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldNotSpanColumnsAddedInPostBuilderWhenRowsAreReferredByIndexesForHorizontal()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<HorizontalByIndexesWithColumnFromPostBuilder> schema = builder.BuildSchema<HorizontalByIndexesWithColumnFromPostBuilder>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<HorizontalByIndexesWithColumnFromPostBuilder>());

            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Complex Header", rowSpan: 2),
                    ReportCellHelper.CreateReportCell("ID"),
                },
                new[]
                {
                    null,
                    ReportCellHelper.CreateReportCell("Name"),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Age", columnSpan: 2),
                    null,
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldSpanColumnsAddedInPostBuilderWhenColumnsAreReferredByTitlesForVertical()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<VerticalByTitlesWithColumnFromPostBuilder> schema = builder.BuildSchema<VerticalByTitlesWithColumnFromPostBuilder>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<VerticalByTitlesWithColumnFromPostBuilder>());

            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Complex Header", columnSpan: 3),
                    null,
                    null,
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID"),
                    ReportCellHelper.CreateReportCell("Name"),
                    ReportCellHelper.CreateReportCell("Age"),
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldSpanColumnsAddedInPostBuilderWhenRowsAreReferredByTitlesForHorizontal()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<HorizontalByTitlesWithColumnFromPostBuilder> schema = builder.BuildSchema<HorizontalByTitlesWithColumnFromPostBuilder>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<HorizontalByTitlesWithColumnFromPostBuilder>());

            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Complex Header", rowSpan: 3),
                    ReportCellHelper.CreateReportCell("ID"),
                },
                new[]
                {
                    null,
                    ReportCellHelper.CreateReportCell("Name"),
                },
                new[]
                {
                    null,
                    ReportCellHelper.CreateReportCell("Age"),
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldSpanColumnsAddedInPostBuilderWhenColumnsAreReferredByPropertyNamesForVertical()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<VerticalByPropertyNamesWithColumnFromPostBuilder> schema = builder.BuildSchema<VerticalByPropertyNamesWithColumnFromPostBuilder>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<VerticalByPropertyNamesWithColumnFromPostBuilder>());

            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Complex Header", columnSpan: 3),
                    null,
                    null,
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID"),
                    ReportCellHelper.CreateReportCell("Name"),
                    ReportCellHelper.CreateReportCell("Age"),
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldSpanColumnsAddedInPostBuilderWhenRowsAreReferredByPropertyNamesForHorizontal()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<HorizontalByPropertyNamesWithColumnFromPostBuilder> schema = builder.BuildSchema<HorizontalByPropertyNamesWithColumnFromPostBuilder>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<HorizontalByPropertyNamesWithColumnFromPostBuilder>());

            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Complex Header", rowSpan: 3),
                    ReportCellHelper.CreateReportCell("ID"),
                },
                new[]
                {
                    null,
                    ReportCellHelper.CreateReportCell("Name"),
                },
                new[]
                {
                    null,
                    ReportCellHelper.CreateReportCell("Age"),
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldThrowWhenStartIndexDoesNotExistForVertical()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            Action action = () => builder.BuildSchema<VerticalWithWrongStartIndex>();

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void BuildReportTableShouldThrowWhenEndIndexDoesNotExistForVertical()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            Action action = () => builder.BuildSchema<VerticalWithWrongEndIndex>();

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void BuildReportTableShouldThrowWhenStartIndexDoesNotExistForHorizontal()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            Action action = () => builder.BuildSchema<HorizontalWithWrongStartIndex>();

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void BuildReportTableShouldThrowWhenEndIndexDoesNotExistForHorizontal()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            Action action = () => builder.BuildSchema<HorizontalWithWrongEndIndex>();

            action.Should().ThrowExactly<ArgumentException>();
        }
    }
}
