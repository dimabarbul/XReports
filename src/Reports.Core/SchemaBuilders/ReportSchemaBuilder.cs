using System;
using System.Collections.Generic;
using Reports.Core.Interfaces;
using Reports.Core.Models;
using Reports.Core.ReportCellProcessors;

namespace Reports.Core.SchemaBuilders
{
    public abstract class ReportSchemaBuilder<TSourceEntity>
    {
        protected readonly List<ConfiguredCellsProvider> CellsProviders = new List<ConfiguredCellsProvider>();
        protected readonly Dictionary<string, ConfiguredCellsProvider> NamedProviders = new Dictionary<string, ConfiguredCellsProvider>();
        protected readonly List<ConfiguredCellsProvider> HeaderProviders = new List<ConfiguredCellsProvider>();
        protected readonly List<ComplexHeader> ComplexHeaders = new List<ComplexHeader>();
        protected readonly Dictionary<string, List<ReportCellProperty>> ComplexHeadersProperties = new Dictionary<string, List<ReportCellProperty>>();

        protected List<ReportCellProperty> TableProperties { get; set; } = new List<ReportCellProperty>();
        protected ConfiguredCellsProvider CurrentProvider;

        protected class ConfiguredCellsProvider
        {
            public IReportCellsProvider<TSourceEntity> Provider { get; set; }
            public List<ReportCellProperty> CellProperties { get; set; } = new List<ReportCellProperty>();
            public List<ReportCellProperty> HeaderProperties { get; set; } = new List<ReportCellProperty>();
            public List<IReportCellProcessor<TSourceEntity>> CellProcessors { get; set; } = new List<IReportCellProcessor<TSourceEntity>>();
            public List<IReportCellProcessor<TSourceEntity>> HeaderProcessors { get; set; } = new List<IReportCellProcessor<TSourceEntity>>();

            public ConfiguredCellsProvider(IReportCellsProvider<TSourceEntity> provider)
            {
                this.Provider = provider;
            }
        }

        public ReportSchemaBuilder<TSourceEntity> AddAlias(string alias)
        {
            this.NamedProviders[alias] = this.CurrentProvider;

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddAlias(string alias, string target)
        {
            this.NamedProviders[alias] = this.NamedProviders[target];

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddTableProperties(params ReportCellProperty[] properties)
        {
            this.TableProperties.AddRange(properties);

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddProperties(params ReportCellProperty[] properties)
        {
            this.CurrentProvider.CellProperties.AddRange(properties);

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddDynamicProperty(Func<TSourceEntity, ReportCellProperty> propertySelector)
        {
            this.CurrentProvider.CellProcessors.Add(new DynamicPropertiesCellProcessor<TSourceEntity>(propertySelector));

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddHeaderProperties(params ReportCellProperty[] properties)
        {
            this.CurrentProvider.HeaderProperties.AddRange(properties);

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddProcessors(params IReportCellProcessor<TSourceEntity>[] processors)
        {
            this.CurrentProvider.CellProcessors.AddRange(processors);

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddHeaderProcessors(params IReportCellProcessor<TSourceEntity>[] processors)
        {
            this.CurrentProvider.HeaderProcessors.AddRange(processors);

            return this;
        }
        public ReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, int fromColumn, int? toColumn = null)
        {
            this.ComplexHeaders.Add(new ComplexHeader()
            {
                RowIndex = rowIndex,
                Title = title,
                StartIndex = fromColumn,
                EndIndex = toColumn ?? fromColumn,
            });

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, string fromColumn, string toColumn = null)
        {
            this.ComplexHeaders.Add(new ComplexHeader()
            {
                RowIndex = rowIndex,
                Title = title,
                StartIndex = this.CellsProviders.FindIndex(c => c.Provider.Title.Equals(fromColumn, StringComparison.OrdinalIgnoreCase)),
                EndIndex = this.CellsProviders.FindIndex(c => c.Provider.Title.Equals(toColumn ?? fromColumn, StringComparison.OrdinalIgnoreCase)),
            });

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(string title, params ReportCellProperty[] properties)
        {
            if (!this.ComplexHeadersProperties.ContainsKey(title))
            {
                this.ComplexHeadersProperties.Add(title, new List<ReportCellProperty>());
            }

            this.ComplexHeadersProperties[title].AddRange(properties);

            return this;
        }
    }
}
