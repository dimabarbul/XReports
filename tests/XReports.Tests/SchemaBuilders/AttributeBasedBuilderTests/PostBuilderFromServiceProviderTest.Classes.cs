using System;
using System.Collections.Generic;
using XReports.Attributes;
using XReports.Interfaces;

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

            public class PostBuilder : IVerticalReportPostBuilder<ForVerticalReport>
            {
                private readonly Dependency dependency;

                public PostBuilder(Dependency dependency)
                {
                    this.dependency = dependency;
                }

                public void Build(IVerticalReportSchemaBuilder<ForVerticalReport> builder)
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

            public class PostBuilder : IHorizontalReportPostBuilder<ForHorizontalReport>
            {
                private readonly Dependency dependency;

                public PostBuilder(Dependency dependency)
                {
                    this.dependency = dependency;
                }

                public void Build(IHorizontalReportSchemaBuilder<ForHorizontalReport> builder)
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

            public class PostBuilder : IVerticalReportPostBuilder<VerticalWithTrackingPostBuilder>
            {
                private readonly List<Attribute> attributes;

                public PostBuilder(List<Attribute> attributes)
                {
                    this.attributes = attributes;
                }

                public void Build(IVerticalReportSchemaBuilder<VerticalWithTrackingPostBuilder> builder)
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

            public class PostBuilder : IHorizontalReportPostBuilder<HorizontalWithTrackingPostBuilder>
            {
                private readonly List<Attribute> attributes;

                public PostBuilder(List<Attribute> attributes)
                {
                    this.attributes = attributes;
                }

                public void Build(IHorizontalReportSchemaBuilder<HorizontalWithTrackingPostBuilder> builder)
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
                private readonly Dependency dependency;

                public PostBuilder(Dependency dependency)
                {
                    this.dependency = dependency;
                }

                public void Build(IVerticalReportSchemaBuilder<VerticalWithDisposablePostBuilder> builder)
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

            public class PostBuilder : IHorizontalReportPostBuilder<HorizontalWithDisposablePostBuilder>, IDisposable
            {
                private readonly Dependency dependency;

                public PostBuilder(Dependency dependency)
                {
                    this.dependency = dependency;
                }

                public void Build(IHorizontalReportSchemaBuilder<HorizontalWithDisposablePostBuilder> builder)
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
