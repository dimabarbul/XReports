using System.Data;
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
        public void Build_FromDataReader_Correct()
        {
            VerticalReportSchemaBuilder<IDataReader> builder = new VerticalReportSchemaBuilder<IDataReader>();

            builder.AddColumn("Name", x => x.GetString(0));
            builder.AddColumn("Age", x => x.GetInt32(1));

            using DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new[]
            {
                new DataColumn("Name", typeof(string)),
                new DataColumn("Age", typeof(int)),
            });
            dataTable.Rows.Add("John", 23);
            dataTable.Rows.Add("Jane", 22);
            using IDataReader dataReader = new DataTableReader(dataTable);
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(dataReader);

            ReportCell[][] cells = this.GetCellsAsArray(reportTable.Rows);

            cells[0][0].GetValue<string>().Should().Be("John");
            cells[0][1].GetValue<int>().Should().Be(23);
            cells[1][0].GetValue<string>().Should().Be("Jane");
            cells[1][1].GetValue<int>().Should().Be(22);
        }
    }
}
