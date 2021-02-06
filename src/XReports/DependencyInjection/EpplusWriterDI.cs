using System;
using Microsoft.Extensions.DependencyInjection;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Writers;

namespace XReports.DependencyInjection
{
    public static class EpplusWriterDI
    {
        public static IServiceCollection AddEpplusWriter(this IServiceCollection services)
        {
            return services.AddEpplusWriter<IEpplusWriter, EpplusWriter>();
        }

        public static IServiceCollection AddEpplusWriter<TEpplusWriter>(this IServiceCollection services)
            where TEpplusWriter : class, IEpplusWriter, new()
        {
            return services.AddEpplusWriter<IEpplusWriter, TEpplusWriter>();
        }

        public static IServiceCollection AddEpplusWriter<TIEpplusWriter, TEpplusWriter>(this IServiceCollection services)
            where TIEpplusWriter : class, IEpplusWriter
            where TEpplusWriter : class, TIEpplusWriter, new()
        {
            services.AddScoped<TIEpplusWriter, TEpplusWriter>(sp =>
            {
                TEpplusWriter writer = new TEpplusWriter();

                foreach (Type formatterType in typeof(IEpplusFormatter).GetImplementingTypes())
                {
                    writer.AddFormatter((IEpplusFormatter)ActivatorUtilities.GetServiceOrCreateInstance(sp, formatterType));
                }

                return writer;
            });

            if (typeof(IEpplusWriter) != typeof(TIEpplusWriter))
            {
                services.AddScoped<IEpplusWriter>(sp => sp.GetRequiredService<TIEpplusWriter>());
            }

            return services;
        }
    }
}
