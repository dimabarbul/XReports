using System;
using System.Data;
using FluentAssertions;
using XReports.Core.Tests.Extensions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.Models
{
    public class VerticalReportSchemaTest
    {
        [Fact]
        public void ReportShouldHaveHeaderWhenThereAreNoRows()
        {
            ReportSchemaBuilder<(string FirstName, string LastName)> reportBuilder =
                new ReportSchemaBuilder<(string FirstName, string LastName)>();
            reportBuilder.AddColumn("First name", x => x.FirstName);
            reportBuilder.AddColumn("Last name", x => x.LastName);

            IReportTable<ReportCell> table = reportBuilder.BuildVerticalSchema().BuildReportTable(Array.Empty<(string, string)>());

            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("First name"),
                    ReportCellHelper.CreateReportCell("Last name"),
                },
            });
            table.Rows.Should().BeEmpty();
        }

        [Fact]
        public void EnumeratingReportMultipleTimesShouldWork()
        {
            ReportSchemaBuilder<string> reportBuilder = new ReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", s => s);

            IReportTable<ReportCell> table = reportBuilder.BuildVerticalSchema().BuildReportTable(new[]
            {
                "test",
            });
            // enumerating for the first time
            table.Enumerate();

            // enumerating for the second time
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Value"),
                },
            });
            table.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("test"),
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldSupportDataReaderAsDataSource()
        {
            ReportSchemaBuilder<IDataReader> builder = new ReportSchemaBuilder<IDataReader>();

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
                    IReportTable<ReportCell> reportTable = builder.BuildVerticalSchema().BuildReportTable(dataReader);

                    reportTable.Rows.Should().Equal(new[]
                    {
                        new[]
                        {
                            ReportCellHelper.CreateReportCell("John"),
                            ReportCellHelper.CreateReportCell(23),
                        },
                        new[]
                        {
                            ReportCellHelper.CreateReportCell("Jane"),
                            ReportCellHelper.CreateReportCell(22),
                        },
                    });
                }
            }
        }

        [Fact]
        public void BuildReportTableShouldThrowWhenDataReaderIsDataSourceButSourceTypeIsDifferent()
        {
            ReportSchemaBuilder<string> builder = new ReportSchemaBuilder<string>();

            builder.AddColumn("Value", s => s);

            using (DataTable dataTable = new DataTable())
            {
                dataTable.Columns.AddRange(new[] { new DataColumn("Value", typeof(string)) });
                dataTable.Rows.Add("John");
                dataTable.Rows.Add("Jane");

                using (IDataReader dataReader = new DataTableReader(dataTable))
                {
                    Action action = () => _ = builder.BuildVerticalSchema().BuildReportTable(dataReader);

                    action.Should().ThrowExactly<ArgumentException>();
                }
            }
        }

        [Fact]
        public void BuildReportTableShouldThrowWhenDataReaderIsSourceTypeButDataSourceIsDifferent()
        {
            ReportSchemaBuilder<IDataReader> builder = new ReportSchemaBuilder<IDataReader>();

            builder.AddColumn("Value", x => x.GetString(0));

            using (DataTable dataTable = new DataTable())
            {
                dataTable.Columns.AddRange(new[] { new DataColumn("Value", typeof(string)), });
                dataTable.Rows.Add("John");
                dataTable.Rows.Add("Jane");

                using (IDataReader dataReader = new DataTableReader(dataTable))
                {
                    Action action = () => _ = builder.BuildVerticalSchema().BuildReportTable(new[] { dataReader });

                    action.Should().ThrowExactly<ArgumentException>();
                }
            }
        }

        [Fact]
        public void SchemaShouldBeAvailableForBuildingMultipleReportsWithDifferentData()
        {
            ReportSchemaBuilder<string> reportBuilder =
                new ReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", x => x);
            reportBuilder.AddColumn("Length", x => x.Length);

            VerticalReportSchema<string> schema =
                reportBuilder.BuildVerticalSchema();
            IReportTable<ReportCell> table1 = schema.BuildReportTable(new[]
            {
                "Test",
            });
            IReportTable<ReportCell> table2 = schema.BuildReportTable(new[]
            {
                "String",
            });

            table1.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Value"),
                    ReportCellHelper.CreateReportCell("Length"),
                },
            });
            table1.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Test"),
                    ReportCellHelper.CreateReportCell(4),
                },
            });
            table2.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Value"),
                    ReportCellHelper.CreateReportCell("Length"),
                },
            });
            table2.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("String"),
                    ReportCellHelper.CreateReportCell(6),
                },
            });
        }

        [Fact]
        public void BuildReportTableShouldThrowWhenDataReaderIsClosed()
        {
            ReportSchemaBuilder<IDataReader> builder = new ReportSchemaBuilder<IDataReader>();

            builder.AddColumn("Value", x => x.GetString(0));

            using (DataTable dataTable = new DataTable())
            {
                dataTable.Columns.AddRange(new[] { new DataColumn("Value", typeof(string)), });
                dataTable.Rows.Add("John");
                dataTable.Rows.Add("Jane");

                using (IDataReader dataReader = new DataTableReader(dataTable))
                {
                    dataReader.Close();

                    Action action = () => _ = builder.BuildVerticalSchema().BuildReportTable(dataReader);

                    action.Should().ThrowExactly<InvalidOperationException>();
                }
            }
        }
    }
}
