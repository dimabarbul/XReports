using XReports.SchemaBuilders;
using XReports.SchemaBuilders.Attributes;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class BuildParameterTest
    {
        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        private class ForVerticalReport
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<ForVerticalReport, int>
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
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<ForHorizontalReport, int>
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
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            private class VerticalPostBuilder : IReportSchemaPostBuilder<VerticalWithWrongPostBuilderParameterType, string>
            {
                public void Build(IReportSchemaBuilder<VerticalWithWrongPostBuilderParameterType> builder, string parameter, BuildOptions options)
                {
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

            // TODO: rename to something reasonable, like, PostBuilderWithoutBuildParameter
            private class HorizontalPostBuilder : IReportSchemaPostBuilder<VerticalWithWrongPostBuilderInterface>
            {
                public void Build(IReportSchemaBuilder<VerticalWithWrongPostBuilderInterface> builder, BuildOptions options)
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

        [HorizontalReport(PostBuilder = typeof(HorizontalPostBuilder))]
        private class HorizontalWithWrongPostBuilderParameterType
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            private class HorizontalPostBuilder : IReportSchemaPostBuilder<HorizontalWithWrongPostBuilderParameterType, string>
            {
                public void Build(IReportSchemaBuilder<HorizontalWithWrongPostBuilderParameterType> builder, string parameter, BuildOptions options)
                {
                }
            }
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

            // TODO: rename to something reasonable, like, PostBuilderWithoutBuildParameter
            private class VerticalPostBuilder : IReportSchemaPostBuilder<HorizontalWithWrongPostBuilderInterface>
            {
                public void Build(IReportSchemaBuilder<HorizontalWithWrongPostBuilderInterface> builder, BuildOptions options)
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

        private class Dependency
        {
            public int Parameter { get; set; }
        }

        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        private class ForVerticalReportWithDependency
        {
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<ForVerticalReportWithDependency, int>
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
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<ForHorizontalReportWithDependency, int>
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
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<HorizontalWithNewHeaderRowInPostBuilder, int>
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
            [HeaderRow]
            [ReportColumn(1, "ID")]
            public int Id { get; set; }

            [ReportColumn(2, "Name")]
            public string Name { get; set; }

            public class PostBuilder : IReportSchemaPostBuilder<HorizontalWithChangedTypeInPostBuilder, bool>
            {
                public void Build(IReportSchemaBuilder<HorizontalWithChangedTypeInPostBuilder> builder, bool parameter, BuildOptions options)
                {
                    options.IsVertical = parameter;
                }
            }
        }
    }
}
