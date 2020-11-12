using FluentAssertions;
using Reports.Builders;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.Models;
using Reports.ValueProviders;
using Xunit;

namespace Reports.Tests.Builders
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void Build_OneColumnTwoHeaderRows_CorrectCells()
        {
            VerticalReportBuilder<int> reportBuilder = new VerticalReportBuilder<int>();
            reportBuilder.AddColumn("Value", i => i);
            reportBuilder.AddComplexHeader(0, "Statistics", "Value");

            IReportTable<ReportCell> table = reportBuilder.Build(new[] { 1 });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(2);
            headerCells[0][0].GetValue<string>().Should().Be("Statistics");
            headerCells[1][0].GetValue<string>().Should().Be("Value");

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].GetValue<int>().Should().Be(1);
        }

        [Fact]
        public void Build_TwoColumnsTwoHeaderRowsByColumnName_CorrectCells()
        {
            VerticalReportBuilder<(string Name, int Age)> reportBuilder = new VerticalReportBuilder<(string Name, int Age)>();
            reportBuilder.AddColumn("Name", x => x.Name);
            reportBuilder.AddColumn("Age", x => x.Age);
            reportBuilder.AddComplexHeader(0, "Personal Info", "Name", "Age");

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                ("John Doe", 30),
                ("Jane Doe", 27),
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(2);
            headerCells[0][0].GetValue<string>().Should().Be("Personal Info");
            headerCells[0][0].ColumnSpan.Should().Be(2);
            headerCells[0][1].Should().BeNull();
            headerCells[1][0].GetValue<string>().Should().Be("Name");
            headerCells[1][0].ColumnSpan.Should().Be(1);
            headerCells[1][1].GetValue<string>().Should().Be("Age");

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].GetValue<string>().Should().Be("John Doe");
            cells[0][0].ColumnSpan.Should().Be(1);
            cells[0][1].GetValue<int>().Should().Be(30);
            cells[1][0].GetValue<string>().Should().Be("Jane Doe");
            cells[1][0].ColumnSpan.Should().Be(1);
            cells[1][1].GetValue<int>().Should().Be(27);
        }

        [Fact]
        public void Build_ThreeColumnsTwoHeaderRowsByColumnIndexes_CorrectCells()
        {
            VerticalReportBuilder<(string Name, int Age, string Gender)> reportBuilder = new VerticalReportBuilder<(string Name, int Age, string Gender)>();
            reportBuilder.AddColumn("Name", x => x.Name);
            reportBuilder.AddColumn("Age", x => x.Age);
            reportBuilder.AddColumn("Gender", x => x.Gender);
            reportBuilder.AddComplexHeader(0, "Personal Info", 0, 2);

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                ("John Doe", 30, "Male"),
                ("Jane Doe", 27, "Female"),
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(2);
            headerCells[0][0].GetValue<string>().Should().Be("Personal Info");
            headerCells[0][0].ColumnSpan.Should().Be(3);
            headerCells[0][1].Should().BeNull();
            headerCells[0][2].Should().BeNull();
            headerCells[1][0].GetValue<string>().Should().Be("Name");
            headerCells[1][1].GetValue<string>().Should().Be("Age");
            headerCells[1][2].GetValue<string>().Should().Be("Gender");

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].GetValue<string>().Should().Be("John Doe");
            cells[0][1].GetValue<int>().Should().Be(30);
            cells[0][2].GetValue<string>().Should().Be("Male");
            cells[1][0].GetValue<string>().Should().Be("Jane Doe");
            cells[1][1].GetValue<int>().Should().Be(27);
            cells[1][2].GetValue<string>().Should().Be("Female");
        }

        [Fact]
        public void Build_TwoHeaderGroups_CorrectCells()
        {
            VerticalReportBuilder<(string Name, int Age, string Job, decimal Salary)> reportBuilder =
                new VerticalReportBuilder<(string Name, int Age, string Job, decimal Salary)>();
            reportBuilder.AddColumn("Name", x => x.Name);
            reportBuilder.AddColumn("Age", x => x.Age);
            reportBuilder.AddColumn("Job", x => x.Job);
            reportBuilder.AddColumn("Salary", x => x.Salary);
            reportBuilder.AddComplexHeader(0, "Personal Info", 0, 1);
            reportBuilder.AddComplexHeader(0, "Job Info", 2, 3);

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                ("John Doe", 30, "Developer", 1000m),
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(2);
            headerCells[0][0].GetValue<string>().Should().Be("Personal Info");
            headerCells[0][0].ColumnSpan.Should().Be(2);
            headerCells[0][1].Should().BeNull();
            headerCells[0][2].GetValue<string>().Should().Be("Job Info");
            headerCells[0][2].ColumnSpan.Should().Be(2);
            headerCells[0][3].Should().BeNull();
            headerCells[1][0].GetValue<string>().Should().Be("Name");
            headerCells[1][1].GetValue<string>().Should().Be("Age");
            headerCells[1][2].GetValue<string>().Should().Be("Job");
            headerCells[1][3].GetValue<string>().Should().Be("Salary");

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].GetValue<string>().Should().Be("John Doe");
            cells[0][1].GetValue<int>().Should().Be(30);
            cells[0][2].GetValue<string>().Should().Be("Developer");
            cells[0][3].GetValue<decimal>().Should().Be(1000m);
        }

        [Fact]
        public void Build_ColumnOutOfHeaderGroup_CorrectCells()
        {
            VerticalReportBuilder<(string Name, int Age)> reportBuilder =
                new VerticalReportBuilder<(string Name, int Age)>();
            reportBuilder.AddColumn("#", new SequentialNumberValueProvider());
            reportBuilder.AddColumn("Name", x => x.Name);
            reportBuilder.AddColumn("Age", x => x.Age);
            reportBuilder.AddComplexHeader(0, "Employee", 1, 2);

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                ("John Doe", 30),
                ("Jane Doe", 30),
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(2);
            headerCells[0][0].GetValue<string>().Should().Be("#");
            headerCells[0][0].RowSpan.Should().Be(2);
            headerCells[0][0].ColumnSpan.Should().Be(1);
            headerCells[0][1].GetValue<string>().Should().Be("Employee");
            headerCells[0][1].ColumnSpan.Should().Be(2);
            headerCells[0][1].RowSpan.Should().Be(1);
            headerCells[0][2].Should().BeNull();
            headerCells[1][0].Should().BeNull();
            headerCells[1][1].GetValue<string>().Should().Be("Name");
            headerCells[1][2].GetValue<string>().Should().Be("Age");

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].GetValue<int>().Should().Be(1);
            cells[0][1].GetValue<string>().Should().Be("John Doe");
            cells[0][2].GetValue<int>().Should().Be(30);
            cells[1][0].GetValue<int>().Should().Be(2);
            cells[1][1].GetValue<string>().Should().Be("Jane Doe");
            cells[1][2].GetValue<int>().Should().Be(30);
        }

        [Fact]
        public void Build_ThreeHeaderRows_CorrectCells()
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
            VerticalReportBuilder<(string FirstName, string LastName, int Age)> reportBuilder =
                new VerticalReportBuilder<(string FirstName, string LastName, int Age)>();
            reportBuilder.AddColumn("#", new SequentialNumberValueProvider());
            reportBuilder.AddColumn("First Name", x => x.FirstName);
            reportBuilder.AddColumn("Last Name", x => x.LastName);
            reportBuilder.AddColumn("Age", x => x.Age);
            reportBuilder.AddComplexHeader(0, "Employee", "First Name", "Age");
            reportBuilder.AddComplexHeader(1, "Personal Info", "First Name", "Last Name");

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                ("John", "Doe", 30),
                ("Jane", "Doe", 30),
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(3);
            headerCells[0][0].GetValue<string>().Should().Be("#");
            headerCells[0][0].RowSpan.Should().Be(3);
            headerCells[0][0].ColumnSpan.Should().Be(1);
            headerCells[0][1].GetValue<string>().Should().Be("Employee");
            headerCells[0][1].ColumnSpan.Should().Be(3);
            headerCells[0][1].RowSpan.Should().Be(1);
            headerCells[0][2].Should().BeNull();
            headerCells[0][3].Should().BeNull();

            headerCells[1][0].Should().BeNull();
            headerCells[1][1].GetValue<string>().Should().Be("Personal Info");
            headerCells[1][1].RowSpan.Should().Be(1);
            headerCells[1][1].ColumnSpan.Should().Be(2);
            headerCells[1][2].Should().BeNull();
            headerCells[1][3].GetValue<string>().Should().Be("Age");
            headerCells[1][3].ColumnSpan.Should().Be(1);
            headerCells[1][3].RowSpan.Should().Be(2);

            headerCells[2][0].Should().BeNull();
            headerCells[2][1].GetValue<string>().Should().Be("First Name");
            headerCells[2][1].RowSpan.Should().Be(1);
            headerCells[2][1].ColumnSpan.Should().Be(1);
            headerCells[2][2].GetValue<string>().Should().Be("Last Name");
            headerCells[2][2].ColumnSpan.Should().Be(1);
            headerCells[2][2].RowSpan.Should().Be(1);
            headerCells[2][3].Should().BeNull();

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].GetValue<int>().Should().Be(1);
            cells[0][1].GetValue<string>().Should().Be("John");
            cells[0][2].GetValue<string>().Should().Be("Doe");
            cells[0][3].GetValue<int>().Should().Be(30);
            cells[1][0].GetValue<int>().Should().Be(2);
            cells[1][1].GetValue<string>().Should().Be("Jane");
            cells[1][2].GetValue<string>().Should().Be("Doe");
            cells[1][3].GetValue<int>().Should().Be(30);
        }

        [Fact]
        public void Build_TwoHeaderGroupsInOneRow_CorrectCells()
        {
            VerticalReportBuilder<(string FirstName, string LastName, int Age)> reportBuilder =
                new VerticalReportBuilder<(string FirstName, string LastName, int Age)>();
            reportBuilder.AddColumn("First Name", x => x.FirstName);
            reportBuilder.AddColumn("Last Name", x => x.LastName);
            reportBuilder.AddColumn("Age", x => x.Age);
            reportBuilder.AddComplexHeader(0, "Personal Info", "First Name", "Last Name");
            reportBuilder.AddComplexHeader(0, "Extra", "Age");

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                ("John", "Doe", 30),
                ("Jane", "Doe", 30),
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(2);
            headerCells[0][0].GetValue<string>().Should().Be("Personal Info");
            headerCells[0][0].ColumnSpan.Should().Be(2);
            headerCells[0][0].RowSpan.Should().Be(1);
            headerCells[0][1].Should().BeNull();
            headerCells[0][2].GetValue<string>().Should().Be("Extra");

            headerCells[1][0].GetValue<string>().Should().Be("First Name");
            headerCells[1][0].RowSpan.Should().Be(1);
            headerCells[1][0].ColumnSpan.Should().Be(1);
            headerCells[1][1].GetValue<string>().Should().Be("Last Name");
            headerCells[1][1].ColumnSpan.Should().Be(1);
            headerCells[1][1].RowSpan.Should().Be(1);
            headerCells[1][2].GetValue<string>().Should().Be("Age");
            headerCells[1][2].ColumnSpan.Should().Be(1);
            headerCells[1][2].RowSpan.Should().Be(1);

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].GetValue<string>().Should().Be("John");
            cells[0][1].GetValue<string>().Should().Be("Doe");
            cells[0][2].GetValue<int>().Should().Be(30);
            cells[1][0].GetValue<string>().Should().Be("Jane");
            cells[1][1].GetValue<string>().Should().Be("Doe");
            cells[1][2].GetValue<int>().Should().Be(30);
        }
    }
}
