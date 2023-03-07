using System;
using System.Collections.Generic;
using XReports.Attributes;
using XReports.Interfaces;
using XReports.Models;

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
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportPostBuilder<ForVerticalReport>
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
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportPostBuilder<ForHorizontalReport>
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
            [ReportVariable(1, "ID")]
            [My]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            [My]
            public string Name { get; set; }

            public class PostBuilder : IReportPostBuilder<VerticalWithTrackingPostBuilder>
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
            [ReportVariable(1, "ID")]
            [My]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            [My]
            public string Name { get; set; }

            public class PostBuilder : IReportPostBuilder<HorizontalWithTrackingPostBuilder>
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
            [ReportVariable(1, "ID")]
            public int Id { get; set; }
        }

        [VerticalReport(PostBuilder = typeof(HorizontalPostBuilder))]
        private class VerticalWithWrongPostBuilderInterface
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            private class HorizontalPostBuilder : IReportPostBuilder<VerticalWithWrongPostBuilderInterface, int>
            {
                public void Build(IReportSchemaBuilder<VerticalWithWrongPostBuilderInterface> builder, int parameter, BuildOptions options)
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

            private class VerticalPostBuilder : IReportPostBuilder<HorizontalWithWrongPostBuilderInterface, int>
            {
                public void Build(IReportSchemaBuilder<HorizontalWithWrongPostBuilderInterface> builder, int parameter, BuildOptions options)
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

            public class PostBuilder : IReportPostBuilder<VerticalWithDisposablePostBuilder>, IDisposable
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
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportPostBuilder<HorizontalWithDisposablePostBuilder>, IDisposable
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
