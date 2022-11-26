using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using XReports.DependencyInjection;
using Xunit;

namespace XReports.Core.Tests.Options
{
    public partial class TypesCollectionTest
    {
        [Fact]
        public void AddShouldThrowExceptionWhenTypeDoesNotImplementCorrectBaseType()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            Action act = () => options.Add(typeof(NotImplementingInterface));

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddShouldThrowExceptionWhenTypeIsAbstract()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            Action act = () => options.Add(typeof(AbstractClass));

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddShouldAddTypeWhenItIsCorrect()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.Add(typeof(ConcreteClass));

            options.Should().BeEquivalentTo(typeof(ConcreteClass));
        }

        [Fact]
        public void AddShouldAddMultipleTypeWhenTheyAreCorrect()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.Add(typeof(ConcreteClass), typeof(AnotherConcreteClass));

            options.Should().BeEquivalentTo(typeof(ConcreteClass), typeof(AnotherConcreteClass));
        }

        [Fact]
        public void AddShouldNotAddDuplicates()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();
            options.AddByBaseType<IBaseInterface>();

            options.Add(typeof(ConcreteClass), typeof(AnotherConcreteClass));

            options.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddByBaseTypeAsGenericArgumentShouldThrowExceptionWhenInvalidBaseType()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            Action action = () => options.AddByBaseType<IDisposable>();

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddByBaseTypeAsGenericArgumentShouldAddAllValidImplementations()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddByBaseType<IBaseInterface>();

            options.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddByBaseTypeAsGenericArgumentShouldAddAllValidImplementationsWhenBaseTypeIsClass()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddByBaseType<MyAbstractClass>();

            options.Should().BeEquivalentTo(typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddByBaseTypeAsGenericArgumentShouldNotAddDuplicates()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();
            options.Add(typeof(ConcreteClass));

            options.AddByBaseType<IBaseInterface>();

            try
            {
                options.Should().BeEquivalentTo(
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
        public void AddByBaseTypeAsNonGenericArgumentShouldAddAllValidImplementations()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddByBaseType(typeof(IBaseInterface));

            options.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddByBaseTypeAsNonGenericArgumentShouldAddAllValidImplementationsWhenBaseTypeIsClass()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddByBaseType(typeof(MyAbstractClass));

            options.Should().BeEquivalentTo(typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddByBaseTypeAsNonGenericArgumentShouldThrowExceptionWhenInvalidBaseType()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            Action action = () => options.AddByBaseType(typeof(IDisposable));

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddByBaseTypeAsNonGenericArgumentShouldNotAddDuplicates()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();
            options.Add(typeof(ConcreteClass));

            options.AddByBaseType(typeof(IBaseInterface));

            options.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddFromAssemblyShouldAddAllValidImplementations()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddFromAssembly(Assembly.GetExecutingAssembly());

            options.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddFromAssemblyWithCustomBaseTypeAsNonGenericArgumentShouldAddAllValidImplementations()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddFromAssembly(Assembly.GetExecutingAssembly(), typeof(IMyInterface));

            options.Should().BeEquivalentTo(
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddFromAssemblyWithCustomBaseTypeAsNonGenericArgumentShouldAddAllValidImplementationsWhenBaseTypeIsClass()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddFromAssembly(Assembly.GetExecutingAssembly(), typeof(MyAbstractClass));

            options.Should().BeEquivalentTo(typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddFromAssemblyWithCustomBaseTypeAsNonGenericArgumentShouldThrowExceptionWhenInvalidBaseType()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            Action action = () => options.AddFromAssembly(Assembly.GetExecutingAssembly(), typeof(IDisposable));

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddFromAssemblyWithCustomBaseTypeAsGenericArgumentShouldAddAllValidImplementations()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddFromAssembly<IMyInterface>(Assembly.GetExecutingAssembly());

            options.Should().BeEquivalentTo(
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddFromAssemblyWithCustomBaseTypeAsGenericArgumentShouldAddAllValidImplementationsWhenBaseTypeIsClass()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();

            options.AddFromAssembly<MyAbstractClass>(Assembly.GetExecutingAssembly());

            options.Should().BeEquivalentTo(typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddFromAssemblyShouldNotAddDuplicates()
        {
            TypesCollection<IBaseInterface> options = new TypesCollection<IBaseInterface>();
            options.Add(typeof(ConcreteClass));

            options.AddFromAssembly(Assembly.GetExecutingAssembly());

            options.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }
    }
}
