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
            Action<ReportConverterOptions<TReportCell>> configure = null)
            where TReportCell : BaseReportCell, new()
        {
            return services.AddScoped<IReportConverter<TReportCell>>(sp => CreateReportConverter(sp, configure));
        }

        public static IServiceCollection AddReportConverter<TReportCell, TPropertyHandler>(
            this IServiceCollection services)
            where TReportCell : BaseReportCell, new()
            where TPropertyHandler : IPropertyHandler<TReportCell>
        {
            return services.AddReportConverter<TReportCell>(o =>
            {
                o.AddHandlersByInterface<TPropertyHandler>();
            });
        }

        public static IServiceCollection AddReportConverter<TReportCell>(
            this IServiceCollection services,
            string name,
            Action<ReportConverterOptions<TReportCell>> configure = null)
            where TReportCell : BaseReportCell, new()
        {
            AddFactoryRegistration(services, name, configure);
            TryRegisterFactory<TReportCell>(services);

            return services;
        }

        public static IServiceCollection AddReportConverter<TReportCell, TPropertyHandler>(
            this IServiceCollection services,
            string name)
            where TReportCell : BaseReportCell, new()
            where TPropertyHandler : IPropertyHandler<TReportCell>
        {
            return services.AddReportConverter<TReportCell>(name, o =>
            {
                o.AddHandlersByInterface<TPropertyHandler>();
            });
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

        private static void TryRegisterFactory<TReportCell>(IServiceCollection services)
            where TReportCell : BaseReportCell, new()
        {
            services.TryAddScoped<IReportConverterFactory<TReportCell>, ReportConverterFactory<TReportCell>>();
        }
    }
}
