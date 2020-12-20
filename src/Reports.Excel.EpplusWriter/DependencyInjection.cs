using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Reports.Core.Extensions;

namespace Reports.Excel.EpplusWriter
{
    public static class DependencyInjection
    {
        public static IServiceCollection UseEpplusWriter(this IServiceCollection services)
        {
            return services.UseEpplusWriter<IEpplusWriter, EpplusWriter>();
        }

        public static IServiceCollection UseEpplusWriter<TEpplusWriter>(this IServiceCollection services)
            where TEpplusWriter : class, IEpplusWriter, new()
        {
            return services.UseEpplusWriter<IEpplusWriter, TEpplusWriter>();
        }

        public static IServiceCollection UseEpplusWriter<TIEpplusWriter, TEpplusWriter>(this IServiceCollection services)
            where TIEpplusWriter : class, IEpplusWriter
            where TEpplusWriter : class, TIEpplusWriter, new()
        {
            Type[] formatterTypes = typeof(IEpplusFormatter).GetImplementingTypes();

            foreach (Type formatterType in formatterTypes)
            {
                services.AddScoped(formatterType);
            }

            services.AddScoped<TIEpplusWriter, TEpplusWriter>(sp =>
            {
                TEpplusWriter writer = new TEpplusWriter();

                foreach (Type formatterType in formatterTypes)
                {
                    writer.AddFormatter((IEpplusFormatter) sp.GetRequiredService(formatterType));
                }

                return writer;
            });

            if (typeof(IEpplusWriter) != typeof(TIEpplusWriter))
            {
                services.AddScoped<IEpplusWriter, TIEpplusWriter>();
            }

            return services;
        }
    }
}
