using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using XReports.Interfaces;
using XReports.Options;
using Xunit;

namespace XReports.Core.Tests.Options
{
    public partial class ReportConverterOptionsTests
    {
        [Fact]
        public void AddHandlersShouldThrowExceptionWhenTypeDoesNotImplementCorrectInterface()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            Action act = () => options.AddHandlers(typeof(ReportCellHandler));

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddHandlersShouldThrowExceptionWhenTypeIsAbstract()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            Action act = () => options.AddHandlers(typeof(AbstractHtmlHandler));

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddHandlersShouldAddTypeWhenItIsCorrect()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            options.AddHandlers(typeof(HtmlHandler));

            options.Types.Should().BeEquivalentTo(typeof(HtmlHandler));
        }

        [Fact]
        public void AddHandlersShouldAddMultipleTypeWhenTheyAreCorrect()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            options.AddHandlers(typeof(HtmlHandler), typeof(AnotherHtmlHandler));

            options.Types.Should().BeEquivalentTo(typeof(HtmlHandler), typeof(AnotherHtmlHandler));
        }

        [Fact]
        public void AddHandlersShouldNotAddDuplicates()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();
            options.AddHandlersByInterface<IPropertyHandler<HtmlCell>>();

            options.AddHandlers(typeof(HtmlHandler), typeof(AnotherHtmlHandler));

            options.Types.Should().BeEquivalentTo(typeof(HtmlHandler), typeof(AnotherHtmlHandler));
        }

        [Fact]
        public void AddHandlersByInterfaceAsGenericArgumentShouldAddAllValidImplementations()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            options.AddHandlersByInterface<IPropertyHandler<HtmlCell>>();

            options.Types.Should().BeEquivalentTo(typeof(HtmlHandler), typeof(AnotherHtmlHandler));
        }

        [Fact]
        public void AddHandlersByInterfaceAsGenericArgumentShouldNotAddDuplicates()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();
            options.AddHandlers(typeof(HtmlHandler));

            options.AddHandlersByInterface<IPropertyHandler<HtmlCell>>();

            try
            {
                options.Types.Should().BeEquivalentTo(typeof(HtmlHandler), typeof(AnotherHtmlHandler));
            }
            catch (ReflectionTypeLoadException e)
            {
                throw new InvalidOperationException(
                    "ReflectionTypeLoadException: " + string.Join("\n", e.LoaderExceptions.Select(ex => ex?.Message)),
                    e);
            }
        }

        [Fact]
        public void AddHandlersByInterfaceAsNonGenericArgumentShouldAddAllValidImplementations()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            options.AddHandlersByInterface(typeof(IPropertyHandler<HtmlCell>));

            options.Types.Should().BeEquivalentTo(typeof(HtmlHandler), typeof(AnotherHtmlHandler));
        }

        [Fact]
        public void AddHandlersByInterfaceAsNonGenericArgumentShouldNotAddDuplicates()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();
            options.AddHandlers(typeof(HtmlHandler));

            options.AddHandlersByInterface(typeof(IPropertyHandler<HtmlCell>));

            options.Types.Should().BeEquivalentTo(typeof(HtmlHandler), typeof(AnotherHtmlHandler));
        }

        [Fact]
        public void AddHandlersFromAssemblyShouldAddAllValidImplementations()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            options.AddHandlersFromAssembly(Assembly.GetExecutingAssembly());

            options.Types.Should().BeEquivalentTo(typeof(HtmlHandler), typeof(AnotherHtmlHandler));
        }

        [Fact]
        public void AddHandlersFromAssemblyShouldNotAddDuplicates()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();
            options.AddHandlers(typeof(HtmlHandler));

            options.AddHandlersFromAssembly(Assembly.GetExecutingAssembly());

            options.Types.Should().BeEquivalentTo(typeof(HtmlHandler), typeof(AnotherHtmlHandler));
        }
    }
}
