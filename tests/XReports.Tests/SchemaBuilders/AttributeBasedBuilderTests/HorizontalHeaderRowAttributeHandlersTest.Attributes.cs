using System;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.AttributeHandlers;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class HorizontalHeaderRowAttributeHandlersTest
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
        private sealed class CustomAttribute : Attribute
        {
        }

        private class CustomAttributeHandler : AttributeHandler<CustomAttribute>
        {
            public int CallsCount { get; private set; }

            protected override void HandleAttribute<TSourceEntity>(
                IReportSchemaBuilder<TSourceEntity> builder,
                IReportColumnBuilder<TSourceEntity> cellsProviderBuilder,
                CustomAttribute attribute)
            {
                this.CallsCount++;
            }
        }
    }
}
