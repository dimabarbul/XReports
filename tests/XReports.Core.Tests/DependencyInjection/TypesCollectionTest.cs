using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using XReports.DependencyInjection;
using Xunit;

namespace XReports.Core.Tests.DependencyInjection
{
    public partial class TypesCollectionTest
    {
        [Fact]
        public void AddShouldThrowExceptionWhenTypeDoesNotImplementCorrectBaseType()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            Action act = () => types.Add(typeof(NotImplementingInterface));

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddShouldThrowExceptionWhenTypeIsAbstract()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            Action act = () => types.Add(typeof(AbstractClass));

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddShouldAddTypeWhenItIsCorrect()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            types.Add(typeof(ConcreteClass));

            types.Should().BeEquivalentTo(typeof(ConcreteClass));
        }

        [Fact]
        public void AddShouldAddMultipleTypeWhenTheyAreCorrect()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            types.Add(typeof(ConcreteClass), typeof(AnotherConcreteClass));

            types.Should().BeEquivalentTo(typeof(ConcreteClass), typeof(AnotherConcreteClass));
        }

        [Fact]
        public void AddShouldNotAddDuplicates()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();
            types.AddByBaseType<IBaseInterface>();

            types.Add(typeof(ConcreteClass), typeof(AnotherConcreteClass));

            types.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddByBaseTypeAsGenericArgumentShouldThrowExceptionWhenInvalidBaseType()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            Action action = () => types.AddByBaseType<IDisposable>();

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddByBaseTypeAsGenericArgumentShouldAddAllValidImplementations()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            types.AddByBaseType<IBaseInterface>();

            types.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddByBaseTypeAsGenericArgumentShouldAddAllValidImplementationsWhenBaseTypeIsClass()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            types.AddByBaseType<MyAbstractClass>();

            types.Should().BeEquivalentTo(typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddByBaseTypeAsGenericArgumentShouldNotAddDuplicates()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();
            types.Add(typeof(ConcreteClass));

            types.AddByBaseType<IBaseInterface>();

            try
            {
                types.Should().BeEquivalentTo(
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
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            types.AddByBaseType(typeof(IBaseInterface));

            types.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddByBaseTypeAsNonGenericArgumentShouldAddAllValidImplementationsWhenBaseTypeIsClass()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            types.AddByBaseType(typeof(MyAbstractClass));

            types.Should().BeEquivalentTo(typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddByBaseTypeAsNonGenericArgumentShouldThrowExceptionWhenInvalidBaseType()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            Action action = () => types.AddByBaseType(typeof(IDisposable));

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddByBaseTypeAsNonGenericArgumentShouldNotAddDuplicates()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();
            types.Add(typeof(ConcreteClass));

            types.AddByBaseType(typeof(IBaseInterface));

            types.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddFromAssemblyShouldAddAllValidImplementations()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            types.AddFromAssembly(Assembly.GetExecutingAssembly());

            types.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddFromAssemblyWithCustomBaseTypeAsNonGenericArgumentShouldAddAllValidImplementations()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            types.AddFromAssembly(Assembly.GetExecutingAssembly(), typeof(IMyInterface));

            types.Should().BeEquivalentTo(
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddFromAssemblyWithCustomBaseTypeAsNonGenericArgumentShouldAddAllValidImplementationsWhenBaseTypeIsClass()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            types.AddFromAssembly(Assembly.GetExecutingAssembly(), typeof(MyAbstractClass));

            types.Should().BeEquivalentTo(typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddFromAssemblyWithCustomBaseTypeAsNonGenericArgumentShouldThrowExceptionWhenInvalidBaseType()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            Action action = () => types.AddFromAssembly(Assembly.GetExecutingAssembly(), typeof(IDisposable));

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddFromAssemblyWithCustomBaseTypeAsGenericArgumentShouldAddAllValidImplementations()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            types.AddFromAssembly<IMyInterface>(Assembly.GetExecutingAssembly());

            types.Should().BeEquivalentTo(
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddFromAssemblyWithCustomBaseTypeAsGenericArgumentShouldAddAllValidImplementationsWhenBaseTypeIsClass()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            types.AddFromAssembly<MyAbstractClass>(Assembly.GetExecutingAssembly());

            types.Should().BeEquivalentTo(typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void AddFromAssemblyShouldNotAddDuplicates()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();
            types.Add(typeof(ConcreteClass));

            types.AddFromAssembly(Assembly.GetExecutingAssembly());

            types.Should().BeEquivalentTo(
                typeof(ConcreteClass),
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void RemoveAsGenericShouldThrowExceptionWhenTypeDoesNotImplementCorrectBaseType()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            Action act = () => types.Remove<NotImplementingInterface>();

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RemoveAsGenericShouldRemoveTheType()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();
            types.AddByBaseType<IBaseInterface>();

            types.Remove<ConcreteClass>();

            types.Should().BeEquivalentTo(
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void RemoveAsGenericShouldNotThrowWhenTypeIsRemovedMultipleTimes()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();
            types.AddByBaseType<IBaseInterface>();

            types.Remove<ConcreteClass>();
            types.Remove<ConcreteClass>();

            types.Should().BeEquivalentTo(
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void RemoveAsGenericShouldNotThrowWhenTypeIsNotLoaded()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();
            types.AddByBaseType<IMyInterface>();

            types.Remove<ConcreteClass>();

            types.Should().BeEquivalentTo(
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void RemoveAsGenericShouldThrowExceptionWhenTypeIsAbstract()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            Action act = () => types.Remove<AbstractClass>();

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RemoveAsGenericShouldThrowExceptionWhenTypeIsInterface()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            Action act = () => types.Remove<IMyInterface>();

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RemoveAsNonGenericShouldThrowExceptionWhenTypeDoesNotImplementCorrectBaseType()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            Action act = () => types.Remove(typeof(NotImplementingInterface));

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RemoveAsNonGenericShouldRemoveTheTypes()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();
            types.AddByBaseType<IBaseInterface>();

            types.Remove(typeof(ConcreteClass), typeof(AnotherConcreteClass));

            types.Should().BeEquivalentTo(
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void RemoveAsNonGenericShouldNotThrowWhenTypeIsRemovedMultipleTimes()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();
            types.AddByBaseType<IBaseInterface>();

            types.Remove(typeof(ConcreteClass));
            types.Remove(typeof(ConcreteClass));

            types.Should().BeEquivalentTo(
                typeof(AnotherConcreteClass),
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void RemoveAsNonGenericShouldNotThrowWhenTypeIsNotLoaded()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();
            types.AddByBaseType<IMyInterface>();

            types.Remove(typeof(ConcreteClass));

            types.Should().BeEquivalentTo(
                typeof(MyConcreteClass),
                typeof(MyAnotherConcreteClass));
        }

        [Fact]
        public void RemoveAsNonGenericShouldThrowWhenTypeIsAbstract()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            Action act = () => types.Remove(typeof(AbstractClass));

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RemoveAsNonGenericShouldThrowWhenTypeIsInterface()
        {
            TypesCollection<IBaseInterface> types = new TypesCollection<IBaseInterface>();

            Action act = () => types.Remove(typeof(IMyInterface));

            act.Should().Throw<ArgumentException>();
        }
    }
}
