using System;
using System.Threading.Tasks;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.Attributes;
using XReports.SchemaBuilders.ReportCellProviders.ValueProviders;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class PostBuilderTest
    {
        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        private class Vertical
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<Vertical>
            {
                public static int Calls { get; set; }

                public void Build(IReportSchemaBuilder<Vertical> builder, BuildOptions options)
                {
                    Calls++;
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        private class Horizontal
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<Horizontal>
            {
                public static int Calls;

                public void Build(IReportSchemaBuilder<Horizontal> builder, BuildOptions options)
                {
                    Calls++;
                }
            }
        }

        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        [MyTable]
        private class VerticalWithTrackingPostBuilder
        {
            [ReportColumn(1, "ID")]
            [My]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            [My]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<VerticalWithTrackingPostBuilder>
            {
                public static Action OnHandle;

                public void Build(IReportSchemaBuilder<VerticalWithTrackingPostBuilder> builder, BuildOptions options)
                {
                    OnHandle?.Invoke();
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        [MyTable]
        private class HorizontalWithTrackingPostBuilder
        {
            [ReportColumn(1, "ID")]
            [My]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            [My]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<HorizontalWithTrackingPostBuilder>
            {
                public static Action OnHandle;

                public void Build(IReportSchemaBuilder<HorizontalWithTrackingPostBuilder> builder, BuildOptions options)
                {
                    OnHandle?.Invoke();
                }
            }
        }

        [VerticalReport(PostBuilder = typeof(Vertical.PostBuilder))]
        private class VerticalWithWrongPostBuilderEntityType
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }
        }

        [VerticalReport(PostBuilder = typeof(HorizontalPostBuilder))]
        private class VerticalWithWrongPostBuilderInterface
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            // TODO: rename to something reasonable, like, PostBuilderWithoutBuildParameter
            private class HorizontalPostBuilder : IReportSchemaPostBuilder<VerticalWithWrongPostBuilderInterface, int>
            {
                public void Build(IReportSchemaBuilder<VerticalWithWrongPostBuilderInterface> builder, int parameter, BuildOptions options)
                {
                }
            }
        }

        [VerticalReport(PostBuilder = typeof(int))]
        private class VerticalWithWrongPostBuilderType
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }
        }

        [HorizontalReport(PostBuilder = typeof(Horizontal.PostBuilder))]
        private class HorizontalWithWrongPostBuilderEntityType
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }
        }

        [HorizontalReport(PostBuilder = typeof(VerticalPostBuilder))]
        private class HorizontalWithWrongPostBuilderInterface
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            // TODO: rename to something reasonable, like, PostBuilderWithoutBuildParameter
            private class VerticalPostBuilder : IReportSchemaPostBuilder<HorizontalWithWrongPostBuilderInterface, int>
            {
                public void Build(IReportSchemaBuilder<HorizontalWithWrongPostBuilderInterface> builder, int parameter, BuildOptions options)
                {
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(int))]
        private class HorizontalWithWrongPostBuilderType
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }
        }

        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        private class VerticalWithDisposablePostBuilder
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<VerticalWithDisposablePostBuilder>, IDisposable
            {
                public static int DisposeCalls { get; set; }

                public void Build(IReportSchemaBuilder<VerticalWithDisposablePostBuilder> builder, BuildOptions options)
                {
                }

                public void Dispose()
                {
                    GC.SuppressFinalize(this);
                    DisposeCalls++;
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        private class HorizontalWithDisposablePostBuilder
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<HorizontalWithDisposablePostBuilder>, IDisposable
            {
                public static int DisposeCalls { get; set; }

                public void Build(IReportSchemaBuilder<HorizontalWithDisposablePostBuilder> builder, BuildOptions options)
                {
                }

                public void Dispose()
                {
                    GC.SuppressFinalize(this);
                    DisposeCalls++;
                }
            }
        }

        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        private class VerticalWithAsyncDisposablePostBuilder
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<VerticalWithAsyncDisposablePostBuilder>, IAsyncDisposable
            {
                public void Build(IReportSchemaBuilder<VerticalWithAsyncDisposablePostBuilder> builder, BuildOptions options)
                {
                }

                public ValueTask DisposeAsync()
                {
                    GC.SuppressFinalize(this);
                    return new ValueTask(Task.CompletedTask);
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        private class HorizontalWithAsyncDisposablePostBuilder
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<HorizontalWithAsyncDisposablePostBuilder>, IAsyncDisposable
            {
                public void Build(IReportSchemaBuilder<HorizontalWithAsyncDisposablePostBuilder> builder, BuildOptions options)
                {
                }

                public ValueTask DisposeAsync()
                {
                    GC.SuppressFinalize(this);
                    return new ValueTask(Task.CompletedTask);
                }
            }
        }

        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        private class VerticalWithDisposableAndAsyncDisposablePostBuilder
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<VerticalWithDisposableAndAsyncDisposablePostBuilder>, IAsyncDisposable, IDisposable
            {
                public static int DisposeCalls { get; set; }
                public static int AsyncDisposeCalls { get; set; }

                public void Build(IReportSchemaBuilder<VerticalWithDisposableAndAsyncDisposablePostBuilder> builder, BuildOptions options)
                {
                }

                public void Dispose()
                {
                    GC.SuppressFinalize(this);
                    DisposeCalls++;
                }

                public ValueTask DisposeAsync()
                {
                    GC.SuppressFinalize(this);
                    AsyncDisposeCalls++;

                    return new ValueTask(Task.CompletedTask);
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        private class HorizontalWithDisposableAndAsyncDisposablePostBuilder
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<HorizontalWithDisposableAndAsyncDisposablePostBuilder>, IAsyncDisposable, IDisposable
            {
                public static int DisposeCalls { get; set; }
                public static int AsyncDisposeCalls { get; set; }

                public void Build(IReportSchemaBuilder<HorizontalWithDisposableAndAsyncDisposablePostBuilder> builder, BuildOptions options)
                {
                }

                public void Dispose()
                {
                    GC.SuppressFinalize(this);
                    DisposeCalls++;
                }

                public ValueTask DisposeAsync()
                {
                    GC.SuppressFinalize(this);
                    AsyncDisposeCalls++;

                    return new ValueTask(Task.CompletedTask);
                }
            }
        }

        private class Dependency
        {
            public int CallsCount { get; private set; }

            public void Call()
            {
                this.CallsCount++;
            }
        }

        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        private class VerticalWithDisposablePostBuilderWithDependency
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<VerticalWithDisposablePostBuilderWithDependency>, IDisposable
            {
                private readonly Dependency dependency;

                public PostBuilder(Dependency dependency)
                {
                    this.dependency = dependency;
                }

                public void Build(IReportSchemaBuilder<VerticalWithDisposablePostBuilderWithDependency> builder, BuildOptions options)
                {
                }

                public void Dispose()
                {
                    GC.SuppressFinalize(this);
                    this.dependency.Call();
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        private class HorizontalWithDisposablePostBuilderWithDependency
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<HorizontalWithDisposablePostBuilderWithDependency>, IDisposable
            {
                private readonly Dependency dependency;

                public PostBuilder(Dependency dependency)
                {
                    this.dependency = dependency;
                }

                public void Build(IReportSchemaBuilder<HorizontalWithDisposablePostBuilderWithDependency> builder, BuildOptions options)
                {
                }

                public void Dispose()
                {
                    GC.SuppressFinalize(this);
                    this.dependency.Call();
                }
            }
        }

        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        private class VerticalForColumnByPropertyName
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<VerticalForColumnByPropertyName>
            {
                public void Build(IReportSchemaBuilder<VerticalForColumnByPropertyName> builder, BuildOptions options)
                {
                    builder.ForColumn(new ColumnId(nameof(VerticalForColumnByPropertyName.Id)))
                        .AddHeaderProperties(new MyProperty());
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        private class HorizontalForRowByPropertyName
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<HorizontalForRowByPropertyName>
            {
                public void Build(IReportSchemaBuilder<HorizontalForRowByPropertyName> builder, BuildOptions options)
                {
                    builder.ForColumn(new ColumnId(nameof(HorizontalForRowByPropertyName.Id)))
                        .AddHeaderProperties(new MyProperty());
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        private class HorizontalWithNewHeaderRowInPostBuilder
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<HorizontalWithNewHeaderRowInPostBuilder>
            {
                public void Build(IReportSchemaBuilder<HorizontalWithNewHeaderRowInPostBuilder> builder, BuildOptions options)
                {
                    builder.InsertColumn(0, "#", new SequentialNumberValueProvider());
                    options.HeaderRowsCount++;
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        private class HorizontalWithChangedTypeInPostBuilder
        {
            [HeaderRow]
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<HorizontalWithChangedTypeInPostBuilder>
            {
                public void Build(IReportSchemaBuilder<HorizontalWithChangedTypeInPostBuilder> builder, BuildOptions options)
                {
                    options.IsVertical = true;
                }
            }
        }
    }
}
