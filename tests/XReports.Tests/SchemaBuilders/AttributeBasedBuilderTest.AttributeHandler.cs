using System;
using XReports.AttributeHandlers;
using XReports.Attributes;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Assertions;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class AttributeBasedBuilderTest
    {
        [Fact]
        public void BuildVerticalReportShouldApplyCustomAttributeHandler()
        {
            AttributeBasedBuilder helper = new(
                this.serviceProvider,
                new[] { new CustomAttributeHandler() });

            IReportSchema<EntityWithCustomAttribute> schema = helper.BuildSchema<EntityWithCustomAttribute>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new EntityWithCustomAttribute() { Title = "Test" },
            });

            reportTable.Rows.Should().BeEquivalentTo(new[]
            {
                new[] {
                    new ReportCellData("Test")
                    {
                        Properties = new[] { new CustomProperty() },
                    },
                },
            });
        }

        private class EntityWithCustomAttribute
        {
            [ReportVariable(1, "Title")]
            [Custom]
            public string Title { get; set; }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
        private sealed class CustomAttribute : Attribute
        {
        }

        private class CustomProperty : ReportCellProperty
        {
        }

        private class CustomAttributeHandler : AttributeHandler<CustomAttribute>
        {
            protected override void HandleAttribute<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, CustomAttribute attribute)
            {
                builder.AddProperties(new CustomProperty());
            }
        }
    }
}
