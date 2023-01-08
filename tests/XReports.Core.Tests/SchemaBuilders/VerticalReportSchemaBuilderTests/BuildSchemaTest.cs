using System;
using FluentAssertions;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.VerticalReportSchemaBuilderTests
{
    public class BuildSchemaTest
    {
        [Fact]
        public void BuildSchemaShouldThrowWhenNoRowsAdded()
        {
            VerticalReportSchemaBuilder<string> schemaBuilder = new VerticalReportSchemaBuilder<string>();

            Action action = () => schemaBuilder.BuildSchema();

            action.Should().ThrowExactly<InvalidOperationException>();
        }
    }
}
