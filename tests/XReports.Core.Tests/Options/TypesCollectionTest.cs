using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using XReports.DependencyInjection;
using Xunit;

namespace XReports.Core.Tests.Options
{
    public partial class TypesCollectionTests
    {
        [Fact]
        public void AddHandlersShouldThrowExceptionWhenTypeDoesNotImplementCorrectBaseType()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            Action act = () => options.Add(typeof(NotImplementingInterface));

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddHandlersShouldThrowExceptionWhenTypeIsAbstract()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            Action act = () => options.Add(typeof(AbstractClass));

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddHandlersShouldAddTypeWhenItIsCorrect()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.Add(typeof(ConcreteClass));

            options.Types.Should().BeEquivalentTo(typeof(ConcreteClass));
        }

        [Fact]
        public void AddHandlersShouldAddMultipleTypeWhenTheyAreCorrect()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.Add(typeof(ConcreteClass), typeof(AnotherConcreteClass));

            options.Types.Should().BeEquivalentTo(typeof(ConcreteClass), typeof(AnotherConcreteClass));
        }

        [Fact]
        public void AddHandlersShouldNotAddDuplicates()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();
            options.AddByBaseType<IBaseInterface>();

            options.Add(typeof(ConcreteClass), typeof(AnotherConcreteClass));

            options.Types.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddHandlersByBaseTypeAsGenericArgumentShouldThrowExceptionWhenInvalidBaseType()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            Action action = () => options.AddByBaseType<IDisposable>();

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddHandlersByBaseTypeAsGenericArgumentShouldAddAllValidImplementations()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddByBaseType<IBaseInterface>();

            options.Types.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddHandlersByBaseTypeAsGenericArgumentShouldAddAllValidImplementationsWhenBaseTypeIsClass()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddByBaseType<MyAbstractClass>();

            options.Types.Should().BeEquivalentTo(typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddHandlersByBaseTypeAsGenericArgumentShouldNotAddDuplicates()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();
            options.Add(typeof(ConcreteClass));

            options.AddByBaseType<IBaseInterface>();

            try
            {
                options.Types.Should().BeEquivalentTo(
                    typeof(ConcreteClass),
                    typeof(AnotherConcreteClass),
                    typeof(MyConcreteClass),
                    typeof(MyAnotherConcreteClass));
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
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddByBaseType(typeof(IBaseInterface));

            options.Types.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddHandlersByBaseTypeAsNonGenericArgumentShouldAddAllValidImplementationsWhenBaseTypeIsClass()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddByBaseType(typeof(MyAbstractClass));

            options.Types.Should().BeEquivalentTo(typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddHandlersByBaseTypeAsNonGenericArgumentShouldThrowExceptionWhenInvalidBaseType()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            Action action = () => options.AddByBaseType(typeof(IDisposable));

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddHandlersByBaseTypeAsNonGenericArgumentShouldNotAddDuplicates()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();
            options.Add(typeof(ConcreteClass));

            options.AddByBaseType(typeof(IBaseInterface));

            options.Types.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddHandlersFromAssemblyShouldAddAllValidImplementations()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddFromAssembly(Assembly.GetExecutingAssembly());

            options.Types.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddHandlersFromAssemblyWithCustomBaseTypeAsNonGenericArgumentShouldAddAllValidImplementations()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddFromAssembly(Assembly.GetExecutingAssembly(), typeof(IMyInterface));

            options.Types.Should().BeEquivalentTo(
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddHandlersFromAssemblyWithCustomBaseTypeAsNonGenericArgumentShouldAddAllValidImplementationsWhenBaseTypeIsClass()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddFromAssembly(Assembly.GetExecutingAssembly(), typeof(MyAbstractClass));

            options.Types.Should().BeEquivalentTo(typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddHandlersFromAssemblyWithCustomBaseTypeAsNonGenericArgumentShouldThrowExceptionWhenInvalidBaseType()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            Action action = () => options.AddFromAssembly(Assembly.GetExecutingAssembly(), typeof(IDisposable));

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddHandlersFromAssemblyWithCustomBaseTypeAsGenericArgumentShouldAddAllValidImplementations()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddFromAssembly<IMyInterface>(Assembly.GetExecutingAssembly());

            options.Types.Should().BeEquivalentTo(
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddHandlersFromAssemblyWithCustomBaseTypeAsGenericArgumentShouldAddAllValidImplementationsWhenBaseTypeIsClass()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddFromAssembly<MyAbstractClass>(Assembly.GetExecutingAssembly());

            options.Types.Should().BeEquivalentTo(typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddHandlersFromAssemblyShouldNotAddDuplicates()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();
            options.Add(typeof(ConcreteClass));

            options.AddFromAssembly(Assembly.GetExecutingAssembly());

            options.Types.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }
    }
}
