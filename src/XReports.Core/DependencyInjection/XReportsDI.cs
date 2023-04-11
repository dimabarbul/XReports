using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using XReports.Converter;
using XReports.Table;

namespace XReports.DependencyInjection
{
    /// <summary>
    /// Extension methods for registering XReport classes in dependency injection service collection.
    /// </summary>
    public static class XReportsDI
    {
        /// <summary>
        /// Registers report converter in <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Service collection to register classes in.</param>
        /// <param name="configure">Action that configures property handlers that will be used by the report converter.</param>
        /// <param name="lifetime">Service lifetime of the report converter.</param>
        /// <typeparam name="TReportCell">Type of cells of report the converter converts to.</typeparam>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddReportConverter<TReportCell>(
            this IServiceCollection services,
            Action<TypesCollection<IPropertyHandler<TReportCell>>> configure = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TReportCell : ReportCell, new()
        {
            services.Add(new ServiceDescriptor(
                typeof(IReportConverter<TReportCell>),
                sp => CreateReportConverter(sp, configure),
                lifetime));

            return services;
        }

        /// <summary>
        /// Registers report converter in <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Service collection to register classes in.</param>
        /// <param name="lifetime">Service lifetime of the report converter.</param>
        /// <typeparam name="TReportCell">Type of cells of report the converter converts to.</typeparam>
        /// <typeparam name="TPropertyHandler">Base type of handlers to use in the converter. All assemblies into current application domain are scanned.</typeparam>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddReportConverter<TReportCell, TPropertyHandler>(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TReportCell : ReportCell, new()
            where TPropertyHandler : IPropertyHandler<TReportCell>
        {
            return services.AddReportConverter<TReportCell>(
                o =>
                {
                    o.AddByBaseType<TPropertyHandler>();
                },
                lifetime);
        }

        /// <summary>
        /// Registers named report converter in <see cref="IServiceCollection"/> through <see cref="IReportConverterFactory{TReportCell}"/>.
        /// </summary>
        /// <param name="services">Service collection to register classes in.</param>
        /// <param name="name">Name by which the converter will be available in <see cref="IReportConverterFactory{TReportCell}"/>.</param>
        /// <param name="configure">Action that configures property handlers that will be used by the report converter.</param>
        /// <param name="lifetime">Service lifetime of the report converter.</param>
        /// <typeparam name="TReportCell">Type of cells of report the converter converts to.</typeparam>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddReportConverter<TReportCell>(
            this IServiceCollection services,
            string name,
            Action<TypesCollection<IPropertyHandler<TReportCell>>> configure = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TReportCell : ReportCell, new()
        {
            TryRegisterFactory<TReportCell>(services, lifetime);
            AddFactoryRegistration(services, name, configure);

            return services;
        }

        /// <summary>
        /// Registers named report converter in <see cref="IServiceCollection"/> through <see cref="IReportConverterFactory{TReportCell}"/>.
        /// </summary>
        /// <param name="services">Service collection to register classes in.</param>
        /// <param name="name">Name by which the converter will be available in <see cref="IReportConverterFactory{TReportCell}"/>.</param>
        /// <param name="lifetime">Service lifetime of the report converter.</param>
        /// <typeparam name="TReportCell">Type of cells of report the converter converts to.</typeparam>
        /// <typeparam name="TPropertyHandler">Base type of handlers to use in the converter. All assemblies into current application domain are scanned.</typeparam>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddReportConverter<TReportCell, TPropertyHandler>(
            this IServiceCollection services,
            string name,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TReportCell : ReportCell, new()
            where TPropertyHandler : IPropertyHandler<TReportCell>
        {
            return services.AddReportConverter<TReportCell>(
                name,
                o =>
                {
                    o.AddByBaseType<TPropertyHandler>();
                },
                lifetime);
        }

        internal static IReportConverter<TReportCell> CreateReportConverter<TReportCell>(
            IServiceProvider sp,
            Action<TypesCollection<IPropertyHandler<TReportCell>>> configure)
            where TReportCell : ReportCell, new()
        {
            TypesCollection<IPropertyHandler<TReportCell>> options = new TypesCollection<IPropertyHandler<TReportCell>>();
            configure?.Invoke(options);

            return new ReportConverter<TReportCell>(
                options.Types
                    .Select(t =>
                        (IPropertyHandler<TReportCell>)ActivatorUtilities.GetServiceOrCreateInstance(sp, t)));
        }

        private static void AddFactoryRegistration<TReportCell>(
            IServiceCollection services,
            string name,
            Action<TypesCollection<IPropertyHandler<TReportCell>>> configure)
            where TReportCell : ReportCell, new()
        {
            services.Configure<ReportConverterFactoryOptions<TReportCell>>(
                o =>
                {
                    if (o.Registrations.OfType<ReportConverterRegistration<TReportCell>>()
                        .Any(r => r.Name.Equals(name, StringComparison.Ordinal)))
                    {
                        throw new ArgumentException($"Report converter for type {typeof(TReportCell)} and name \"{name}\" has already been registered.", nameof(name));
                    }

                    o.Registrations.Add(new ReportConverterRegistration<TReportCell>(name, configure));
                });
        }

        private static void TryRegisterFactory<TReportCell>(IServiceCollection services, ServiceLifetime lifetime)
            where TReportCell : ReportCell, new()
        {
            if (services
                .Any(r =>
                    r.ServiceType == typeof(IReportConverterFactory<TReportCell>)
                    && r.Lifetime != lifetime))
            {
                throw new ArgumentException("Report converter factory has already been registered with different lifetime.", nameof(lifetime));
            }

            services.TryAdd(new ServiceDescriptor(typeof(IReportConverterFactory<TReportCell>), typeof(ReportConverterFactory<TReportCell>), lifetime));
        }
    }
}
