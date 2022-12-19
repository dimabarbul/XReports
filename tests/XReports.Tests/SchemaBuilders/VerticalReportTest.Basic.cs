using System;
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
        public void BuildShouldHaveHeaderWhenNoRows()
        {
            VerticalReportSchemaBuilder<(string FirstName, string LastName)> reportBuilder = new VerticalReportSchemaBuilder<(string FirstName, string LastName)>();
            reportBuilder.AddColumn("First name", x => x.FirstName);
            reportBuilder.AddColumn("Last name", x => x.LastName);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Array.Empty<(string, string)>());

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].GetValue<string>().Should().Be("First name");
            headerCells[0][1].GetValue<string>().Should().Be("Last name");

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().BeEmpty();
        }

        [Fact]
        public void BuildShouldSupportMultipleRows()
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
    }
}
