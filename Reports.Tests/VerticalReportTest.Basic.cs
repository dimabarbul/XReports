using FluentAssertions;
using Reports.Builders;
using Reports.Interfaces;
using Reports.ValueProviders;
using Reports.Extensions;
using Reports.Models;
using Xunit;

namespace Reports.Tests
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void Build_NoRows_HasHeader()
        {
            VerticalReportBuilder<(string FirstName, string LastName)> reportBuilder = new VerticalReportBuilder<(string FirstName, string LastName)>();
            reportBuilder.AddColumn("First name", x => x.FirstName);
            reportBuilder.AddColumn("Last name", x => x.LastName);

            IReportTable<ReportCell> table = reportBuilder.Build(new (string, string)[] { });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].GetValue<string>().Should().Be("First name");
            headerCells[0][1].GetValue<string>().Should().Be("Last name");

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().BeEmpty();
        }

        [Fact]
        public void Build_TwoRows_CorrectCells()
        {
            VerticalReportBuilder<(string FirstName, string LastName)> reportBuilder = new VerticalReportBuilder<(string FirstName, string LastName)>();
            reportBuilder.AddColumn("First name", x => x.FirstName);
            reportBuilder.AddColumn("Last name", x => x.LastName);

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                ("John", "Doe"),
                ("Jane", "Do"),
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].GetValue<string>().Should().Be("First name");
            headerCells[0][1].GetValue<string>().Should().Be("Last name");

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].GetValue<string>().Should().Be("John");
            cells[0][1].GetValue<string>().Should().Be("Doe");
            cells[1][0].GetValue<string>().Should().Be("Jane");
            cells[1][1].GetValue<string>().Should().Be("Do");
        }

        [Fact]
        public void Build_SequentialNumberValueProvider_CorrectValues()
        {
            VerticalReportBuilder<string> reportBuilder = new VerticalReportBuilder<string>();
            reportBuilder.AddColumn("#", new SequentialNumberValueProvider());
            reportBuilder.AddColumn("Name", s => s);

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                "John Doe",
                "Jane Doe",
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].GetValue<string>().Should().Be("#");
            headerCells[0][1].GetValue<string>().Should().Be("Name");

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].GetValue<int>().Should().Be(1);
            cells[0][1].GetValue<string>().Should().Be("John Doe");
            cells[1][0].GetValue<int>().Should().Be(2);
            cells[1][1].GetValue<string>().Should().Be("Jane Doe");
        }

        [Fact]
        public void Build_FromArray_CorrectValues()
        {
            VerticalReportBuilder<string[]> reportBuilder = new VerticalReportBuilder<string[]>();
            reportBuilder.AddColumn("Item1", a => a[0]);
            reportBuilder.AddColumn("Item2", a => a[1]);

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                new[] { "John Doe", "Manager" },
                new[] { "Jane Doe", "Developer" },
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].GetValue<string>().Should().Be("Item1");
            headerCells[0][1].GetValue<string>().Should().Be("Item2");

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].GetValue<string>().Should().Be("John Doe");
            cells[0][1].GetValue<string>().Should().Be("Manager");
            cells[1][0].GetValue<string>().Should().Be("Jane Doe");
            cells[1][1].GetValue<string>().Should().Be("Developer");
        }
    }
}
