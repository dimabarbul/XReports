using System;
using XReports.Interfaces;

namespace XReports.Tests.DependencyInjection
{
    public partial class AttributeBasedBuilderDITest
    {
        private interface IMyAttributeHandler : IAttributeHandler
        {
        }

        private class MyAttributeHandler : IMyAttributeHandler
        {
            public void Handle<TSourceEntity>(
                IReportSchemaBuilder<TSourceEntity> schemaBuilder,
                IReportSchemaCellsProviderBuilder<TSourceEntity> cellsProviderBuilder,
                Attribute attribute)
            {
                throw new NotImplementedException();
            }
        }

        private class MyAnotherAttributeHandler : IMyAttributeHandler
        {
            public void Handle<TSourceEntity>(
                IReportSchemaBuilder<TSourceEntity> schemaBuilder,
                IReportSchemaCellsProviderBuilder<TSourceEntity> cellsProviderBuilder,
                Attribute attribute)
            {
                throw new NotImplementedException();
            }
        }

        private class MyHandlerWithDependency : IAttributeHandler
        {
            public MyHandlerWithDependency(MyAttributeHandler _)
            {
            }

            public void Handle<TSourceEntity>(
                IReportSchemaBuilder<TSourceEntity> schemaBuilder,
                IReportSchemaCellsProviderBuilder<TSourceEntity> cellsProviderBuilder,
                Attribute attribute)
            {
                throw new NotImplementedException();
            }
        }
    }
}
