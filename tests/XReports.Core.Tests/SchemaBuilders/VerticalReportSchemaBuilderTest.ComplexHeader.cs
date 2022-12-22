using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using XReports.ValueProviders;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders
{
    public partial class VerticalReportSchemaBuilderTest
    {
        [Fact]
        public void BuildShouldSupportOneComplexHeaderRow()
        {
            VerticalReportSchemaBuilder<int> reportBuilder = new VerticalReportSchemaBuilder<int>();
            reportBuilder.AddColumn("Value", i => i);
            reportBuilder.AddComplexHeader(0, "Statistics", "Value");

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[] { 1 });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "Statistics" },
                new[] { "Value" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { 1 },
            });
        }

        [Fact]
        public void BuildShouldSupportComplexHeaderRowsByColumnNames()
        {
            VerticalReportSchemaBuilder<(string Name, int Age)> reportBuilder =
                new VerticalReportSchemaBuilder<(string Name, int Age)>();
            reportBuilder.AddColumn("Name", x => x.Name);
            reportBuilder.AddColumn("Age", x => x.Age);
            reportBuilder.AddComplexHeader(0, "Personal Info", "Name", "Age");

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                ("John Doe", 30),
                ("Jane Doe", 27),
            });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Personal Info") { ColumnSpan = 2 },
                    null,
                },
                new object[] { "Name", "Age" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { "John Doe", 30 },
                new object[] { "Jane Doe", 27 },
            });
        }

        [Fact]
        public void BuildShouldSupportComplexHeaderRowsByColumnIndexes()
        {
            VerticalReportSchemaBuilder<(string Name, int Age, string Gender)> reportBuilder =
                new VerticalReportSchemaBuilder<(string Name, int Age, string Gender)>();
            reportBuilder.AddColumn("Name", x => x.Name);
            reportBuilder.AddColumn("Age", x => x.Age);
            reportBuilder.AddColumn("Gender", x => x.Gender);
            reportBuilder.AddComplexHeader(0, "Personal Info", 0, 2);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                ("John Doe", 30, "Male"),
                ("Jane Doe", 27, "Female"),
            });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Personal Info") { ColumnSpan = 3 },
                    null,
                    null,
                },
                new object[] { "Name", "Age", "Gender" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { "John Doe", 30, "Male" },
                new object[] { "Jane Doe", 27, "Female" },
            });
        }

        [Fact]
        public void BuildShouldSupportComplexHeaderWithMultipleGroupsInOneRowByColumnIndexes()
        {
            VerticalReportSchemaBuilder<(string Name, int Age, string Job, decimal Salary)> reportBuilder =
                new VerticalReportSchemaBuilder<(string Name, int Age, string Job, decimal Salary)>();
            reportBuilder.AddColumn("Name", x => x.Name);
            reportBuilder.AddColumn("Age", x => x.Age);
            reportBuilder.AddColumn("Job", x => x.Job);
            reportBuilder.AddColumn("Salary", x => x.Salary);
            reportBuilder.AddComplexHeader(0, "Personal Info", 0, 1);
            reportBuilder.AddComplexHeader(0, "Job Info", 2, 3);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                ("John Doe", 30, "Developer", 1000m),
            });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Personal Info") { ColumnSpan = 2 },
                    null,
                    new ReportCellData("Job Info") { ColumnSpan = 2 },
                    null,
                },
                new object[] { "Name", "Age", "Job", "Salary" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { "John Doe", 30, "Developer", 1000m },
            });
        }

        [Fact]
        public void BuildShouldSupportComplexHeaderNotSpanningAllColumns()
        {
            VerticalReportSchemaBuilder<(string Name, int Age)> reportBuilder =
                new VerticalReportSchemaBuilder<(string Name, int Age)>();
            reportBuilder.AddColumn("#", new SequentialNumberValueProvider());
            reportBuilder.AddColumn("Name", x => x.Name);
            reportBuilder.AddColumn("Age", x => x.Age);
            reportBuilder.AddComplexHeader(0, "Employee", 1, 2);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                ("John Doe", 30),
                ("Jane Doe", 30),
            });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("#") { RowSpan = 2 },
                    new ReportCellData("Employee") { ColumnSpan = 2 },
                    null,
                },
                new object[] { null, "Name", "Age" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { 1, "John Doe", 30 },
                new object[] { 2, "Jane Doe", 30 },
            });
        }

        [Fact]
        public void BuildShouldSupportMultipleComplexHeaderRows()
        {
            /*
             ------------------------------------
             | # |           Employee           |
             |   |      Personal Info     | Age |
             |   | First Name | Last Name |     |
             ------------------------------------
             | 1 | John       | Doe       | 30  |
             | 2 | Jane       | Doe       | 30  |
             ------------------------------------
             */
            VerticalReportSchemaBuilder<(string FirstName, string LastName, int Age)> reportBuilder =
                new VerticalReportSchemaBuilder<(string FirstName, string LastName, int Age)>();
            reportBuilder.AddColumn("#", new SequentialNumberValueProvider());
            reportBuilder.AddColumn("First Name", x => x.FirstName);
            reportBuilder.AddColumn("Last Name", x => x.LastName);
            reportBuilder.AddColumn("Age", x => x.Age);
            reportBuilder.AddComplexHeader(0, "Employee", "First Name", "Age");
            reportBuilder.AddComplexHeader(1, "Personal Info", "First Name", "Last Name");

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                ("John", "Doe", 30),
                ("Jane", "Doe", 30),
            });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("#") { RowSpan = 3 },
                    new ReportCellData("Employee") { ColumnSpan = 3 },
                    null,
                    null,
                },
                new object[]
                {
                    null,
                    new ReportCellData("Personal Info") { ColumnSpan = 2 },
                    null,
                    new ReportCellData("Age") { RowSpan = 2 },
                },
                new object[] { null, "First Name", "Last Name", null },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { 1, "John", "Doe", 30 },
                new object[] { 2, "Jane", "Doe", 30 },
            });
        }

        [Fact]
        public void BuildShouldSupportComplexHeaderWithMultipleGroupsInOneRowByColumnNames()
        {
            VerticalReportSchemaBuilder<(string FirstName, string LastName, int Age)> reportBuilder =
                new VerticalReportSchemaBuilder<(string FirstName, string LastName, int Age)>();
            reportBuilder.AddColumn("First Name", x => x.FirstName);
            reportBuilder.AddColumn("Last Name", x => x.LastName);
            reportBuilder.AddColumn("Age", x => x.Age);
            reportBuilder.AddComplexHeader(0, "Personal Info", "First Name", "Last Name");
            reportBuilder.AddComplexHeader(0, "Extra", "Age");

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                ("John", "Doe", 30),
                ("Jane", "Doe", 30),
            });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Personal Info") { ColumnSpan = 2 },
                    null,
                    "Extra",
                },
                new object[] { "First Name", "Last Name", "Age" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { "John", "Doe", 30 },
                new object[] { "Jane", "Doe", 30 },
            });
        }
    }
}
