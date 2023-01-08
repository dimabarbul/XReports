using System;
using FluentAssertions;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.HorizontalReportSchemaBuilderTests
{
    public class BuildSchemaTest
    {
        [Fact]
        public void BuildSchemaShouldThrowWhenNoRowsAdded()
        {
            HorizontalReportSchemaBuilder<string> schemaBuilder = new HorizontalReportSchemaBuilder<string>();

            Action action = () => schemaBuilder.BuildSchema();

            action.Should().ThrowExactly<InvalidOperationException>();
        }
    }
}
