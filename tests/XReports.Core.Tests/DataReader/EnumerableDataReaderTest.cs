using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluentAssertions;
using XReports.DataReader;
using Xunit;

namespace XReports.Core.Tests.DataReader
{
    public class EnumerableDataReaderTest
    {
        [Fact]
        public void EnumerableShouldReturnCorrectValues()
        {
            using (DataTable dataTable = new DataTable())
            {
                dataTable.Columns.AddRange(new[]
                {
                    new DataColumn("Name", typeof(string)),
                    new DataColumn("Age", typeof(int)),
                });
                dataTable.Rows.Add("John", 23);
                dataTable.Rows.Add("Jane", 22);

                using (IDataReader dataReader = new DataTableReader(dataTable))
                {
                    IEnumerable<IDataReader> enumerable = dataReader.AsEnumerable();

                    enumerable
                        .Select(dr => new
                        {
                            Name = dr.GetString(0),
                            Age = dr.GetInt32(1),
                        })
                        .Should()
                        .BeEquivalentTo(
                            new
                            {
                                Name = "John",
                                Age = 23,
                            },
                            new
                            {
                                Name = "Jane",
                                Age = 22,
                            });
                }
            }
        }

        [Fact]
        public void EnumerableShouldReturnEmptyResultWhenIteratedSecondTime()
        {
            using (DataTable dataTable = new DataTable())
            {
                dataTable.Columns.AddRange(new[]
                {
                    new DataColumn("Name", typeof(string)),
                });
                dataTable.Rows.Add("John");
                dataTable.Rows.Add("Jane");

                using (IDataReader dataReader = new DataTableReader(dataTable))
                {
                    IEnumerable<IDataReader> enumerable = dataReader.AsEnumerable();

                    _ = enumerable.ToArray();

                    enumerable.Should().BeEmpty();
                }
            }
        }

        [Fact]
        public void MoveNextShouldThrowWhenDataReaderIsClosed()
        {
            using (DataTable dataTable = new DataTable())
            {
                dataTable.Columns.AddRange(new[] { new DataColumn("Value", typeof(string)), });
                dataTable.Rows.Add("John");
                dataTable.Rows.Add("Jane");

                using (IDataReader dataReader = new DataTableReader(dataTable))
                {
                    dataReader.Close();
                    IEnumerable<IDataReader> enumerable = dataReader.AsEnumerable();
                    using (IEnumerator<IDataReader> enumerator = enumerable.GetEnumerator())
                    {
                        Action action = () => _ = enumerator.MoveNext();

                        action.Should().ThrowExactly<InvalidOperationException>();
                    }
                }
            }
        }
    }
}
