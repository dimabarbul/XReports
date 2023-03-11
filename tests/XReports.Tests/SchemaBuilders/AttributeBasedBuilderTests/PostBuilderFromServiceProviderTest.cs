using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using XReports.Attributes;
using XReports.Interfaces;
using XReports.Schema;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class PostBuilderFromServiceProviderTest
    {
        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        public void BuildSchemaShouldCallPostBuilderForVerticalReport(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = this.CreateServiceCollection();
            serviceCollection.Add(new ServiceDescriptor(
                typeof(ForVerticalReport.PostBuilder),
                typeof(ForVerticalReport.PostBuilder),
                lifetime));
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);
            AttributeBasedBuilder builder = new AttributeBasedBuilder(serviceProvider, Enumerable.Empty<IAttributeHandler>());

            IReportSchema<ForVerticalReport> _ = builder.BuildSchema<ForVerticalReport>();

            Dependency dependency = serviceProvider.GetRequiredService<Dependency>();
            dependency.CallsCount.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldCallPostBuilderWhenPostBuilderIsScopedForVerticalReport()
        {
            IServiceCollection serviceCollection = this.CreateServiceCollection();
            serviceCollection.AddScoped<ForVerticalReport.PostBuilder>();
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);
            IServiceScope serviceScope = serviceProvider.CreateScope();
            AttributeBasedBuilder builder = new AttributeBasedBuilder(serviceScope.ServiceProvider, Enumerable.Empty<IAttributeHandler>());

            IReportSchema<ForVerticalReport> _ = builder.BuildSchema<ForVerticalReport>();

            Dependency dependency = serviceScope.ServiceProvider.GetRequiredService<Dependency>();
            dependency.CallsCount.Should().Be(1);
            serviceScope.Dispose();
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        public void BuildSchemaShouldCallPostBuilderForHorizontalReport(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = this.CreateServiceCollection();
            serviceCollection.Add(new ServiceDescriptor(
                typeof(ForHorizontalReport.PostBuilder),
                typeof(ForHorizontalReport.PostBuilder),
                lifetime));
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);
            AttributeBasedBuilder builder = new AttributeBasedBuilder(serviceProvider, Enumerable.Empty<IAttributeHandler>());

            IReportSchema<ForHorizontalReport> _ = builder.BuildSchema<ForHorizontalReport>();

            Dependency dependency = serviceProvider.GetRequiredService<Dependency>();
            dependency.CallsCount.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldCallPostBuilderWhenPostBuilderIsScopedForHorizontalReport()
        {
            IServiceCollection serviceCollection = this.CreateServiceCollection();
            serviceCollection.AddScoped<ForHorizontalReport.PostBuilder>();
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);
            IServiceScope serviceScope = serviceProvider.CreateScope();
            AttributeBasedBuilder builder = new AttributeBasedBuilder(serviceScope.ServiceProvider, Enumerable.Empty<IAttributeHandler>());

            IReportSchema<ForHorizontalReport> _ = builder.BuildSchema<ForHorizontalReport>();

            Dependency dependency = serviceScope.ServiceProvider.GetRequiredService<Dependency>();
            dependency.CallsCount.Should().Be(1);
            serviceScope.Dispose();
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        public void BuildSchemaShouldCallPostBuilderWhenAttributeBasedBuilderAndDependencyAreInDiContainerForVerticalReport(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = this.CreateServiceCollection();
            serviceCollection.Add(new ServiceDescriptor(
                typeof(ForVerticalReport.PostBuilder),
                typeof(ForVerticalReport.PostBuilder),
                lifetime));
            serviceCollection.Add(new ServiceDescriptor(
                typeof(AttributeBasedBuilder),
                typeof(AttributeBasedBuilder),
                lifetime));
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);
            AttributeBasedBuilder builder = serviceProvider.GetRequiredService<AttributeBasedBuilder>();

            IReportSchema<ForVerticalReport> _ = builder.BuildSchema<ForVerticalReport>();

            Dependency dependency = serviceProvider.GetRequiredService<Dependency>();
            dependency.CallsCount.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldCallPostBuilderWhenAttributeBasedBuilderAndDependencyAreInDiContainerAsScopedForVerticalReport()
        {
            IServiceCollection serviceCollection = this.CreateServiceCollection();
            serviceCollection.AddScoped<ForVerticalReport.PostBuilder>();
            serviceCollection.AddScoped<AttributeBasedBuilder>();
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);
            IServiceScope serviceScope = serviceProvider.CreateScope();
            AttributeBasedBuilder builder = serviceScope.ServiceProvider.GetRequiredService<AttributeBasedBuilder>();

            IReportSchema<ForVerticalReport> _ = builder.BuildSchema<ForVerticalReport>();

            Dependency dependency = serviceScope.ServiceProvider.GetRequiredService<Dependency>();
            dependency.CallsCount.Should().Be(1);
            serviceScope.Dispose();
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        public void BuildSchemaShouldCallPostBuilderWhenAttributeBasedBuilderAndDependencyAreInDiContainerForHorizontalReport(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = this.CreateServiceCollection();
            serviceCollection.Add(new ServiceDescriptor(
                typeof(ForHorizontalReport.PostBuilder),
                typeof(ForHorizontalReport.PostBuilder),
                lifetime));
            serviceCollection.Add(new ServiceDescriptor(
                typeof(AttributeBasedBuilder),
                typeof(AttributeBasedBuilder),
                lifetime));
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);
            AttributeBasedBuilder builder = serviceProvider.GetRequiredService<AttributeBasedBuilder>();

            IReportSchema<ForHorizontalReport> _ = builder.BuildSchema<ForHorizontalReport>();

            Dependency dependency = serviceProvider.GetRequiredService<Dependency>();
            dependency.CallsCount.Should().Be(1);
        }

        [Fact]
        public void BuildSchemaShouldCallPostBuilderWhenAttributeBasedBuilderAndDependencyAreInDiContainerAsScopedForHorizontalReport()
        {
            IServiceCollection serviceCollection = this.CreateServiceCollection();
            serviceCollection.AddScoped<ForHorizontalReport.PostBuilder>();
            serviceCollection.AddScoped<AttributeBasedBuilder>();
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);
            IServiceScope serviceScope = serviceProvider.CreateScope();
            AttributeBasedBuilder builder = serviceScope.ServiceProvider.GetRequiredService<AttributeBasedBuilder>();

            IReportSchema<ForHorizontalReport> _ = builder.BuildSchema<ForHorizontalReport>();

            Dependency dependency = serviceScope.ServiceProvider.GetRequiredService<Dependency>();
            dependency.CallsCount.Should().Be(1);
            serviceScope.Dispose();
        }

        [Fact]
        public void BuildSchemaShouldCallPostBuilderAfterAttributeHandlersForVerticalReport()
        {
            List<Attribute> processedAttributes = new List<Attribute>();

            IServiceCollection serviceCollection = this.CreateServiceCollection();
            serviceCollection.Add(new ServiceDescriptor(typeof(List<Attribute>), processedAttributes));
            serviceCollection.AddTransient<IAttributeHandler, MyAttributeHandler>();
            serviceCollection.AddTransient<IAttributeHandler, MyTableAttributeHandler>();
            serviceCollection.AddTransient<VerticalWithTrackingPostBuilder.PostBuilder>();
            serviceCollection.AddTransient<AttributeBasedBuilder>();
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);

            AttributeBasedBuilder builder = serviceProvider.GetRequiredService<AttributeBasedBuilder>();

            IReportSchema<VerticalWithTrackingPostBuilder> _ = builder.BuildSchema<VerticalWithTrackingPostBuilder>();

            processedAttributes.Should().Equal(new Attribute[]
            {
                new MyAttribute(),
                new MyAttribute(),
                new MyTableAttribute(),
                null,
            });
        }

        [Fact]
        public void BuildSchemaShouldCallPostBuilderAfterAttributeHandlersForHorizontalReport()
        {
            List<Attribute> processedAttributes = new List<Attribute>();

            IServiceCollection serviceCollection = this.CreateServiceCollection();
            serviceCollection.Add(new ServiceDescriptor(typeof(List<Attribute>), processedAttributes));
            serviceCollection.AddTransient<IAttributeHandler, MyAttributeHandler>();
            serviceCollection.AddTransient<IAttributeHandler, MyTableAttributeHandler>();
            serviceCollection.AddTransient<HorizontalWithTrackingPostBuilder.PostBuilder>();
            serviceCollection.AddTransient<AttributeBasedBuilder>();
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);

            AttributeBasedBuilder builder = serviceProvider.GetRequiredService<AttributeBasedBuilder>();

            IReportSchema<HorizontalWithTrackingPostBuilder> _ = builder.BuildSchema<HorizontalWithTrackingPostBuilder>();

            processedAttributes.Should().Equal(new Attribute[]
            {
                new MyAttribute(),
                new MyAttribute(),
                new MyTableAttribute(),
                null,
            });
        }

        [Theory]
        [InlineData(typeof(VerticalWithWrongPostBuilderEntityType))]
        [InlineData(typeof(VerticalWithWrongPostBuilderInterface))]
        [InlineData(typeof(VerticalWithWrongPostBuilderType))]
        [InlineData(typeof(HorizontalWithWrongPostBuilderEntityType))]
        [InlineData(typeof(HorizontalWithWrongPostBuilderInterface))]
        [InlineData(typeof(HorizontalWithWrongPostBuilderType))]
        public void BuildSchemaShouldThrowWhenPostBuilderClassIsWrong(Type entityType)
        {
            IServiceCollection serviceCollection = this.CreateServiceCollection();
            serviceCollection.AddTransient(entityType.GetCustomAttribute<ReportAttribute>().PostBuilder);
            serviceCollection.AddTransient<AttributeBasedBuilder>();
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);
            AttributeBasedBuilder builder = serviceProvider.GetRequiredService<AttributeBasedBuilder>();

            Action action = () =>
            {
                builder.GetType().GetMethod(nameof(AttributeBasedBuilder.BuildSchema), Array.Empty<Type>())
                    .MakeGenericMethod(entityType)
                    .Invoke(builder, Array.Empty<object>());
            };

            action.Should().ThrowExactly<TargetInvocationException>()
                .WithInnerExceptionExactly<ArgumentException>($"type {entityType} is incorrectly set up");
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        public void BuildSchemaShouldNotDisposePostBuilderWhenItIsDisposableAndRegisteredInDiContainerForVerticalReport(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = this.CreateServiceCollection();
            serviceCollection.Add(new ServiceDescriptor(
                typeof(VerticalWithDisposablePostBuilder.PostBuilder),
                typeof(VerticalWithDisposablePostBuilder.PostBuilder),
                lifetime));
            serviceCollection.Add(new ServiceDescriptor(
                typeof(AttributeBasedBuilder),
                typeof(AttributeBasedBuilder),
                lifetime));
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);
            AttributeBasedBuilder builder = serviceProvider.GetRequiredService<AttributeBasedBuilder>();

            IReportSchema<VerticalWithDisposablePostBuilder> _ = builder.BuildSchema<VerticalWithDisposablePostBuilder>();

            Dependency dependency = serviceProvider.GetRequiredService<Dependency>();
            dependency.CallsCount.Should().Be(0);
        }

        [Fact]
        public void BuildSchemaShouldNotDisposePostBuilderWhenItIsDisposableAndRegisteredInDiContainerAsScopedForVerticalReport()
        {
            IServiceCollection serviceCollection = this.CreateServiceCollection();
            serviceCollection.AddScoped<VerticalWithDisposablePostBuilder.PostBuilder>();
            serviceCollection.AddScoped<AttributeBasedBuilder>();
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);
            IServiceScope serviceScope = serviceProvider.CreateScope();
            AttributeBasedBuilder builder = serviceScope.ServiceProvider.GetRequiredService<AttributeBasedBuilder>();

            IReportSchema<VerticalWithDisposablePostBuilder> _ = builder.BuildSchema<VerticalWithDisposablePostBuilder>();

            Dependency dependency = serviceScope.ServiceProvider.GetRequiredService<Dependency>();
            dependency.CallsCount.Should().Be(0);
            serviceScope.Dispose();
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        public void BuildSchemaShouldNotDisposePostBuilderWhenItIsDisposableAndRegisteredInDiContainerForHorizontalReport(ServiceLifetime lifetime)
        {
            IServiceCollection serviceCollection = this.CreateServiceCollection();
            serviceCollection.Add(new ServiceDescriptor(
                typeof(HorizontalWithDisposablePostBuilder.PostBuilder),
                typeof(HorizontalWithDisposablePostBuilder.PostBuilder),
                lifetime));
            serviceCollection.Add(new ServiceDescriptor(
                typeof(AttributeBasedBuilder),
                typeof(AttributeBasedBuilder),
                lifetime));
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);
            AttributeBasedBuilder builder = serviceProvider.GetRequiredService<AttributeBasedBuilder>();

            IReportSchema<HorizontalWithDisposablePostBuilder> _ = builder.BuildSchema<HorizontalWithDisposablePostBuilder>();

            Dependency dependency = serviceProvider.GetRequiredService<Dependency>();
            dependency.CallsCount.Should().Be(0);
        }

        [Fact]
        public void BuildSchemaShouldNotDisposePostBuilderWhenItIsDisposableAndRegisteredInDiContainerAsScopedForHorizontalReport()
        {
            IServiceCollection serviceCollection = this.CreateServiceCollection();
            serviceCollection.AddScoped<HorizontalWithDisposablePostBuilder.PostBuilder>();
            serviceCollection.AddScoped<AttributeBasedBuilder>();
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);
            IServiceScope serviceScope = serviceProvider.CreateScope();
            AttributeBasedBuilder builder = serviceScope.ServiceProvider.GetRequiredService<AttributeBasedBuilder>();

            IReportSchema<HorizontalWithDisposablePostBuilder> _ = builder.BuildSchema<HorizontalWithDisposablePostBuilder>();

            Dependency dependency = serviceScope.ServiceProvider.GetRequiredService<Dependency>();
            dependency.CallsCount.Should().Be(0);
            serviceScope.Dispose();
        }

        private IServiceCollection CreateServiceCollection()
        {
            return new ServiceCollection()
                .AddSingleton<Dependency>();
        }
    }
}
