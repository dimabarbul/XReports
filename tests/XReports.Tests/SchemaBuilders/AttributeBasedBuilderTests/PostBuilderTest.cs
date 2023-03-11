using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using XReports.Interfaces;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class PostBuilderTest
    {
        [Fact]
        public void BuildSchemaShouldCallPostBuilderForVerticalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            IReportSchema<Vertical> _ = builder.BuildSchema<Vertical>();

            Vertical.PostBuilder.Calls.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldCallPostBuilderForHorizontalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            IReportSchema<Horizontal> _ = builder.BuildSchema<Horizontal>();

            Horizontal.PostBuilder.Calls.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldCallPostBuilderAfterAttributeHandlersForVerticalReport()
        {
            List<Attribute> processedAttributes = new List<Attribute>();
            MyAttributeHandler myAttributeHandler = new MyAttributeHandler(a => processedAttributes.Add(a));
            MyTableAttributeHandler myTableAttributeHandler = new MyTableAttributeHandler(a => processedAttributes.Add(a));
            VerticalWithTrackingPostBuilder.PostBuilder.OnHandle += () => processedAttributes.Add(null);
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new IAttributeHandler[]
            {
                myAttributeHandler,
                myTableAttributeHandler,
            });

            IReportSchema<VerticalWithTrackingPostBuilder> _ = builder.BuildSchema<VerticalWithTrackingPostBuilder>();

            processedAttributes.Should().Equal(new Attribute[]
            {
                new MyAttribute(),
                new MyAttribute(),
                new MyTableAttribute(),
                null,
            });
        }

        [Fact]
        public void BuildSchemaShouldCallPostBuilderAfterAttributeHandlersForHorizontalReport()
        {
            List<Attribute> processedAttributes = new List<Attribute>();
            MyAttributeHandler myAttributeHandler = new MyAttributeHandler(a => processedAttributes.Add(a));
            MyTableAttributeHandler myTableAttributeHandler = new MyTableAttributeHandler(a => processedAttributes.Add(a));
            HorizontalWithTrackingPostBuilder.PostBuilder.OnHandle += () => processedAttributes.Add(null);
            AttributeBasedBuilder builder = new AttributeBasedBuilder(new IAttributeHandler[]
            {
                myAttributeHandler,
                myTableAttributeHandler,
            });

            IReportSchema<HorizontalWithTrackingPostBuilder> _ = builder.BuildSchema<HorizontalWithTrackingPostBuilder>();

            processedAttributes.Should().Equal(new Attribute[]
            {
                new MyAttribute(),
                new MyAttribute(),
                new MyTableAttribute(),
                null,
            });
        }

        [Theory]
        [InlineData(typeof(VerticalWithWrongPostBuilderEntityType))]
        [InlineData(typeof(VerticalWithWrongPostBuilderInterface))]
        [InlineData(typeof(VerticalWithWrongPostBuilderType))]
        [InlineData(typeof(HorizontalWithWrongPostBuilderEntityType))]
        [InlineData(typeof(HorizontalWithWrongPostBuilderInterface))]
        [InlineData(typeof(HorizontalWithWrongPostBuilderType))]
        public void BuildSchemaShouldThrowWhenPostBuilderClassIsWrong(Type entityType)
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            Action action = () =>
            {
                builder.GetType().GetMethod(nameof(AttributeBasedBuilder.BuildSchema), Array.Empty<Type>())
                    .MakeGenericMethod(entityType)
                    .Invoke(builder, Array.Empty<object>());
            };

            action.Should().ThrowExactly<TargetInvocationException>()
                .WithInnerExceptionExactly<ArgumentException>();
        }

        [Fact]
        public void BuildSchemaShouldDisposePostBuilderWhenItIsDisposableForVerticalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            IReportSchema<VerticalWithDisposablePostBuilder> _ = builder.BuildSchema<VerticalWithDisposablePostBuilder>();

            VerticalWithDisposablePostBuilder.PostBuilder.DisposeCalls.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldDisposePostBuilderWhenItIsDisposableForHorizontalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            IReportSchema<HorizontalWithDisposablePostBuilder> _ = builder.BuildSchema<HorizontalWithDisposablePostBuilder>();

            HorizontalWithDisposablePostBuilder.PostBuilder.DisposeCalls.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldThrowWhenPostBuilderIsAsyncDisposableButNotDisposableForVerticalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            Action action = () => builder.BuildSchema<VerticalWithAsyncDisposablePostBuilder>();

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void BuildSchemaShouldThrowWhenPostBuilderIsAsyncDisposableButNotDisposableForHorizontalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            Action action = () => builder.BuildSchema<HorizontalWithAsyncDisposablePostBuilder>();

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void BuildSchemaShouldDisposePostBuilderSynchronouslyWhenItIsDisposableAndAsyncDisposableForVerticalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            IReportSchema<VerticalWithDisposableAndAsyncDisposablePostBuilder> _ = builder.BuildSchema<VerticalWithDisposableAndAsyncDisposablePostBuilder>();

            VerticalWithDisposableAndAsyncDisposablePostBuilder.PostBuilder.DisposeCalls.Should().Be(1);
            VerticalWithDisposableAndAsyncDisposablePostBuilder.PostBuilder.AsyncDisposeCalls.Should().Be(0);
        }

        [Fact]
        public void BuildSchemaShouldDisposePostBuilderSynchronouslyWhenItIsDisposableAndAsyncDisposableForHorizontalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            IReportSchema<HorizontalWithDisposableAndAsyncDisposablePostBuilder> _ = builder.BuildSchema<HorizontalWithDisposableAndAsyncDisposablePostBuilder>();

            HorizontalWithDisposableAndAsyncDisposablePostBuilder.PostBuilder.DisposeCalls.Should().Be(1);
            HorizontalWithDisposableAndAsyncDisposablePostBuilder.PostBuilder.AsyncDisposeCalls.Should().Be(0);
        }

        [Fact]
        public void BuildSchemaShouldCreateAndDisposePostBuilderWhenItIsDisposableAndHasDependencyAndIsNotInDiContainerForVerticalReport()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<Dependency>()
                .BuildServiceProvider(true);
            AttributeBasedBuilder builder = new AttributeBasedBuilder(serviceProvider, Enumerable.Empty<IAttributeHandler>());

            IReportSchema<VerticalWithDisposablePostBuilderWithDependency> _ = builder.BuildSchema<VerticalWithDisposablePostBuilderWithDependency>();

            Dependency dependency = serviceProvider.GetRequiredService<Dependency>();
            dependency.CallsCount.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldCreateAndDisposePostBuilderWhenItIsDisposableAndHasDependencyAndIsNotInDiContainerForHorizontalReport()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<Dependency>()
                .BuildServiceProvider(true);
            AttributeBasedBuilder builder = new AttributeBasedBuilder(serviceProvider, Enumerable.Empty<IAttributeHandler>());

            IReportSchema<HorizontalWithDisposablePostBuilderWithDependency> _ = builder.BuildSchema<HorizontalWithDisposablePostBuilderWithDependency>();

            Dependency dependency = serviceProvider.GetRequiredService<Dependency>();
            dependency.CallsCount.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldThrowWhenPostBuilderHasDependencyAndThereIsNoDiContainerForVerticalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            Action action = () => builder.BuildSchema<VerticalWithDisposablePostBuilderWithDependency>();

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void BuildSchemaShouldThrowWhenPostBuilderHasDependencyAndThereIsNoDiContainerForHorizontalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            Action action = () => builder.BuildSchema<HorizontalWithDisposablePostBuilderWithDependency>();

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void BuildSchemaShouldThrowWhenPostBuilderHasDependencyWhichIsNotInDiContainerForVerticalReport()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .BuildServiceProvider(true);
            AttributeBasedBuilder builder = new AttributeBasedBuilder(serviceProvider, Enumerable.Empty<IAttributeHandler>());

            Action action = () => builder.BuildSchema<VerticalWithDisposablePostBuilderWithDependency>();

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void BuildSchemaShouldThrowWhenPostBuilderHasDependencyWhichIsNotInDiContainerForHorizontalReport()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .BuildServiceProvider(true);
            AttributeBasedBuilder builder = new AttributeBasedBuilder(serviceProvider, Enumerable.Empty<IAttributeHandler>());

            Action action = () => builder.BuildSchema<HorizontalWithDisposablePostBuilderWithDependency>();

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void BuildSchemaShouldMakeColumnsAvailableInPostBuilderByPropertyNamesForVerticalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            IReportSchema<VerticalForColumnByPropertyName> schema = builder.BuildSchema<VerticalForColumnByPropertyName>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<VerticalForColumnByPropertyName>());
            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID", new MyProperty()),
                    ReportCellHelper.CreateReportCell("Name"),
                },
            });
        }

        [Fact]
        public void BuildSchemaShouldMakeRowsAvailableInPostBuilderByPropertyNamesForHorizontalReport()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            IReportSchema<HorizontalForRowByPropertyName> schema = builder.BuildSchema<HorizontalForRowByPropertyName>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Empty<HorizontalForRowByPropertyName>());
            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID", new MyProperty()),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Name"),
                },
            });
        }

        [Fact]
        public void BuildSchemaShouldAllowAddingHeaderRowsInPostBuilder()
        {
            AttributeBasedBuilder builder = new AttributeBasedBuilder(Enumerable.Empty<IAttributeHandler>());

            IReportSchema<HorizontalWithNewHeaderRowInPostBuilder> schema = builder.BuildSchema<HorizontalWithNewHeaderRowInPostBuilder>();

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
                    ReportCellHelper.CreateReportCell("#"),
                    ReportCellHelper.CreateReportCell(1),
                },
            });
            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("ID"),
                    ReportCellHelper.CreateReportCell(1),
                },
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

            IReportSchema<HorizontalWithChangedTypeInPostBuilder> schema = builder.BuildSchema<HorizontalWithChangedTypeInPostBuilder>();

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
