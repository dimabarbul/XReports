using System;
using XReports.AttributeHandlers;
using XReports.Attributes;
using XReports.Interfaces;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class PostBuilderTest
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
            private event Action<Attribute> OnHandle;

            public MyTableAttributeHandler(Action<Attribute> onHandle)
            {
                this.OnHandle += onHandle;
            }

            protected override void HandleAttribute<TSourceEntity>(
                IReportSchemaBuilder<TSourceEntity> builder,
                IReportSchemaCellsProviderBuilder<TSourceEntity> cellsProviderBuilder,
                MyTableAttribute attribute)
            {
                this.OnHandle?.Invoke(attribute);
            }
        }

        private class MyAttributeHandler : AttributeHandler<MyAttribute>
        {
            private event Action<Attribute> OnHandle;

            public MyAttributeHandler(Action<Attribute> onHandle)
            {
                this.OnHandle += onHandle;
            }

            protected override void HandleAttribute<TSourceEntity>(
                IReportSchemaBuilder<TSourceEntity> builder,
                IReportSchemaCellsProviderBuilder<TSourceEntity> cellsProviderBuilder,
                MyAttribute attribute)
            {
                this.OnHandle?.Invoke(attribute);
            }
        }
    }
}
