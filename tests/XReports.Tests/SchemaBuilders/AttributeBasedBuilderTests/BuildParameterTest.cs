using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using XReports.Interfaces;
using XReports.SchemaBuilders;
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
                try
                {
                    builder.GetType()
                        .GetMethod(
                            nameof(AttributeBasedBuilder.BuildSchema),
                            genericParameterCount: 1,
                            Type.EmptyTypes)
                        .MakeGenericMethod(entityType, expectedParameterType)
                        .Invoke(builder, new object[] { 1 });
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException;
                }
            };

            action.Should().ThrowExactly<ArgumentException>();
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
    }
}
