using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.Options;

namespace XReports.DependencyInjection
{
    public static class XReportsDI
    {
        public static IServiceCollection AddReportConverter<TReportCell>(
            this IServiceCollection services,
            Action<ReportConverterOptions<TReportCell>> configure = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TReportCell : BaseReportCell, new()
        {
            services.Add(new ServiceDescriptor(
                typeof(IReportConverter<TReportCell>),
                sp => CreateReportConverter(sp, configure),
                lifetime));

            return services;
        }

        public static IServiceCollection AddReportConverter<TReportCell, TPropertyHandler>(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TReportCell : BaseReportCell, new()
            where TPropertyHandler : IPropertyHandler<TReportCell>
        {
            return services.AddReportConverter<TReportCell>(
                o =>
                {
                    o.AddHandlersByBaseType<TPropertyHandler>();
                },
                lifetime);
        }

        public static IServiceCollection AddReportConverter<TReportCell>(
            this IServiceCollection services,
            string name,
            Action<ReportConverterOptions<TReportCell>> configure = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TReportCell : BaseReportCell, new()
        {
            TryRegisterFactory<TReportCell>(services, lifetime);
            AddFactoryRegistration(services, name, configure);

            return services;
        }

        public static IServiceCollection AddReportConverter<TReportCell, TPropertyHandler>(
            this IServiceCollection services,
            string name,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TReportCell : BaseReportCell, new()
            where TPropertyHandler : IPropertyHandler<TReportCell>
        {
            return services.AddReportConverter<TReportCell>(
                name,
                o =>
                {
                    o.AddHandlersByBaseType<TPropertyHandler>();
                },
                lifetime);
        }

        internal static IReportConverter<TReportCell> CreateReportConverter<TReportCell>(
            IServiceProvider sp,
            Action<ReportConverterOptions<TReportCell>> configure)
            where TReportCell : BaseReportCell, new()
        {
            ReportConverterOptions<TReportCell> options = new ReportConverterOptions<TReportCell>();
            configure?.Invoke(options);

            return new ReportConverter<TReportCell>(
                options.Types
                    .Select(t =>
                        (IPropertyHandler<TReportCell>)ActivatorUtilities.GetServiceOrCreateInstance(sp, t)));
        }

        private static void AddFactoryRegistration<TReportCell>(
            IServiceCollection services,
            string name,
            Action<ReportConverterOptions<TReportCell>> configure)
            where TReportCell : BaseReportCell, new()
        {
            services.Configure<ReportConverterFactoryOptions<TReportCell>>(
                o =>
                {
                    if (o.Registrations.OfType<ReportConverterRegistration<TReportCell>>()
                        .Any(r => r.Name.Equals(name, StringComparison.Ordinal)))
                    {
                        throw new ArgumentException($"Report converter for type {typeof(TReportCell)} and name \"{name}\" has already been registered.", nameof(name));
                    }

                    o.Registrations.Add(
                        new ReportConverterRegistration<TReportCell>()
                        {
                            Name = name,
                            ConfigureOptions = configure,
                        });
                });
        }

        private static void TryRegisterFactory<TReportCell>(IServiceCollection services, ServiceLifetime lifetime)
            where TReportCell : BaseReportCell, new()
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
