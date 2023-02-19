using System;
using System.Collections.Generic;
using XReports.AttributeHandlers;
using XReports.Attributes;
using XReports.Interfaces;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class PostBuilderFromServiceProviderTest
    {
        [AttributeUsage(AttributeTargets.Class)]
        private sealed class MyTableAttribute : TableAttribute
        {
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
        private sealed class MyAttribute : Attribute
        {
        }

        private class MyTableAttributeHandler : AttributeHandler<MyTableAttribute>
        {
            private readonly List<Attribute> attributes;

            public MyTableAttributeHandler(List<Attribute> attributes)
            {
                this.attributes = attributes;
            }

            protected override void HandleAttribute<TSourceEntity>(
                IReportSchemaBuilder<TSourceEntity> builder,
                IReportSchemaCellsProviderBuilder<TSourceEntity> cellsProviderBuilder,
                MyTableAttribute attribute)
            {
                this.attributes.Add(attribute);
            }
        }

        private class MyAttributeHandler : AttributeHandler<MyAttribute>
        {
            private readonly List<Attribute> attributes;

            public MyAttributeHandler(List<Attribute> attributes)
            {
                this.attributes = attributes;
            }

            protected override void HandleAttribute<TSourceEntity>(
                IReportSchemaBuilder<TSourceEntity> builder,
                IReportSchemaCellsProviderBuilder<TSourceEntity> cellsProviderBuilder,
                MyAttribute attribute)
            {
                this.attributes.Add(attribute);
            }
        }
    }
}