using XReports.Attributes;
using XReports.Interfaces;

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

            public class PostBuilder : IVerticalReportPostBuilder<ForVerticalReport, int>
            {
                public static int Parameter { get; private set; }

                public void Build(IVerticalReportSchemaBuilder<ForVerticalReport> builder, int parameter)
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

            public class PostBuilder : IHorizontalReportPostBuilder<ForHorizontalReport, int>
            {
                public static int Parameter { get; private set; }

                public void Build(IHorizontalReportSchemaBuilder<ForHorizontalReport> builder, int parameter)
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

            private class VerticalPostBuilder : IVerticalReportPostBuilder<VerticalWithWrongPostBuilderParameterType, string>
            {
                public void Build(IVerticalReportSchemaBuilder<VerticalWithWrongPostBuilderParameterType> builder, string parameter)
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

        [HorizontalReport(PostBuilder = typeof(HorizontalPostBuilder))]
        private class HorizontalWithWrongPostBuilderParameterType
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            private class HorizontalPostBuilder : IHorizontalReportPostBuilder<HorizontalWithWrongPostBuilderParameterType, string>
            {
                public void Build(IHorizontalReportSchemaBuilder<HorizontalWithWrongPostBuilderParameterType> builder, string parameter)
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

            public class PostBuilder : IVerticalReportPostBuilder<ForVerticalReportWithDependency, int>
            {
                private readonly Dependency dependency;

                public PostBuilder(Dependency dependency)
                {
                    this.dependency = dependency;
                }

                public void Build(IVerticalReportSchemaBuilder<ForVerticalReportWithDependency> builder, int parameter)
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

            public class PostBuilder : IHorizontalReportPostBuilder<ForHorizontalReportWithDependency, int>
            {
                private readonly Dependency dependency;

                public PostBuilder(Dependency dependency)
                {
                    this.dependency = dependency;
                }

                public void Build(IHorizontalReportSchemaBuilder<ForHorizontalReportWithDependency> builder, int parameter)
                {
                    this.dependency.Parameter = parameter;
                }
            }
        }
    }
}
