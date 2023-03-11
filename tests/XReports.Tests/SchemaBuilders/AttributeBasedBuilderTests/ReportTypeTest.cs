using System.Linq;
using FluentAssertions;
using XReports.Attributes;
using XReports.Interfaces;
using XReports.Schema;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public class ReportTypeTest
    {
        [Fact]
        public void BuildSchemaShouldBuildVerticalReportWhenThereIsNoReportAttribute()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            IReportSchema<WithNoReportAttribute> reportSchema = builder.BuildSchema<WithNoReportAttribute>();

            reportSchema.Should().BeAssignableTo<IVerticalReportSchema<WithNoReportAttribute>>();
        }

        [Fact]
        public void BuildSchemaShouldBuildVerticalReportWhenThereIsVerticalReportAttribute()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            IReportSchema<WithVerticalReportAttribute> reportSchema = builder.BuildSchema<WithVerticalReportAttribute>();

            reportSchema.Should().BeAssignableTo<IVerticalReportSchema<WithVerticalReportAttribute>>();
        }

        [Fact]
        public void BuildSchemaShouldBuildHorizontalReportWhenThereIsHorizontalReportAttribute()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            IReportSchema<WithHorizontalReportAttribute> reportSchema = builder.BuildSchema<WithHorizontalReportAttribute>();

            reportSchema.Should().BeAssignableTo<IHorizontalReportSchema<WithHorizontalReportAttribute>>();
        }

        private class WithNoReportAttribute
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }
        }

        [VerticalReport]
        private class WithVerticalReportAttribute
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }
        }

        [HorizontalReport]
        private class WithHorizontalReportAttribute
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }
        }
    }
}
