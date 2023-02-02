using System;
using XReports.AttributeHandlers;
using XReports.Interfaces;

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
                IReportSchemaCellsProviderBuilder<TSourceEntity> cellsProviderBuilder,
                CustomAttribute attribute)
            {
                this.CallsCount++;
            }
        }
    }
}
