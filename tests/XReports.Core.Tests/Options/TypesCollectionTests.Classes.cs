namespace XReports.Core.Tests.Options
{
    public partial class TypesCollectionTests
    {
        private interface IBaseInterface
        {
        }

        private class ConcreteClass : IBaseInterface
        {
        }

        private class AnotherConcreteClass : IBaseInterface
        {
        }

        private abstract class AbstractClass : IBaseInterface
        {
        }

        private class NotImplementingInterface
        {
        }

        private interface IMyInterface : IBaseInterface
        {
        }

        private class MyConcreteClass : IMyInterface
        {
        }

        private class MyAnotherConcreteClass : MyAbstractClass
        {
        }

        private abstract class MyAbstractClass : IMyInterface
        {
        }
    }
}
