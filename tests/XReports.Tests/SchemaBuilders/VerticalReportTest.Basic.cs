using FluentAssertions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void Build_NoRows_HasHeader()
        {
            VerticalReportSchemaBuilder<(string FirstName, string LastName)> reportBuilder = new VerticalReportSchemaBuilder<(string FirstName, string LastName)>();
            reportBuilder.AddColumn("First name", x => x.FirstName);
            reportBuilder.AddColumn("Last name", x => x.LastName);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new (string, string)[] { });

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
            VerticalReportSchemaBuilder<(string FirstName, string LastName)> reportBuilder = new VerticalReportSchemaBuilder<(string FirstName, string LastName)>();
            reportBuilder.AddColumn("First name", x => x.FirstName);
            reportBuilder.AddColumn("Last name", x => x.LastName);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
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
        public void Build_FromArray_CorrectValues()
        {
            VerticalReportSchemaBuilder<string[]> reportBuilder = new VerticalReportSchemaBuilder<string[]>();
            reportBuilder.AddColumn("Item1", a => a[0]);
            reportBuilder.AddColumn("Item2", a => a[1]);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
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
