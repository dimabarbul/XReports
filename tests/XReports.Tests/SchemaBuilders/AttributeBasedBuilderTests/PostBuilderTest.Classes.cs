using System;
using System.Threading.Tasks;
using XReports.Attributes;
using XReports.Interfaces;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class PostBuilderTest
    {
        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        private class ForVerticalReport
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IVerticalReportPostBuilder<ForVerticalReport>
            {
                public static int Calls { get; set; }

                public void Build(IVerticalReportSchemaBuilder<ForVerticalReport> builder)
                {
                    Calls++;
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        private class ForHorizontalReport
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IHorizontalReportPostBuilder<ForHorizontalReport>
            {
                public static int Calls;

                public void Build(IHorizontalReportSchemaBuilder<ForHorizontalReport> builder)
                {
                    Calls++;
                }
            }
        }

        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        [MyTable]
        private class VerticalWithTrackingPostBuilder
        {
            [ReportVariable(1, "ID")]
            [My]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            [My]
            public string Name { get; set; }

            public class PostBuilder : IVerticalReportPostBuilder<VerticalWithTrackingPostBuilder>
            {
                public static Action OnHandle;

                public void Build(IVerticalReportSchemaBuilder<VerticalWithTrackingPostBuilder> builder)
                {
                    OnHandle?.Invoke();
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        [MyTable]
        private class HorizontalWithTrackingPostBuilder
        {
            [ReportVariable(1, "ID")]
            [My]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            [My]
            public string Name { get; set; }

            public class PostBuilder : IHorizontalReportPostBuilder<HorizontalWithTrackingPostBuilder>
            {
                public static Action OnHandle;

                public void Build(IHorizontalReportSchemaBuilder<HorizontalWithTrackingPostBuilder> builder)
                {
                    OnHandle?.Invoke();
                }
            }
        }

        [VerticalReport(PostBuilder = typeof(ForVerticalReport.PostBuilder))]
        private class VerticalWithWrongPostBuilderEntityType
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }
        }

        [VerticalReport(PostBuilder = typeof(HorizontalPostBuilder))]
        private class VerticalWithWrongPostBuilderInterface
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            private class HorizontalPostBuilder : IHorizontalReportPostBuilder<VerticalWithWrongPostBuilderInterface>
            {
                public void Build(IHorizontalReportSchemaBuilder<VerticalWithWrongPostBuilderInterface> builder)
                {
                }
            }
        }

        [VerticalReport(PostBuilder = typeof(int))]
        private class VerticalWithWrongPostBuilderType
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }
        }

        [HorizontalReport(PostBuilder = typeof(ForHorizontalReport.PostBuilder))]
        private class HorizontalWithWrongPostBuilderEntityType
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }
        }

        [HorizontalReport(PostBuilder = typeof(VerticalPostBuilder))]
        private class HorizontalWithWrongPostBuilderInterface
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            private class VerticalPostBuilder : IVerticalReportPostBuilder<HorizontalWithWrongPostBuilderInterface>
            {
                public void Build(IVerticalReportSchemaBuilder<HorizontalWithWrongPostBuilderInterface> builder)
                {
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(int))]
        private class HorizontalWithWrongPostBuilderType
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }
        }

        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        private class VerticalWithDisposablePostBuilder
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IVerticalReportPostBuilder<VerticalWithDisposablePostBuilder>, IDisposable
            {
                public static int DisposeCalls { get; set; }

                public void Build(IVerticalReportSchemaBuilder<VerticalWithDisposablePostBuilder> builder)
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
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IHorizontalReportPostBuilder<HorizontalWithDisposablePostBuilder>, IDisposable
            {
                public static int DisposeCalls { get; set; }

                public void Build(IHorizontalReportSchemaBuilder<HorizontalWithDisposablePostBuilder> builder)
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
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IVerticalReportPostBuilder<VerticalWithAsyncDisposablePostBuilder>, IAsyncDisposable
            {
                public void Build(IVerticalReportSchemaBuilder<VerticalWithAsyncDisposablePostBuilder> builder)
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
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IHorizontalReportPostBuilder<HorizontalWithAsyncDisposablePostBuilder>, IAsyncDisposable
            {
                public void Build(IHorizontalReportSchemaBuilder<HorizontalWithAsyncDisposablePostBuilder> builder)
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
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IVerticalReportPostBuilder<VerticalWithDisposableAndAsyncDisposablePostBuilder>, IAsyncDisposable, IDisposable
            {
                public static int DisposeCalls { get; set; }
                public static int AsyncDisposeCalls { get; set; }

                public void Build(IVerticalReportSchemaBuilder<VerticalWithDisposableAndAsyncDisposablePostBuilder> builder)
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
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IHorizontalReportPostBuilder<HorizontalWithDisposableAndAsyncDisposablePostBuilder>, IAsyncDisposable, IDisposable
            {
                public static int DisposeCalls { get; set; }
                public static int AsyncDisposeCalls { get; set; }

                public void Build(IHorizontalReportSchemaBuilder<HorizontalWithDisposableAndAsyncDisposablePostBuilder> builder)
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
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IVerticalReportPostBuilder<VerticalWithDisposablePostBuilderWithDependency>, IDisposable
            {
                private readonly Dependency dependency;

                public PostBuilder(Dependency dependency)
                {
                    this.dependency = dependency;
                }

                public void Build(IVerticalReportSchemaBuilder<VerticalWithDisposablePostBuilderWithDependency> builder)
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
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IHorizontalReportPostBuilder<HorizontalWithDisposablePostBuilderWithDependency>, IDisposable
            {
                private readonly Dependency dependency;

                public PostBuilder(Dependency dependency)
                {
                    this.dependency = dependency;
                }

                public void Build(IHorizontalReportSchemaBuilder<HorizontalWithDisposablePostBuilderWithDependency> builder)
                {
                }

                public void Dispose()
                {
                    GC.SuppressFinalize(this);
                    this.dependency.Call();
                }
            }
        }
    }
}
