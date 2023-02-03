using System;
using System.Linq;
using FluentAssertions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    /// <summary>
    /// Adding complex header using attributes is just a wrapper around adding
    /// complex header using <see cref="VerticalReportSchemaBuilder{TSourceEntity}"/>
    /// or <see cref="HorizontalReportSchemaBuilder{TSourceEntity}"/>, so these tests
    /// are not covering all scenarios.
    /// </summary>
    public partial class ComplexHeaderTest
    {
        [Fact]
        public void BuildReportTableShouldBuildComplexHeaderWhenColumnsAreReferredByIndexesForVerticalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<VerticalOneComplexHeaderByIndexes> schema = builder.BuildSchema<VerticalOneComplexHeaderByIndexes>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<VerticalOneComplexHeaderByIndexes>());

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("ID") { RowSpan = 2 },
                    new ReportCellData("Personal") { ColumnSpan = 2 },
                    null,
                },
                new object[] { null, "Name", "Age" },
            });
        }

        [Fact]
        public void BuildReportTableShouldBuildComplexHeaderWhenRowsAreReferredByIndexesForHorizontalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<HorizontalOneComplexHeaderByIndexes> schema = builder.BuildSchema<HorizontalOneComplexHeaderByIndexes>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<HorizontalOneComplexHeaderByIndexes>());

            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("ID") { ColumnSpan = 2 },
                    null,
                },
                new object[]
                {
                    new ReportCellData("Personal") { RowSpan = 2 },
                    "Name",
                },
                new object[]
                {
                    null,
                    "Age",
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldBuildComplexHeaderWhenColumnsAreReferredByTitlesForVerticalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<VerticalOneComplexHeaderByTitles> schema = builder.BuildSchema<VerticalOneComplexHeaderByTitles>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<VerticalOneComplexHeaderByTitles>());

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("ID") { RowSpan = 2 },
                    new ReportCellData("Personal") { ColumnSpan = 2 },
                    null,
                },
                new object[] { null, "Name", "Age" },
            });
        }

        [Fact]
        public void BuildReportTableShouldBuildComplexHeaderWhenRowsAreReferredByTitlesForHorizontalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<HorizontalOneComplexHeaderByTitles> schema = builder.BuildSchema<HorizontalOneComplexHeaderByTitles>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<HorizontalOneComplexHeaderByTitles>());

            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("ID") { ColumnSpan = 2 },
                    null,
                },
                new object[]
                {
                    new ReportCellData("Personal") { RowSpan = 2 },
                    "Name",
                },
                new object[]
                {
                    null,
                    "Age",
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldBuildComplexHeaderWhenGapInColumnIndexesForVerticalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<VerticalByIndexesWithGapsInIndexes> schema = builder.BuildSchema<VerticalByIndexesWithGapsInIndexes>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<VerticalByIndexesWithGapsInIndexes>());

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("ID") { RowSpan = 2 },
                    new ReportCellData("Personal") { ColumnSpan = 2 },
                    null,
                },
                new object[] { null, "Name", "Age" },
            });
        }

        [Fact]
        public void BuildReportTableShouldBuildComplexHeaderWhenGapInRowIndexesForHorizontalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<HorizontalByIndexesWithGapsInIndexes> schema = builder.BuildSchema<HorizontalByIndexesWithGapsInIndexes>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<HorizontalByIndexesWithGapsInIndexes>());

            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("ID") { ColumnSpan = 2 },
                    null,
                },
                new object[]
                {
                    new ReportCellData("Personal") { RowSpan = 2 },
                    "Name",
                },
                new object[]
                {
                    null,
                    "Age",
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldSupportMultipleLevelsOfComplexHeaderForVertical()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<VerticalMultipleLevelsOfComplexHeader> schema = builder.BuildSchema<VerticalMultipleLevelsOfComplexHeader>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<VerticalMultipleLevelsOfComplexHeader>());

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    "Common",
                    new ReportCellData("Employee Info") { ColumnSpan = 4 },
                    null,
                    null,
                    null,
                    new ReportCellData("Dept. Info") { RowSpan = 2 },
                },
                new object[]
                {
                    new ReportCellData("ID") { RowSpan = 3 },
                    new ReportCellData("Personal") { ColumnSpan = 2 },
                    null,
                    new ReportCellData("Job Info") { ColumnSpan = 2 },
                    null,
                    null,
                },
                new object[]
                {
                    null,
                    new ReportCellData("Name") { RowSpan = 2 },
                    new ReportCellData("Age") { RowSpan = 2 },
                    new ReportCellData("Job Title") { RowSpan = 2 },
                    "Sensitive",
                    new ReportCellData("Employee # in Department") { RowSpan = 2 },
                },
                new object[]
                {
                    null,
                    null,
                    null,
                    null,
                    "Salary",
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

            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    "Common",
                    new ReportCellData("ID") { ColumnSpan = 3 },
                    null,
                    null,
                },
                new object[]
                {
                    new ReportCellData("Employee Info") { RowSpan = 4 },
                    new ReportCellData("Personal") { RowSpan = 2 },
                    new ReportCellData("Name") { ColumnSpan = 2 },
                    null,
                },
                new object[]
                {
                    null,
                    null,
                    new ReportCellData("Age") { ColumnSpan = 2 },
                    null,
                },
                new object[]
                {
                    null,
                    new ReportCellData("Job Info") { RowSpan = 2 },
                    new ReportCellData("Job Title") { ColumnSpan = 2 },
                    null,
                },
                new object[]
                {
                    null,
                    null,
                    "Sensitive",
                    "Salary",
                },
                new object[]
                {
                    new ReportCellData("Dept. Info") { ColumnSpan = 2 },
                    null,
                    new ReportCellData("Employee # in Department") { ColumnSpan = 2 },
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

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Complex Header") { ColumnSpan = 2 },
                    null,
                    new ReportCellData("Age") { RowSpan = 2 },
                },
                new object[]
                {
                    "ID",
                    "Name",
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

            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Complex Header") { RowSpan = 2 },
                    "ID",
                },
                new object[]
                {
                    null,
                    "Name",
                },
                new object[]
                {
                    new ReportCellData("Age") { ColumnSpan = 2 },
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

            reportTable.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Complex Header") { ColumnSpan = 3 },
                    null,
                    null,
                },
                new object[]
                {
                    "ID",
                    "Name",
                    "Age",
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldSpanColumnsAddedInPostBuilderWhenRowsAreReferredByTitlesForHorizontal()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());
            IReportSchema<HorizontalByTitlesWithColumnFromPostBuilder> schema = builder.BuildSchema<HorizontalByTitlesWithColumnFromPostBuilder>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<HorizontalByTitlesWithColumnFromPostBuilder>());

            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Complex Header") { RowSpan = 3 },
                    "ID",
                },
                new object[]
                {
                    null,
                    "Name",
                },
                new object[]
                {
                    null,
                    "Age",
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
