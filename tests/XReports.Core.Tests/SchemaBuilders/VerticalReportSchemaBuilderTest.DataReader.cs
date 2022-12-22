using System;
using System.Data;
using FluentAssertions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders
{
    public partial class VerticalReportSchemaBuilderTest
    {
        [Fact]
        public void BuildShouldSupportDataReaderAsDataSource()
        {
            VerticalReportSchemaBuilder<IDataReader> builder = new();

            builder.AddColumn("Name", x => x.GetString(0));
            builder.AddColumn("Age", x => x.GetInt32(1));

            using DataTable dataTable = new();
            dataTable.Columns.AddRange(new[]
            {
                new DataColumn("Name", typeof(string)),
                new DataColumn("Age", typeof(int)),
            });
            dataTable.Rows.Add("John", 23);
            dataTable.Rows.Add("Jane", 22);

            using IDataReader dataReader = new DataTableReader(dataTable);
            IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(dataReader);

            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { "John", 23 },
                new object[] { "Jane", 22 },
            });
        }

        [Fact]
        public void BuildShouldThrowWhenDataReaderIsDataSourceButSourceTypeIsDifferent()
        {
            VerticalReportSchemaBuilder<string> builder = new();

            builder.AddColumn("Value", s => s);

            using DataTable dataTable = new();
            dataTable.Columns.AddRange(new[]
            {
                new DataColumn("Value", typeof(string)),
            });
            dataTable.Rows.Add("John");
            dataTable.Rows.Add("Jane");

            using IDataReader dataReader = new DataTableReader(dataTable);
            Action action = () => _ = builder.BuildSchema().BuildReportTable(dataReader);

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void BuildShouldThrowWhenDataReaderIsSourceTypeButDataSourceIsDifferent()
        {
            VerticalReportSchemaBuilder<IDataReader> builder = new();

            builder.AddColumn("Value", x => x.GetString(0));

            using DataTable dataTable = new();
            dataTable.Columns.AddRange(new[]
            {
                new DataColumn("Value", typeof(string)),
            });
            dataTable.Rows.Add("John");
            dataTable.Rows.Add("Jane");

            using IDataReader dataReader = new DataTableReader(dataTable);
            Action action = () => _ = builder.BuildSchema().BuildReportTable(new[] { dataReader });

            action.Should().ThrowExactly<ArgumentException>();
        }
    }
}
