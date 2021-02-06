using System;
using FluentAssertions;
using XReports.AttributeHandlers;
using XReports.Attributes;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Extensions.Builders.Tests.BuilderHelpers
{
    public partial class EntityAttributeBuilderHelperTest
    {
        [Fact]
        public void BuildVerticalReport_CustomAttributeHandler_Applied()
        {
            AttributeBasedBuilder helper = new AttributeBasedBuilder(
                Mocks.ServiceProvider,
                new[] { new CustomAttributeHandler() });

            IReportSchema<EntityWithCustomAttribute> schema = helper.BuildSchema<EntityWithCustomAttribute>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new EntityWithCustomAttribute() { Title = "Test" },
            });

            ReportCell[][] cells = this.GetCellsAsArray(reportTable.Rows);

            cells[0][0].Properties.Should().HaveCount(1)
                .And.ContainItemsAssignableTo<CustomProperty>();
        }

        private class EntityWithCustomAttribute
        {
            [ReportVariable(1, "Title")]
            [Custom]
            public string Title { get; set; }
        }

        private class CustomAttribute : Attribute
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
