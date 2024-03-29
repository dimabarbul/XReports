using System;
using System.Collections.Generic;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.Attributes;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class PostBuilderFromServiceProviderTest
    {
        private class Dependency
        {
            public int CallsCount { get; private set; }

            public void Call()
            {
                this.CallsCount++;
            }
        }

        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        private class ForVerticalReport
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2)]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<ForVerticalReport>
            {
                private readonly Dependency dependency;

                public PostBuilder(Dependency dependency)
                {
                    this.dependency = dependency;
                }

                public void Build(IReportSchemaBuilder<ForVerticalReport> builder, BuildOptions options)
                {
                    this.dependency.Call();
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        private class ForHorizontalReport
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2)]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<ForHorizontalReport>
            {
                private readonly Dependency dependency;

                public PostBuilder(Dependency dependency)
                {
                    this.dependency = dependency;
                }

                public void Build(IReportSchemaBuilder<ForHorizontalReport> builder, BuildOptions options)
                {
                    this.dependency.Call();
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

            [ReportColumn(2)]
            [My]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<VerticalWithTrackingPostBuilder>
            {
                private readonly List<Attribute> attributes;

                public PostBuilder(List<Attribute> attributes)
                {
                    this.attributes = attributes;
                }

                public void Build(IReportSchemaBuilder<VerticalWithTrackingPostBuilder> builder, BuildOptions options)
                {
                    this.attributes.Add(null);
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

            [ReportColumn(2)]
            [My]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<HorizontalWithTrackingPostBuilder>
            {
                private readonly List<Attribute> attributes;

                public PostBuilder(List<Attribute> attributes)
                {
                    this.attributes = attributes;
                }

                public void Build(IReportSchemaBuilder<HorizontalWithTrackingPostBuilder> builder, BuildOptions options)
                {
                    this.attributes.Add(null);
                }
            }
        }

        [VerticalReport(PostBuilder = typeof(ForVerticalReport.PostBuilder))]
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

        [HorizontalReport(PostBuilder = typeof(ForHorizontalReport.PostBuilder))]
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

            [ReportColumn(2)]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<VerticalWithDisposablePostBuilder>, IDisposable
            {
                private readonly Dependency dependency;

                public PostBuilder(Dependency dependency)
                {
                    this.dependency = dependency;
                }

                public void Build(IReportSchemaBuilder<VerticalWithDisposablePostBuilder> builder, BuildOptions options)
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
        private class HorizontalWithDisposablePostBuilder
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2)]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<HorizontalWithDisposablePostBuilder>, IDisposable
            {
                private readonly Dependency dependency;

                public PostBuilder(Dependency dependency)
                {
                    this.dependency = dependency;
                }

                public void Build(IReportSchemaBuilder<HorizontalWithDisposablePostBuilder> builder, BuildOptions options)
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
