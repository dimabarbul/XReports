using XReports.Attributes;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilder;
using XReports.SchemaBuilder.ReportCellsProviders;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class ComplexHeaderTest
    {
        [ComplexHeader(0, "Personal", 2, 3)]
        private class VerticalOneComplexHeaderByIndexes
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string FullName { get; set; }

            [ReportColumn(3, "Age")]
            public int Age { get; set; }
        }

        [ComplexHeader(0, "Personal", "Name", "Age")]
        private class VerticalOneComplexHeaderByTitles
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string FullName { get; set; }

            [ReportColumn(3, "Age")]
            public int Age { get; set; }
        }

        [ComplexHeader(0, "Personal", nameof(FullName), nameof(Age), true)]
        private class VerticalOneComplexHeaderByPropertyNames
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string FullName { get; set; }

            [ReportColumn(3, "Age")]
            public int Age { get; set; }
        }

        [HorizontalReport]
        [ComplexHeader(0, "Personal", 2, 3)]
        private class HorizontalOneComplexHeaderByIndexes
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string FullName { get; set; }

            [ReportColumn(3, "Age")]
            public int Age { get; set; }
        }

        [HorizontalReport]
        [ComplexHeader(0, "Personal", "Name", "Age")]
        private class HorizontalOneComplexHeaderByTitles
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string FullName { get; set; }

            [ReportColumn(3, "Age")]
            public int Age { get; set; }
        }

        [HorizontalReport]
        [ComplexHeader(0, "Personal", nameof(FullName), nameof(Age), true)]
        private class HorizontalOneComplexHeaderByPropertyNames
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string FullName { get; set; }

            [ReportColumn(3, "Age")]
            public int Age { get; set; }
        }

        [ComplexHeader(0, "Personal", 3, 5)]
        private class VerticalByIndexesWithGapsInIndexes
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(3, "Name")]
            public string FullName { get; set; }

            [ReportColumn(5, "Age")]
            public int Age { get; set; }
        }

        [HorizontalReport]
        [ComplexHeader(0, "Personal", 3, 5)]
        private class HorizontalByIndexesWithGapsInIndexes
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(3, "Name")]
            public string FullName { get; set; }

            [ReportColumn(5, "Age")]
            public int Age { get; set; }
        }

        [ComplexHeader(0, "Common", 1)]
        [ComplexHeader(0, "Employee Info", 2, 5)]
        [ComplexHeader(0, 2, "Dept. Info", 6)]
        [ComplexHeader(1, "Personal", "Name", "Age")]
        [ComplexHeader(1, "Job Info", nameof(JobTitle), nameof(Salary), true)]
        [ComplexHeader(2, "Sensitive", "Salary")]
        private class VerticalMultipleLevelsOfComplexHeader
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string FullName { get; set; }

            [ReportColumn(3, "Age")]
            public int Age { get; set; }

            [ReportColumn(4, "Job Title")]
            public string JobTitle { get; set; }

            [ReportColumn(5, "Salary")]
            public decimal Salary { get; set; }

            [ReportColumn(6, "Employee # in Department")]
            public int DepartmentEmployeeCount { get; set; }
        }

        [HorizontalReport]
        [ComplexHeader(0, "Common", 1)]
        [ComplexHeader(0, "Employee Info", 2, 5)]
        [ComplexHeader(0, 2, "Dept. Info", 6)]
        [ComplexHeader(1, "Personal", "Name", "Age")]
        [ComplexHeader(1, "Job Info", 4, 5)]
        [ComplexHeader(2, "Sensitive", "Salary")]
        private class HorizontalMultipleLevelsOfComplexHeader
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string FullName { get; set; }

            [ReportColumn(3, "Age")]
            public int Age { get; set; }

            [ReportColumn(4, "Job Title")]
            public string JobTitle { get; set; }

            [ReportColumn(5, "Salary")]
            public decimal Salary { get; set; }

            [ReportColumn(6, "Employee # in Department")]
            public int DepartmentEmployeeCount { get; set; }
        }

        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        [ComplexHeader(0, "Complex Header", 1, 2)]
        private class VerticalByIndexesWithColumnFromPostBuilder
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Age")]
            public string Age { get; set; }

            private class PostBuilder : IReportPostBuilder<VerticalByIndexesWithColumnFromPostBuilder>
            {
                public void Build(IReportSchemaBuilder<VerticalByIndexesWithColumnFromPostBuilder> builder, BuildOptions options)
                {
                    builder.InsertColumnBefore("Age", "Name", new EmptyCellsProvider<VerticalByIndexesWithColumnFromPostBuilder>());
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        [ComplexHeader(0, "Complex Header", 1, 2)]
        private class HorizontalByIndexesWithColumnFromPostBuilder
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Age")]
            public string Age { get; set; }

            private class PostBuilder : IReportPostBuilder<HorizontalByIndexesWithColumnFromPostBuilder>
            {
                public void Build(IReportSchemaBuilder<HorizontalByIndexesWithColumnFromPostBuilder> builder, BuildOptions options)
                {
                    builder.InsertColumnBefore("Age", "Name", new EmptyCellsProvider<HorizontalByIndexesWithColumnFromPostBuilder>());
                }
            }
        }

        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        [ComplexHeader(0, "Complex Header", "ID", "Age")]
        private class VerticalByTitlesWithColumnFromPostBuilder
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Age")]
            public string Age { get; set; }

            private class PostBuilder : IReportPostBuilder<VerticalByTitlesWithColumnFromPostBuilder>
            {
                public void Build(IReportSchemaBuilder<VerticalByTitlesWithColumnFromPostBuilder> builder, BuildOptions options)
                {
                    builder.InsertColumnBefore("Age", "Name", new EmptyCellsProvider<VerticalByTitlesWithColumnFromPostBuilder>());
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        [ComplexHeader(0, "Complex Header", "ID", "Age")]
        private class HorizontalByTitlesWithColumnFromPostBuilder
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Age")]
            public string Age { get; set; }

            private class PostBuilder : IReportPostBuilder<HorizontalByTitlesWithColumnFromPostBuilder>
            {
                public void Build(IReportSchemaBuilder<HorizontalByTitlesWithColumnFromPostBuilder> builder, BuildOptions options)
                {
                    builder.InsertColumnBefore("Age", "Name", new EmptyCellsProvider<HorizontalByTitlesWithColumnFromPostBuilder>());
                }
            }
        }

        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        [ComplexHeader(0, "Complex Header", nameof(Id), nameof(Age), true)]
        private class VerticalByPropertyNamesWithColumnFromPostBuilder
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Age")]
            public string Age { get; set; }

            private class PostBuilder : IReportPostBuilder<VerticalByPropertyNamesWithColumnFromPostBuilder>
            {
                public void Build(IReportSchemaBuilder<VerticalByPropertyNamesWithColumnFromPostBuilder> builder, BuildOptions options)
                {
                    builder.InsertColumnBefore("Age", "Name", new EmptyCellsProvider<VerticalByPropertyNamesWithColumnFromPostBuilder>());
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        [ComplexHeader(0, "Complex Header", nameof(Id), nameof(Age), true)]
        private class HorizontalByPropertyNamesWithColumnFromPostBuilder
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Age")]
            public string Age { get; set; }

            private class PostBuilder : IReportPostBuilder<HorizontalByPropertyNamesWithColumnFromPostBuilder>
            {
                public void Build(IReportSchemaBuilder<HorizontalByPropertyNamesWithColumnFromPostBuilder> builder, BuildOptions options)
                {
                    builder.InsertColumnBefore("Age", "Name", new EmptyCellsProvider<HorizontalByPropertyNamesWithColumnFromPostBuilder>());
                }
            }
        }

        [ComplexHeader(0, "Personal", 0, 1)]
        private class VerticalWithWrongStartIndex
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string FullName { get; set; }

            [ReportColumn(3, "Age")]
            public int Age { get; set; }
        }

        [ComplexHeader(0, "Personal", 1, 2)]
        private class VerticalWithWrongEndIndex
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(3, "Name")]
            public string FullName { get; set; }

            [ReportColumn(5, "Age")]
            public int Age { get; set; }
        }

        [HorizontalReport]
        [ComplexHeader(0, "Personal", 0, 1)]
        private class HorizontalWithWrongStartIndex
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string FullName { get; set; }

            [ReportColumn(3, "Age")]
            public int Age { get; set; }
        }

        [HorizontalReport]
        [ComplexHeader(0, "Personal", 1, 2)]
        private class HorizontalWithWrongEndIndex
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(3, "Name")]
            public string FullName { get; set; }

            [ReportColumn(5, "Age")]
            public int Age { get; set; }
        }
    }
}
