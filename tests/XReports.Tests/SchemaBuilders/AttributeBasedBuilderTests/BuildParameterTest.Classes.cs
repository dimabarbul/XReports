using XReports.Attributes;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class BuildParameterTest
    {
        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        private class ForVerticalReport
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportPostBuilder<ForVerticalReport, int>
            {
                public static int Parameter { get; private set; }

                public void Build(IReportSchemaBuilder<ForVerticalReport> builder, int parameter, BuildOptions options)
                {
                    Parameter = parameter;
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

            public class PostBuilder : IReportPostBuilder<ForHorizontalReport, int>
            {
                public static int Parameter { get; private set; }

                public void Build(IReportSchemaBuilder<ForHorizontalReport> builder, int parameter, BuildOptions options)
                {
                    Parameter = parameter;
                }
            }
        }

        [VerticalReport(PostBuilder = typeof(VerticalPostBuilder))]
        private class VerticalWithWrongPostBuilderParameterType
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            private class VerticalPostBuilder : IReportPostBuilder<VerticalWithWrongPostBuilderParameterType, string>
            {
                public void Build(IReportSchemaBuilder<VerticalWithWrongPostBuilderParameterType> builder, string parameter, BuildOptions options)
                {
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

            private class HorizontalPostBuilder : IReportPostBuilder<VerticalWithWrongPostBuilderInterface>
            {
                public void Build(IReportSchemaBuilder<VerticalWithWrongPostBuilderInterface> builder, BuildOptions options)
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

        [HorizontalReport(PostBuilder = typeof(HorizontalPostBuilder))]
        private class HorizontalWithWrongPostBuilderParameterType
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            private class HorizontalPostBuilder : IReportPostBuilder<HorizontalWithWrongPostBuilderParameterType, string>
            {
                public void Build(IReportSchemaBuilder<HorizontalWithWrongPostBuilderParameterType> builder, string parameter, BuildOptions options)
                {
                }
            }
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

            private class VerticalPostBuilder : IReportPostBuilder<HorizontalWithWrongPostBuilderInterface>
            {
                public void Build(IReportSchemaBuilder<HorizontalWithWrongPostBuilderInterface> builder, BuildOptions options)
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

        private class Dependency
        {
            public int Parameter { get; set; }
        }

        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        private class ForVerticalReportWithDependency
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportPostBuilder<ForVerticalReportWithDependency, int>
            {
                private readonly Dependency dependency;

                public PostBuilder(Dependency dependency)
                {
                    this.dependency = dependency;
                }

                public void Build(IReportSchemaBuilder<ForVerticalReportWithDependency> builder, int parameter, BuildOptions options)
                {
                    this.dependency.Parameter = parameter;
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        private class ForHorizontalReportWithDependency
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportPostBuilder<ForHorizontalReportWithDependency, int>
            {
                private readonly Dependency dependency;

                public PostBuilder(Dependency dependency)
                {
                    this.dependency = dependency;
                }

                public void Build(IReportSchemaBuilder<ForHorizontalReportWithDependency> builder, int parameter, BuildOptions options)
                {
                    this.dependency.Parameter = parameter;
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        private class HorizontalWithNewHeaderRowInPostBuilder
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportPostBuilder<HorizontalWithNewHeaderRowInPostBuilder, int>
            {
                public void Build(IReportSchemaBuilder<HorizontalWithNewHeaderRowInPostBuilder> builder, int parameter, BuildOptions options)
                {
                    options.HeaderRowsCount = parameter;
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        private class HorizontalWithChangedTypeInPostBuilder
        {
            [HeaderRow(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportPostBuilder<HorizontalWithChangedTypeInPostBuilder, bool>
            {
                public void Build(IReportSchemaBuilder<HorizontalWithChangedTypeInPostBuilder> builder, bool parameter, BuildOptions options)
                {
                    options.IsVertical = parameter;
                }
            }
        }
    }
}
