using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using XReports.DependencyInjection;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Tests.DependencyInjection
{
    public partial class AttributeBasedBuilderDITest
    {
        [Fact]
        public void AddAttributeBasedBuilderWithDefaultLifetimeShouldRegister()
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddAttributeBasedBuilder(null);

            serviceCollection.Should().ContainDescriptor<IAttributeBasedBuilder>(ServiceLifetime.Singleton);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddAttributeBasedBuilderWithLifetimeShouldRegister(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddAttributeBasedBuilder(null, lifetime);

            serviceCollection.Should().ContainDescriptor<IAttributeBasedBuilder>(lifetime);
        }

        [Fact]
        public void AddAttributeBasedBuilderWithHandlersAndDefaultLifetimeShouldRegister()
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddAttributeBasedBuilder(
                    o => o.AddFromAssembly<IMyAttributeHandler>(Assembly.GetExecutingAssembly()));

            serviceCollection.Should().ContainDescriptor<IAttributeBasedBuilder>(ServiceLifetime.Singleton);
            serviceCollection.Should().NotContainDescriptor<IAttributeHandler>();
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddAttributeBasedBuilderWithHandlersAndLifetimeShouldRegister(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddAttributeBasedBuilder(
                    o => o.AddFromAssembly<IMyAttributeHandler>(Assembly.GetExecutingAssembly()),
                    lifetime);

            serviceCollection.Should().ContainDescriptor<IAttributeBasedBuilder>(lifetime);
            serviceCollection.Should().NotContainDescriptor<IAttributeHandler>();
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

        [Theory]
        [InlineData(ServiceLifetime.Scoped, ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton, ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Transient, ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton, ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient, ServiceLifetime.Singleton)]
        [InlineData(ServiceLifetime.Scoped, ServiceLifetime.Singleton)]
        public void AddAttributeBasedBuilderShouldNotReregisterHandlerIfItHasBeenRegisteredBefore(ServiceLifetime builderLifetime, ServiceLifetime handlerLifetime)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .Add(new ServiceDescriptor(typeof(IAttributeHandler), typeof(MyAttributeHandler), handlerLifetime))
                .AddAttributeBasedBuilder(
                    o => o.AddFromAssembly(Assembly.GetExecutingAssembly()),
                    builderLifetime);

            IEnumerable<ServiceDescriptor> formatterDescriptors = serviceCollection
                .Where(sd => sd.ServiceType == typeof(IAttributeHandler));

            formatterDescriptors.Should()
                .OnlyContain(sd =>
                    sd.Lifetime == (
                        sd.ImplementationType == typeof(MyAttributeHandler) ?
                            handlerLifetime :
                            builderLifetime));
        }
    }
}
