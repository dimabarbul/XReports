using System;
using System.Collections.Generic;
using FluentAssertions;
using XReports.ReportCellProperties;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.AttributeHandlers;
using XReports.SchemaBuilders.Attributes;
using Xunit;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public class AttributeHandlersTest
    {
        [Fact]
        public void BuildSchemaShouldCallSingleTypeCustomAttributeHandlerWhenThereIsCustomAttribute()
        {
            SingleTypeCustomAttributeHandler customAttributeHandler1 = new SingleTypeCustomAttributeHandler();
            SingleTypeCustomAttributeHandler customAttributeHandler2 = new SingleTypeCustomAttributeHandler();
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                customAttributeHandler1,
                customAttributeHandler2,
            });

            IReportSchema<WithCustomAttribute> _ = builder.BuildSchema<WithCustomAttribute>();

            customAttributeHandler1.CallsCount.Should().Be(1);
            customAttributeHandler2.CallsCount.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldNotCallSingleTypeCustomAttributeHandlerWhenThereIsNoCustomAttribute()
        {
            SingleTypeCustomAttributeHandler customAttributeHandler = new SingleTypeCustomAttributeHandler();
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                customAttributeHandler,
            });

            IReportSchema<WithoutCustomAttribute> _ = builder.BuildSchema<WithoutCustomAttribute>();

            customAttributeHandler.CallsCount.Should().Be(0);
        }

        [Fact]
        public void BuildSchemaShouldCallCustomAttributeHandlerForEachProperty()
        {
            CustomHandler customHandler = new CustomHandler();
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                customHandler,
            });

            IReportSchema<WithMultipleAttributes> _ = builder.BuildSchema<WithMultipleAttributes>();

            customHandler.Types.Should().BeEquivalentTo(
                typeof(CustomAttribute),
                typeof(BoldAttribute),
                typeof(CustomAttribute),
                typeof(AlignmentAttribute));
        }

        private class WithCustomAttribute
        {
            [ReportColumn(1, "Title")]
            [Custom]
            public string Title { get; set; }
        }

        private class WithoutCustomAttribute
        {
            [ReportColumn(1, "Title")]
            public string Title { get; set; }
        }

        [Custom]
        private class WithMultipleAttributes
        {
            [ReportColumn(1, "ID")]
            [Bold]
            public int Id { get; set; }

            [ReportColumn(2, "Title")]
            [Alignment(Alignment.Left)]
            public string Title { get; set; }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
        private sealed class CustomAttribute : Attribute
        {
        }

        private class SingleTypeCustomAttributeHandler : AttributeHandler<CustomAttribute>
        {
            public int CallsCount { get; private set; }

            protected override void HandleAttribute<TSourceEntity>(
                IReportSchemaBuilder<TSourceEntity> schemaBuilder,
                IReportColumnBuilder<TSourceEntity> columnBuilder,
                CustomAttribute attribute)
            {
                this.CallsCount++;
            }
        }

        private class CustomHandler : IAttributeHandler
        {
            public List<Type> Types { get; } = new List<Type>();

            public void Handle<TSourceEntity>(IReportSchemaBuilder<TSourceEntity> schemaBuilder, IReportColumnBuilder<TSourceEntity> columnBuilder,
                Attribute attribute)
            {
                this.Types.Add(attribute.GetType());
            }
        }
    }
}
