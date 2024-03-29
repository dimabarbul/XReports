using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using XReports.Converter;
using XReports.Table;

namespace XReports.DependencyInjection
{
    internal class ReportConverterFactory<TReportCell> : IReportConverterFactory<TReportCell>
        where TReportCell : ReportCell, new()
    {
        private readonly ReportConverterFactoryOptions<TReportCell> options;
        private readonly IServiceProvider serviceProvider;

        private readonly Dictionary<string, IReportConverter<TReportCell>> converters =
            new Dictionary<string, IReportConverter<TReportCell>>();

        public ReportConverterFactory(
            IServiceProvider serviceProvider,
            IOptions<ReportConverterFactoryOptions<TReportCell>> options)
        {
            this.serviceProvider = serviceProvider;
            this.options = options?.Value ??
                throw new ArgumentNullException(nameof(options));
        }

        public IReportConverter<TReportCell> Get(string name)
        {
            if (this.converters.ContainsKey(name))
            {
                return this.converters[name];
            }

            ReportConverterRegistration<TReportCell> registration = this.options.Registrations
                .FirstOrDefault(r => r.Name.Equals(name, StringComparison.Ordinal));

            if (registration == null)
            {
                throw new ArgumentException($"Report converter with name \"{name}\" is not registered.");
            }

            IReportConverter<TReportCell> converter = XReportsDI.CreateReportConverter(this.serviceProvider, registration.ConfigureOptions);

            this.converters.Add(name, converter);

            return converter;
        }
    }
}
