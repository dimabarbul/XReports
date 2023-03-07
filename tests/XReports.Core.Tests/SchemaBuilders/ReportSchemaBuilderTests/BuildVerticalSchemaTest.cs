using System;
using FluentAssertions;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportSchemaBuilderTests
{
    public class BuildVerticalSchemaTest
    {
        [Fact]
        public void BuildVerticalSchemaShouldThrowWhenNoRowsAdded()
        {
            ReportSchemaBuilder<string> schemaBuilder = new ReportSchemaBuilder<string>();

            Action action = () => schemaBuilder.BuildVerticalSchema();

            action.Should().ThrowExactly<InvalidOperationException>();
        }
    }
}
