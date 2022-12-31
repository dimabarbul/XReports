using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluentAssertions;
using XReports.Core.Tests.Extensions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.Models
{
    public class VerticalReportSchemaTest
    {
        [Fact]
        public void ReportShouldHaveHeaderWhenThereAreNoRows()
        {
            VerticalReportSchemaBuilder<(string FirstName, string LastName)> reportBuilder =
                new VerticalReportSchemaBuilder<(string FirstName, string LastName)>();
            reportBuilder.AddColumn("First name", x => x.FirstName);
            reportBuilder.AddColumn("Last name", x => x.LastName);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Array.Empty<(string, string)>());

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new [] { "First name", "Last name" },
            });
            table.Rows.Should().BeEquivalentTo(Enumerable.Empty<IEnumerable<object>>());
        }

        [Fact]
        public void EnumeratingReportMultipleTimesShouldWork()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new VerticalReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", s => s);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "test",
            });
            // enumerating for the first time
            table.Enumerate();

            // enumerating for the second time
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "Value" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { "test" },
            });
        }

        [Fact]
        public void BuildReportTableShouldSupportDataReaderAsDataSource()
        {
            VerticalReportSchemaBuilder<IDataReader> builder = new VerticalReportSchemaBuilder<IDataReader>();

            builder.AddColumn("Name", x => x.GetString(0));
            builder.AddColumn("Age", x => x.GetInt32(1));

            using (DataTable dataTable = new DataTable())
            {
                dataTable.Columns.AddRange(new[]
                {
                    new DataColumn("Name", typeof(string)), new DataColumn("Age", typeof(int)),
                });
                dataTable.Rows.Add("John", 23);
                dataTable.Rows.Add("Jane", 22);

                using (IDataReader dataReader = new DataTableReader(dataTable))
                {
                    IReportTable<ReportCell> reportTable = builder.BuildSchema().BuildReportTable(dataReader);

                    reportTable.Rows.Should().BeEquivalentTo(new[]
                    {
                        new object[] { "John", 23 },
                        new object[] { "Jane", 22 },
                    });
                }
            }
        }

        [Fact]
        public void BuildReportTableShouldThrowWhenDataReaderIsDataSourceButSourceTypeIsDifferent()
        {
            VerticalReportSchemaBuilder<string> builder = new VerticalReportSchemaBuilder<string>();

            builder.AddColumn("Value", s => s);

            using (DataTable dataTable = new DataTable())
            {
                dataTable.Columns.AddRange(new[] { new DataColumn("Value", typeof(string)) });
                dataTable.Rows.Add("John");
                dataTable.Rows.Add("Jane");

                using (IDataReader dataReader = new DataTableReader(dataTable))
                {
                    Action action = () => _ = builder.BuildSchema().BuildReportTable(dataReader);

                    action.Should().ThrowExactly<ArgumentException>();
                }
            }
        }

        [Fact]
        public void BuildReportTableShouldThrowWhenDataReaderIsSourceTypeButDataSourceIsDifferent()
        {
            VerticalReportSchemaBuilder<IDataReader> builder = new VerticalReportSchemaBuilder<IDataReader>();

            builder.AddColumn("Value", x => x.GetString(0));

            using (DataTable dataTable = new DataTable())
            {
                dataTable.Columns.AddRange(new[] { new DataColumn("Value", typeof(string)), });
                dataTable.Rows.Add("John");
                dataTable.Rows.Add("Jane");

                using (IDataReader dataReader = new DataTableReader(dataTable))
                {
                    Action action = () => _ = builder.BuildSchema().BuildReportTable(new[] { dataReader });

                    action.Should().ThrowExactly<ArgumentException>();
                }
            }
        }

        [Fact]
        public void SchemaShouldBeAvailableForBuildingMultipleReportsWithDifferentData()
        {
            VerticalReportSchemaBuilder<string> reportBuilder =
                new VerticalReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", x => x);
            reportBuilder.AddColumn("Length", x => x.Length);

            VerticalReportSchema<string> schema =
                reportBuilder.BuildSchema();
            IReportTable<ReportCell> table1 = schema.BuildReportTable(new[]
            {
                "Test",
            });
            IReportTable<ReportCell> table2 = schema.BuildReportTable(new[]
            {
                "String",
            });

            table1.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new [] { "Value", "Length" },
            });
            table1.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]{ "Test", 4 },
            });
            table2.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new [] { "Value", "Length" },
            });
            table2.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]{ "String", 6 },
            });
        }
    }
}
