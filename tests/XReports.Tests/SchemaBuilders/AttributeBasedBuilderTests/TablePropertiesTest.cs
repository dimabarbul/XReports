using System;
using System.Collections.Generic;
using FluentAssertions;
using XReports.AttributeHandlers;
using XReports.Attributes;
using XReports.Interfaces;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public class TablePropertiesTest
    {
        [Fact]
        public void BuildSchemaShouldCallCustomAttributeHandlerWhenThereIsCustomAttribute()
        {
            TrackableAttributeHandler trackableAttributeHandler1 = new TrackableAttributeHandler();
            TrackableAttributeHandler trackableAttributeHandler2 = new TrackableAttributeHandler();
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                trackableAttributeHandler1,
                trackableAttributeHandler2,
            });

            IReportSchema<WithCustomTableAttribute> _ = builder.BuildSchema<WithCustomTableAttribute>();

            trackableAttributeHandler1.CallsCount.Should().Be(1);
            trackableAttributeHandler2.CallsCount.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldNotCallCustomAttributeHandlerWhenThereIsNoCustomAttribute()
        {
            TrackableAttributeHandler trackableAttributeHandler = new TrackableAttributeHandler();
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                trackableAttributeHandler,
            });

            IReportSchema<WithoutCustomTableAttribute> _ = builder.BuildSchema<WithoutCustomTableAttribute>();

            trackableAttributeHandler.CallsCount.Should().Be(0);
        }

        [Fact]
        public void BuildSchemaShouldCallCustomAttributeHandlerForTableAttribute()
        {
            CustomHandler customHandler = new CustomHandler();
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
            {
                customHandler,
            });

            IReportSchema<WithCustomTableAttribute> _ = builder.BuildSchema<WithCustomTableAttribute>();

            customHandler.Types.Should().BeEquivalentTo(
                typeof(CustomTableAttribute));
        }

        [CustomTable]
        private class WithCustomTableAttribute
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Title")]
            public string Title { get; set; }
        }

        private class WithoutCustomTableAttribute
        {
            [ReportVariable(1, "Title")]
            public string Title { get; set; }
        }

        [AttributeUsage(AttributeTargets.Class)]
        private sealed class CustomTableAttribute : TableAttribute
        {
        }

        private class TrackableAttributeHandler : AttributeHandler<CustomTableAttribute>
        {
            public int CallsCount { get; private set; }

            protected override void HandleAttribute<TSourceEntity>(
                IReportSchemaBuilder<TSourceEntity> builder,
                IReportSchemaCellsProviderBuilder<TSourceEntity> cellsProviderBuilder,
                CustomTableAttribute attribute)
            {
                this.CallsCount++;
            }
        }

        private class CustomHandler : IAttributeHandler
        {
            public List<Type> Types { get; } = new List<Type>();

            public void Handle<TSourceEntity>(IReportSchemaBuilder<TSourceEntity> schemaBuilder, IReportSchemaCellsProviderBuilder<TSourceEntity> cellsProviderBuilder,
                Attribute attribute)
            {
                this.Types.Add(attribute.GetType());
            }
        }
    }
}