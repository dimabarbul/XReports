using System;
using System.Collections.Immutable;
using System.Linq;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Assertions;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void BuildShouldApplyCustomHeaderProperty()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new();
            reportBuilder.AddColumn("Value", s => s)
                .AddHeaderProperties(new CustomHeaderProperty());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<string>());

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[]
                {
                    new ReportCellData("Value")
                    {
                        Properties = ImmutableArray.Create<ReportCellProperty>(new CustomHeaderProperty()),
                    },
                },
            });
        }

        [Fact]
        public void BuildShouldApplyCustomPropertyForComplexHeaderUsingHeaderName()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new();
            reportBuilder.AddColumn("Value", s => s);
            reportBuilder.AddComplexHeader(0, "Complex", "Value");
            reportBuilder.AddComplexHeaderProperties("Complex", new CustomHeaderProperty());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Array.Empty<string>());

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Complex")
                    {
                        Properties = ImmutableArray.Create<ReportCellProperty>(new CustomHeaderProperty()),
                    },
                },
                new object[] { "Value" },
            });
        }

        private class CustomHeaderProperty : ReportCellProperty, IEquatable<CustomHeaderProperty>
        {
            public bool Equals(CustomHeaderProperty other)
            {
                return true;
            }

            public override bool Equals(object obj)
            {
                if (obj is null)
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != this.GetType())
                {
                    return false;
                }

                return this.Equals((CustomHeaderProperty)obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}
