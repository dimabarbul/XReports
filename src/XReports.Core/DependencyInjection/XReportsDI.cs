using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.DependencyInjection
{
    public static class XReportsDI
    {
        public static IServiceCollection AddReportConverter<TReportCell>(
            this IServiceCollection services,
            params IPropertyHandler<TReportCell>[] handlers)
            where TReportCell : BaseReportCell, new()
        {
            services.AddScoped<IReportConverter<TReportCell>>(
                sp => CreateReportConverter(sp, handlers));

            return services;
        }

        public static IServiceCollection AddReportConverter<TReportCell, TPropertyHandler>(
            this IServiceCollection services,
            params IPropertyHandler<TReportCell>[] handlers)
            where TReportCell : BaseReportCell, new()
            where TPropertyHandler : IPropertyHandler<TReportCell>
        {
            services.AddScoped<IReportConverter<TReportCell>>(
                sp => CreateReportConverter(sp, handlers, typeof(TPropertyHandler)));

            return services;
        }

        public static IServiceCollection AddReportConverter<TReportCell>(
            this IServiceCollection services,
            string name,
            params IPropertyHandler<TReportCell>[] handlers)
            where TReportCell : BaseReportCell, new()
        {
            AddFactoryRegistration(services, name, handlers);
            TryRegisterFactory<TReportCell>(services);

            return services;
        }

        public static IServiceCollection AddReportConverter<TReportCell, TPropertyHandler>(
            this IServiceCollection services,
            string name,
            params IPropertyHandler<TReportCell>[] handlers)
            where TReportCell : BaseReportCell, new()
            where TPropertyHandler : IPropertyHandler<TReportCell>
        {
            AddFactoryRegistration(services, name, handlers, typeof(TPropertyHandler));
            TryRegisterFactory<TReportCell>(services);

            return services;
        }

        internal static IReportConverter<TReportCell> CreateReportConverter<TReportCell>(
            IServiceProvider sp,
            IEnumerable<IPropertyHandler<TReportCell>> handlers,
            Type markerInterface = null)
            where TReportCell : BaseReportCell, new()
        {
            return markerInterface == null ?
                new ReportConverter<TReportCell>(handlers) :
                new ReportConverter<TReportCell>(
                    handlers.Concat(
                        markerInterface.GetImplementingTypes()
                            .Select(t => (IPropertyHandler<TReportCell>)ActivatorUtilities.GetServiceOrCreateInstance(sp, t))));
        }

        private static void AddFactoryRegistration<TReportCell>(
            IServiceCollection services,
            string name,
            IPropertyHandler<TReportCell>[] handlers,
            Type type = null)
            where TReportCell : BaseReportCell, new()
        {
            services.Configure<ReportConverterFactoryOptions<TReportCell>>(
                o =>
                {
                    o.Registrations.Add(
                        new ReportConverterRegistration<TReportCell>()
                        {
                            Name = name,
                            Handlers = handlers,
                            PropertyHandlersInterface = type,
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
