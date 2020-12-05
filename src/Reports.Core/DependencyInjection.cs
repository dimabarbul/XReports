using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Reports.Core.Interfaces;
using Reports.Core.Models;
using Reports.Core.Extensions;

namespace Reports.Core
{
    public static class DependencyInjection
    {
        private static readonly Dictionary<string, Tuple<object, Type>> NamedReportHandlers = new Dictionary<string, Tuple<object, Type>>();

        public static IServiceCollection AddReportConverter<TReportCell>(
            this IServiceCollection services, params IPropertyHandler<TReportCell>[] handlers
        )
            where TReportCell : BaseReportCell, new()
        {
            services.AddScoped<ReportConverter<TReportCell>>(
                sp => CreateReportConverter(sp, handlers)
            );

            return services;
        }

        public static IServiceCollection AddReportConverter<TReportCell, TPropertyHandler>(
            this IServiceCollection services, params IPropertyHandler<TReportCell>[] handlers
        )
            where TReportCell : BaseReportCell, new()
            where TPropertyHandler : IPropertyHandler<TReportCell>
        {
            RegisterHandlers(services, typeof(TPropertyHandler));

            services.AddScoped<ReportConverter<TReportCell>>(
                sp => CreateReportConverter(sp, handlers, typeof(TPropertyHandler))
            );

            return services;
        }

        public static IServiceCollection AddReportConverter<TReportCell>(
            this IServiceCollection services, string name, params IPropertyHandler<TReportCell>[] handlers
        )
            where TReportCell : BaseReportCell, new()
        {
            TryRegisterFactory<TReportCell>(services);

            NamedReportHandlers[name] = new Tuple<object, Type>(handlers, null);

            return services;
        }

        public static IServiceCollection AddReportConverter<TReportCell, TPropertyHandler>(
            this IServiceCollection services, string name, params IPropertyHandler<TReportCell>[] handlers
        )
            where TReportCell : BaseReportCell, new()
            where TPropertyHandler : IPropertyHandler<TReportCell>
        {
            RegisterHandlers(services, typeof(TPropertyHandler));
            TryRegisterFactory<TReportCell>(services);

            NamedReportHandlers[name] = new Tuple<object, Type>(handlers, typeof(TPropertyHandler));

            return services;
        }

        private static void TryRegisterFactory<TReportCell>(IServiceCollection services)
            where TReportCell : BaseReportCell, new()
        {
            services.TryAddScoped<Func<string, ReportConverter<TReportCell>>>(sp =>
                key => CreateReportConverter(sp, (IPropertyHandler<TReportCell>[]) NamedReportHandlers[key].Item1, NamedReportHandlers[key].Item2)
            );
        }

        private static void RegisterHandlers(IServiceCollection services, Type type)
        {
            foreach (Type t in type.GetImplementingTypes())
            {
                services.AddScoped(t);
            }
        }

        private static ReportConverter<TReportCell> CreateReportConverter<TReportCell>(
            IServiceProvider sp,
            IPropertyHandler<TReportCell>[] handlers,
            Type markerInterface = null
        )
            where TReportCell : BaseReportCell, new()
        {
            return new ReportConverter<TReportCell>(
                handlers.Concat(
                    markerInterface == null ? new IPropertyHandler<TReportCell>[0] :
                        markerInterface.GetImplementingTypes()
                            .Select(t => (IPropertyHandler<TReportCell>) sp.GetRequiredService(t))
                )
            );
        }
    }
}
