using System;
using XReports.AttributeHandlers;
using XReports.SchemaBuilder;

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
