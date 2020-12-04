using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Reports.Extensions;

namespace Reports.Excel.EpplusWriter
{
    public static class DependencyInjection
    {
        public static IServiceCollection UseEpplusWriter(this IServiceCollection services)
        {
            return services.UseEpplusWriter<EpplusWriter>();
        }

        public static IServiceCollection UseEpplusWriter<TEpplusWriter>(this IServiceCollection services)
            where TEpplusWriter : EpplusWriter, new()
        {
            Type[] formatterTypes = typeof(IEpplusFormatter).GetImplementingTypes();

            foreach (Type formatterType in formatterTypes)
            {
                services.AddScoped(formatterType);
            }

            services.AddScoped<EpplusWriter, TEpplusWriter>(sp =>
            {
                TEpplusWriter writer = new TEpplusWriter();

                foreach (Type formatterType in formatterTypes)
                {
                    writer.AddFormatter((IEpplusFormatter) sp.GetRequiredService(formatterType));
                }

                return writer;
            });

            return services;
        }
    }
}
