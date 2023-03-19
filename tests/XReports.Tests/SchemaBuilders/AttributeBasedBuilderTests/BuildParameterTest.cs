using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class BuildParameterTest
    {
        [Theory]
        [InlineData(typeof(VerticalWithWrongPostBuilderParameterType))]
        [InlineData(typeof(VerticalWithWrongPostBuilderEntityType))]
        [InlineData(typeof(VerticalWithWrongPostBuilderInterface))]
        [InlineData(typeof(VerticalWithWrongPostBuilderType))]
        [InlineData(typeof(HorizontalWithWrongPostBuilderParameterType))]
        [InlineData(typeof(HorizontalWithWrongPostBuilderEntityType))]
        [InlineData(typeof(HorizontalWithWrongPostBuilderInterface))]
        [InlineData(typeof(HorizontalWithWrongPostBuilderType))]
        public void BuildSchemaShouldThrowWhenPostBuilderClassIsWrong(Type entityType)
        {
            Type expectedParameterType = typeof(int);
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            Action action = () =>
            {
                builder.GetType()
                    .GetMethods()
                    .First(mi =>
                        mi.Name == nameof(AttributeBasedBuilder.BuildSchema)
                        && mi.IsGenericMethodDefinition
                        && mi.GetGenericArguments().Length == 2)
                    .MakeGenericMethod(entityType, expectedParameterType)
                    .Invoke(builder, new object[] { 1 });
            };

            action.Should().ThrowExactly<TargetInvocationException>()
                .WithInnerExceptionExactly<ArgumentException>();
        }

        [Fact]
        public void BuildSchemaShouldCallPostBuilderWithParameterForVerticalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            IReportSchema<ForVerticalReport> _ = builder.BuildSchema<ForVerticalReport, int>(1);

            ForVerticalReport.PostBuilder.Parameter.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldCallPostBuilderWithParameterForHorizontalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            IReportSchema<ForHorizontalReport> _ = builder.BuildSchema<ForHorizontalReport, int>(1);

            ForHorizontalReport.PostBuilder.Parameter.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldCallPostBuilderWithParameterAndDependencyForVerticalReport()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<Dependency>()
                .BuildServiceProvider(true);
            AttributeBasedBuilder builder = new AttributeBasedBuilder(serviceProvider, Enumerable.Empty<IAttributeHandler>());

            IReportSchema<ForVerticalReportWithDependency> _ = builder.BuildSchema<ForVerticalReportWithDependency, int>(1);

            Dependency dependency = serviceProvider.GetRequiredService<Dependency>();
            dependency.Parameter.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldCallPostBuilderWithParameterAndDependencyForHorizontalReport()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<Dependency>()
                .BuildServiceProvider(true);
            AttributeBasedBuilder builder = new AttributeBasedBuilder(serviceProvider, Enumerable.Empty<IAttributeHandler>());

            IReportSchema<ForHorizontalReportWithDependency> _ = builder.BuildSchema<ForHorizontalReportWithDependency, int>(1);

            Dependency dependency = serviceProvider.GetRequiredService<Dependency>();
            dependency.Parameter.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldAllowAddingHeaderRowsInPostBuilder()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            IReportSchema<HorizontalWithNewHeaderRowInPostBuilder> schema = builder.BuildSchema<HorizontalWithNewHeaderRowInPostBuilder, int>(1);

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new HorizontalWithNewHeaderRowInPostBuilder()
                {
                    Id = 1,
                    Name = "John Doe",
                },
            });
            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID"),
                    ReportCellHelper.CreateReportCell(1),
                },
            });
            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Name"),
                    ReportCellHelper.CreateReportCell("John Doe"),
                },
            });
        }

        [Fact]
        public void BuildSchemaShouldAllowChangingReportTypeInPostBuilder()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            IReportSchema<HorizontalWithChangedTypeInPostBuilder> schema = builder.BuildSchema<HorizontalWithChangedTypeInPostBuilder, bool>(true);

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new HorizontalWithChangedTypeInPostBuilder()
                {
                    Id = 1,
                    Name = "John Doe",
                },
            });
            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID"),
                    ReportCellHelper.CreateReportCell("Name"),
                },
            });
            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell(1),
                    ReportCellHelper.CreateReportCell("John Doe"),
                },
            });
        }
    }
}
