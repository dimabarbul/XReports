using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using XReports.DependencyInjection;
using XReports.Interfaces;
using XReports.SchemaBuilders;
using XReports.Tests.Assertions;
using Xunit;

namespace XReports.Tests.DependencyInjection
{
    public partial class AttributeBasedBuilderDITest
    {
        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddAttributeBasedBuilderWithLifetimeShouldRegister(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddAttributeBasedBuilder(null, lifetime);

            serviceCollection.Should().ContainDescriptor<IAttributeBasedBuilder, AttributeBasedBuilder>(lifetime);
        }

        [Fact]
        public void AddAttributeBasedBuilderShouldRegisterWithHandlers()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddAttributeBasedBuilder(o => o.AddFromAssembly<IMyAttributeHandler>(Assembly.GetExecutingAssembly()))
                .BuildServiceProvider();

            IAttributeBasedBuilder builder = serviceProvider.GetService<IAttributeBasedBuilder>();

            builder.Should().NotBeNull();
            IAttributeHandler[] handlers = this.GetPropertyHandlers(builder);
            handlers.Select(h => h.GetType())
                .Should().BeEquivalentTo(typeof(MyAttributeHandler), typeof(MyAnotherAttributeHandler));
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddAttributeBasedBuilderShouldRegisterWithHandlersAndLifetime(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddAttributeBasedBuilder(
                    o => o.AddFromAssembly<IMyAttributeHandler>(Assembly.GetExecutingAssembly()),
                    lifetime);

            serviceCollection.Should().ContainDescriptor<IAttributeBasedBuilder, AttributeBasedBuilder>(lifetime);
        }

        [Fact]
        public void AddAttributeBasedBuilderShouldThrowExceptionIfRequestedWhenHandlersHaveDependencyNotRegisteredInServiceCollection()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddAttributeBasedBuilder(o => o.Add(typeof(MyHandlerWithDependency)))
                .BuildServiceProvider();

            Action action = () => serviceProvider.GetService<IAttributeBasedBuilder>();

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void AddAttributeBasedBuilderShouldResolveIfRequestedWithHandlersWhenHandlersHaveDependencyRegisteredInServiceCollection()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddTransient<MyAttributeHandler>()
                .AddAttributeBasedBuilder(o => o.Add(typeof(MyHandlerWithDependency)))
                .BuildServiceProvider();

            Action action = () => serviceProvider.GetService<IAttributeBasedBuilder>();

            action.Should().NotThrow();
        }

        private IAttributeHandler[] GetPropertyHandlers(IAttributeBasedBuilder builder)
        {
            FieldInfo fieldInfo = typeof(AttributeBasedBuilder).GetField("handlers", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo == null)
            {
                throw new ArgumentException("Field \"handlers\" is not found in attribute based builder.", nameof(builder));
            }

            return fieldInfo.GetValue(builder) as IAttributeHandler[];
        }
    }
}
