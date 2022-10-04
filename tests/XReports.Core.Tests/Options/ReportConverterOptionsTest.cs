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
        public void AddHandlersShouldThrowExceptionWhenTypeDoesNotImplementCorrectBaseType()
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
            options.AddHandlersByBaseType<IPropertyHandler<HtmlCell>>();

            options.AddHandlers(typeof(HtmlHandler), typeof(AnotherHtmlHandler));

            options.Types.Should().BeEquivalentTo(
                typeof(HtmlHandler),
                typeof(AnotherHtmlHandler),
                typeof(MyHtmlHandler),
                typeof(MyAnotherHtmlHandler));
        }

        [Fact]
        public void AddHandlersByBaseTypeAsGenericArgumentShouldThrowExceptionWhenInvalidBaseType()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            Action action = () => options.AddHandlersByBaseType<IDisposable>();

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddHandlersByBaseTypeAsGenericArgumentShouldAddAllValidImplementations()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            options.AddHandlersByBaseType<IPropertyHandler<HtmlCell>>();

            options.Types.Should().BeEquivalentTo(
                typeof(HtmlHandler),
                typeof(AnotherHtmlHandler),
                typeof(MyHtmlHandler),
                typeof(MyAnotherHtmlHandler));
        }

        [Fact]
        public void AddHandlersByBaseTypeAsGenericArgumentShouldAddAllValidImplementationsWhenBaseTypeIsClass()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            options.AddHandlersByBaseType<MyAbstractHtmlHandler>();

            options.Types.Should().BeEquivalentTo(typeof(MyAnotherHtmlHandler));
        }

        [Fact]
        public void AddHandlersByBaseTypeAsGenericArgumentShouldNotAddDuplicates()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();
            options.AddHandlers(typeof(HtmlHandler));

            options.AddHandlersByBaseType<IPropertyHandler<HtmlCell>>();

            try
            {
                options.Types.Should().BeEquivalentTo(
                    typeof(HtmlHandler),
                    typeof(AnotherHtmlHandler),
                    typeof(MyHtmlHandler),
                    typeof(MyAnotherHtmlHandler));
            }
            catch (ReflectionTypeLoadException e)
            {
                throw new InvalidOperationException(
                    "ReflectionTypeLoadException: " + string.Join("\n", e.LoaderExceptions.Select(ex => ex?.Message)),
                    e);
            }
        }

        [Fact]
        public void AddHandlersByBaseTypeAsNonGenericArgumentShouldAddAllValidImplementations()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            options.AddHandlersByBaseType(typeof(IPropertyHandler<HtmlCell>));

            options.Types.Should().BeEquivalentTo(
                typeof(HtmlHandler),
                typeof(AnotherHtmlHandler),
                typeof(MyHtmlHandler),
                typeof(MyAnotherHtmlHandler));
        }

        [Fact]
        public void AddHandlersByBaseTypeAsNonGenericArgumentShouldAddAllValidImplementationsWhenBaseTypeIsClass()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            options.AddHandlersByBaseType(typeof(MyAbstractHtmlHandler));

            options.Types.Should().BeEquivalentTo(typeof(MyAnotherHtmlHandler));
        }

        [Fact]
        public void AddHandlersByBaseTypeAsNonGenericArgumentShouldThrowExceptionWhenInvalidBaseType()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            Action action = () => options.AddHandlersByBaseType(typeof(IDisposable));

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddHandlersByBaseTypeAsNonGenericArgumentShouldNotAddDuplicates()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();
            options.AddHandlers(typeof(HtmlHandler));

            options.AddHandlersByBaseType(typeof(IPropertyHandler<HtmlCell>));

            options.Types.Should().BeEquivalentTo(
                typeof(HtmlHandler),
                typeof(AnotherHtmlHandler),
                typeof(MyHtmlHandler),
                typeof(MyAnotherHtmlHandler));
        }

        [Fact]
        public void AddHandlersFromAssemblyShouldAddAllValidImplementations()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            options.AddHandlersFromAssembly(Assembly.GetExecutingAssembly());

            options.Types.Should().BeEquivalentTo(
                typeof(HtmlHandler),
                typeof(AnotherHtmlHandler),
                typeof(MyHtmlHandler),
                typeof(MyAnotherHtmlHandler));
        }

        [Fact]
        public void AddHandlersFromAssemblyWithCustomBaseTypeAsNonGenericArgumentShouldAddAllValidImplementations()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            options.AddHandlersFromAssembly(Assembly.GetExecutingAssembly(), typeof(IMyPropertyHandler));

            options.Types.Should().BeEquivalentTo(
                typeof(MyHtmlHandler),
                typeof(MyAnotherHtmlHandler));
        }

        [Fact]
        public void AddHandlersFromAssemblyWithCustomBaseTypeAsNonGenericArgumentShouldAddAllValidImplementationsWhenBaseTypeIsClass()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            options.AddHandlersFromAssembly(Assembly.GetExecutingAssembly(), typeof(MyAbstractHtmlHandler));

            options.Types.Should().BeEquivalentTo(typeof(MyAnotherHtmlHandler));
        }

        [Fact]
        public void AddHandlersFromAssemblyWithCustomBaseTypeAsNonGenericArgumentShouldThrowExceptionWhenInvalidBaseType()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            Action action = () => options.AddHandlersFromAssembly(Assembly.GetExecutingAssembly(), typeof(IDisposable));

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddHandlersFromAssemblyWithCustomBaseTypeAsGenericArgumentShouldAddAllValidImplementations()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            options.AddHandlersFromAssembly<IMyPropertyHandler>(Assembly.GetExecutingAssembly());

            options.Types.Should().BeEquivalentTo(
                typeof(MyHtmlHandler),
                typeof(MyAnotherHtmlHandler));
        }

        [Fact]
        public void AddHandlersFromAssemblyWithCustomBaseTypeAsGenericArgumentShouldAddAllValidImplementationsWhenBaseTypeIsClass()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();

            options.AddHandlersFromAssembly<MyAbstractHtmlHandler>(Assembly.GetExecutingAssembly());

            options.Types.Should().BeEquivalentTo(typeof(MyAnotherHtmlHandler));
        }

        [Fact]
        public void AddHandlersFromAssemblyShouldNotAddDuplicates()
        {
            ReportConverterOptions<HtmlCell> options = new ReportConverterOptions<HtmlCell>();
            options.AddHandlers(typeof(HtmlHandler));

            options.AddHandlersFromAssembly(Assembly.GetExecutingAssembly());

            options.Types.Should().BeEquivalentTo(
                typeof(HtmlHandler),
                typeof(AnotherHtmlHandler),
                typeof(MyHtmlHandler),
                typeof(MyAnotherHtmlHandler));
        }
    }
}
