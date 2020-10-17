using FluentAssertions;
using Reports.Builders;
using Reports.Models;
using Reports.ValueFormatters;
using Reports.ValueProviders;
using Xunit;

namespace Reports.Tests
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void Build_OneColumnTwoHeaderRows_CorrectCells()
        {
            VerticalReportBuilder<int> reportBuilder = new VerticalReportBuilder<int>();
            reportBuilder.AddColumn("Value", i => i);
            reportBuilder.AddComplexHeader(0, "Statistics", "Value");

            ReportTable table = reportBuilder.Build(new[] { 1 });

            table.Cells.Should().HaveCount(3);
            table.Cells[0][0].DisplayValue.Should().Be("Statistics");
            table.Cells[1][0].DisplayValue.Should().Be("Value");
            table.Cells[2][0].DisplayValue.Should().Be("1");
        }

        [Fact]
        public void Build_TwoColumnsTwoHeaderRowsByColumnName_CorrectCells()
        {
            VerticalReportBuilder<(string Name, int Age)> reportBuilder = new VerticalReportBuilder<(string Name, int Age)>();
            reportBuilder.AddColumn("Name", x => x.Name);
            reportBuilder.AddColumn("Age", x => x.Age);
            reportBuilder.AddComplexHeader(0, "Personal Info", "Name", "Age");

            ReportTable table = reportBuilder.Build(new[]
            {
                ("John Doe", 30),
                ("Jane Doe", 27),
            });

            table.Cells.Should().HaveCount(4);
            table.Cells[0][0].DisplayValue.Should().Be("Personal Info");
            table.Cells[0][0].ColumnSpan.Should().Be(2);
            table.Cells[0][1].Should().BeNull();
            table.Cells[1][0].DisplayValue.Should().Be("Name");
            table.Cells[1][0].ColumnSpan.Should().Be(1);
            table.Cells[1][1].DisplayValue.Should().Be("Age");
            table.Cells[2][0].DisplayValue.Should().Be("John Doe");
            table.Cells[2][0].ColumnSpan.Should().Be(1);
            table.Cells[2][1].DisplayValue.Should().Be("30");
            table.Cells[3][0].DisplayValue.Should().Be("Jane Doe");
            table.Cells[3][1].DisplayValue.Should().Be("27");
        }

        [Fact]
        public void Build_ThreeColumnsTwoHeaderRowsByColumnIndexes_CorrectCells()
        {
            VerticalReportBuilder<(string Name, int Age, string Gender)> reportBuilder = new VerticalReportBuilder<(string Name, int Age, string Gender)>();
            reportBuilder.AddColumn("Name", x => x.Name);
            reportBuilder.AddColumn("Age", x => x.Age);
            reportBuilder.AddColumn("Gender", x => x.Gender);
            reportBuilder.AddComplexHeader(0, "Personal Info", 0, 2);

            ReportTable table = reportBuilder.Build(new[]
            {
                ("John Doe", 30, "Male"),
                ("Jane Doe", 27, "Female"),
            });

            table.Cells.Should().HaveCount(4);
            table.Cells[0][0].DisplayValue.Should().Be("Personal Info");
            table.Cells[0][0].ColumnSpan.Should().Be(3);
            table.Cells[0][1].Should().BeNull();
            table.Cells[0][2].Should().BeNull();
            table.Cells[1][0].DisplayValue.Should().Be("Name");
            table.Cells[1][1].DisplayValue.Should().Be("Age");
            table.Cells[1][2].DisplayValue.Should().Be("Gender");
            table.Cells[2][0].DisplayValue.Should().Be("John Doe");
            table.Cells[2][1].DisplayValue.Should().Be("30");
            table.Cells[2][2].DisplayValue.Should().Be("Male");
            table.Cells[3][0].DisplayValue.Should().Be("Jane Doe");
            table.Cells[3][1].DisplayValue.Should().Be("27");
            table.Cells[3][2].DisplayValue.Should().Be("Female");
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
            reportBuilder.SetColumnValueFormatter("Salary", new DecimalValueFormatter(0));
            reportBuilder.AddComplexHeader(0, "Personal Info", 0, 1);
            reportBuilder.AddComplexHeader(0, "Job Info", 2, 3);

            ReportTable table = reportBuilder.Build(new[]
            {
                ("John Doe", 30, "Developer", 1000m),
            });

            table.Cells.Should().HaveCount(3);
            table.Cells[0][0].DisplayValue.Should().Be("Personal Info");
            table.Cells[0][0].ColumnSpan.Should().Be(2);
            table.Cells[0][1].Should().BeNull();
            table.Cells[0][2].DisplayValue.Should().Be("Job Info");
            table.Cells[0][2].ColumnSpan.Should().Be(2);
            table.Cells[0][3].Should().BeNull();
            table.Cells[1][0].DisplayValue.Should().Be("Name");
            table.Cells[1][1].DisplayValue.Should().Be("Age");
            table.Cells[1][2].DisplayValue.Should().Be("Job");
            table.Cells[1][3].DisplayValue.Should().Be("Salary");
            table.Cells[2][0].DisplayValue.Should().Be("John Doe");
            table.Cells[2][1].DisplayValue.Should().Be("30");
            table.Cells[2][2].DisplayValue.Should().Be("Developer");
            table.Cells[2][3].DisplayValue.Should().Be("1000");
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

            ReportTable table = reportBuilder.Build(new[]
            {
                ("John Doe", 30),
                ("Jane Doe", 30),
            });

            table.Cells.Should().HaveCount(4);
            table.Cells[0][0].DisplayValue.Should().Be("#");
            table.Cells[0][0].RowSpan.Should().Be(2);
            table.Cells[0][0].ColumnSpan.Should().Be(1);
            table.Cells[0][1].DisplayValue.Should().Be("Employee");
            table.Cells[0][1].ColumnSpan.Should().Be(2);
            table.Cells[0][1].RowSpan.Should().Be(1);
            table.Cells[0][2].Should().BeNull();
            table.Cells[1][0].Should().BeNull();
            table.Cells[1][1].DisplayValue.Should().Be("Name");
            table.Cells[1][2].DisplayValue.Should().Be("Age");
            table.Cells[2][0].DisplayValue.Should().Be("1");
            table.Cells[2][1].DisplayValue.Should().Be("John Doe");
            table.Cells[2][2].DisplayValue.Should().Be("30");
            table.Cells[3][0].DisplayValue.Should().Be("2");
            table.Cells[3][1].DisplayValue.Should().Be("Jane Doe");
            table.Cells[3][2].DisplayValue.Should().Be("30");
        }

        [Fact]
        public void Build_ThreeHeaderRows_CorrectCells()
        {
            VerticalReportBuilder<(string FirstName, string LastName, int Age)> reportBuilder =
                new VerticalReportBuilder<(string FirstName, string LastName, int Age)>();
            reportBuilder.AddColumn("#", new SequentialNumberValueProvider());
            reportBuilder.AddColumn("First Name", x => x.FirstName);
            reportBuilder.AddColumn("Last Name", x => x.LastName);
            reportBuilder.AddColumn("Age", x => x.Age);
            reportBuilder.AddComplexHeader(0, "Employee", "First Name", "Age");
            reportBuilder.AddComplexHeader(1, "Personal Info", "First Name", "Last Name");

            ReportTable table = reportBuilder.Build(new[]
            {
                ("John", "Doe", 30),
                ("Jane", "Doe", 30),
            });

            table.Cells.Should().HaveCount(5);
            table.Cells[0][0].DisplayValue.Should().Be("#");
            table.Cells[0][0].RowSpan.Should().Be(3);
            table.Cells[0][0].ColumnSpan.Should().Be(1);
            table.Cells[0][1].DisplayValue.Should().Be("Employee");
            table.Cells[0][1].ColumnSpan.Should().Be(3);
            table.Cells[0][1].RowSpan.Should().Be(1);
            table.Cells[0][2].Should().BeNull();
            table.Cells[0][3].Should().BeNull();

            table.Cells[1][0].Should().BeNull();
            table.Cells[1][1].DisplayValue.Should().Be("Personal Info");
            table.Cells[1][1].RowSpan.Should().Be(1);
            table.Cells[1][1].ColumnSpan.Should().Be(2);
            table.Cells[1][2].Should().BeNull();
            table.Cells[1][3].DisplayValue.Should().Be("Age");
            table.Cells[1][3].ColumnSpan.Should().Be(1);
            table.Cells[1][3].RowSpan.Should().Be(2);

            table.Cells[2][0].Should().BeNull();
            table.Cells[2][1].DisplayValue.Should().Be("First Name");
            table.Cells[2][1].RowSpan.Should().Be(1);
            table.Cells[2][1].ColumnSpan.Should().Be(1);
            table.Cells[2][2].DisplayValue.Should().Be("Last Name");
            table.Cells[2][2].ColumnSpan.Should().Be(1);
            table.Cells[2][2].RowSpan.Should().Be(1);
            table.Cells[2][3].Should().BeNull();

            table.Cells[3][0].DisplayValue.Should().Be("1");
            table.Cells[3][1].DisplayValue.Should().Be("John");
            table.Cells[3][2].DisplayValue.Should().Be("Doe");
            table.Cells[3][3].DisplayValue.Should().Be("30");
            table.Cells[4][0].DisplayValue.Should().Be("2");
            table.Cells[4][1].DisplayValue.Should().Be("Jane");
            table.Cells[4][2].DisplayValue.Should().Be("Doe");
            table.Cells[4][3].DisplayValue.Should().Be("30");
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

            ReportTable table = reportBuilder.Build(new[]
            {
                ("John", "Doe", 30),
                ("Jane", "Doe", 30),
            });

            table.Cells.Should().HaveCount(4);
            table.Cells[0][0].DisplayValue.Should().Be("Personal Info");
            table.Cells[0][0].ColumnSpan.Should().Be(2);
            table.Cells[0][0].RowSpan.Should().Be(1);
            table.Cells[0][1].Should().BeNull();
            table.Cells[0][2].DisplayValue.Should().Be("Extra");

            table.Cells[1][0].DisplayValue.Should().Be("First Name");
            table.Cells[1][0].RowSpan.Should().Be(1);
            table.Cells[1][0].ColumnSpan.Should().Be(1);
            table.Cells[1][1].DisplayValue.Should().Be("Last Name");
            table.Cells[1][1].ColumnSpan.Should().Be(1);
            table.Cells[1][1].RowSpan.Should().Be(1);
            table.Cells[1][2].DisplayValue.Should().Be("Age");
            table.Cells[1][2].ColumnSpan.Should().Be(1);
            table.Cells[1][2].RowSpan.Should().Be(1);

            table.Cells[2][0].DisplayValue.Should().Be("John");
            table.Cells[2][1].DisplayValue.Should().Be("Doe");
            table.Cells[2][2].DisplayValue.Should().Be("30");
            table.Cells[3][0].DisplayValue.Should().Be("Jane");
            table.Cells[3][1].DisplayValue.Should().Be("Doe");
            table.Cells[3][2].DisplayValue.Should().Be("30");
        }
    }
}
