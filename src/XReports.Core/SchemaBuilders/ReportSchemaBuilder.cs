using System;
using System.Collections.Generic;
using System.Linq;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.SchemaBuilders
{
    public abstract class ReportSchemaBuilder<TSourceEntity> : IReportSchemaBuilder<TSourceEntity>
    {
        private ConfiguredCellsProvider currentProvider;

        private readonly ComplexHeaderBuilder complexHeaderBuilder = new ComplexHeaderBuilder();

        protected List<ConfiguredCellsProvider> CellsProviders { get; } = new List<ConfiguredCellsProvider>();

        protected Dictionary<string, ConfiguredCellsProvider> NamedProviders { get; } = new Dictionary<string, ConfiguredCellsProvider>();

        protected Dictionary<string, List<ReportCellProperty>> ComplexHeadersProperties { get; } = new Dictionary<string, List<ReportCellProperty>>();

        protected List<ReportCellProperty> CommonComplexHeadersProperties { get; } = new List<ReportCellProperty>();

        protected List<ReportCellProperty> GlobalProperties { get; } = new List<ReportCellProperty>();

        protected List<ReportTableProperty> TableProperties { get; } = new List<ReportTableProperty>();

        public IReportSchemaBuilder<TSourceEntity> AddAlias(string alias)
        {
            this.NamedProviders[alias] = this.currentProvider;

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddAlias(string alias, string target)
        {
            this.NamedProviders[alias] = this.NamedProviders[target];

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddGlobalProperties(params ReportCellProperty[] properties)
        {
            this.CheckAllPropertiesNotNull(properties);

            this.GlobalProperties.AddRange(properties);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddTableProperties(params ReportTableProperty[] properties)
        {
            this.CheckAllPropertiesNotNull(properties);

            this.TableProperties.AddRange(properties);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddProperties(params ReportCellProperty[] properties)
        {
            this.CheckAllPropertiesNotNull(properties);

            this.currentProvider.CellProperties.AddRange(properties);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddHeaderProperties(params ReportCellProperty[] properties)
        {
            this.CheckAllPropertiesNotNull(properties);

            this.currentProvider.HeaderProperties.AddRange(properties);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddProcessors(params IReportCellProcessor<TSourceEntity>[] processors)
        {
            this.currentProvider.CellProcessors.AddRange(processors);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddHeaderProcessors(params IReportCellProcessor<TSourceEntity>[] processors)
        {
            this.currentProvider.HeaderProcessors.AddRange(processors);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, int fromColumn, int? toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(rowIndex, title, fromColumn, toColumn);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, string fromColumn, string toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(rowIndex, title, fromColumn, toColumn);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, int rowSpan, string title, int fromColumn, int? toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(rowIndex, rowSpan, title, fromColumn, toColumn);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, int rowSpan, string title, string fromColumn, string toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(rowIndex, rowSpan, title, fromColumn, toColumn);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(string title, params ReportCellProperty[] properties)
        {
            if (!this.ComplexHeadersProperties.ContainsKey(title))
            {
                this.ComplexHeadersProperties.Add(title, new List<ReportCellProperty>());
            }

            this.ComplexHeadersProperties[title].AddRange(properties);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(params ReportCellProperty[] properties)
        {
            this.CheckAllPropertiesNotNull(properties);

            this.CommonComplexHeadersProperties.AddRange(properties);

            return this;
        }

        protected ReportSchemaBuilder<TSourceEntity> SelectProvider(ConfiguredCellsProvider provider)
        {
            this.currentProvider = provider;

            return this;
        }

        protected ReportSchemaBuilder<TSourceEntity> SelectProvider(int index)
        {
            this.currentProvider = this.CellsProviders[index];

            return this;
        }

        protected ReportSchemaBuilder<TSourceEntity> SelectProvider(string title)
        {
            this.currentProvider = this.CellsProviders[this.GetCellsProviderIndex(title)];

            return this;
        }

        protected ReportSchemaBuilder<TSourceEntity> InsertCellsProvider(int index, IReportCellsProvider<TSourceEntity> provider)
        {
            string title = provider.Title;
            if (title == null)
            {
                throw new ArgumentException("Title cannot be null", nameof(provider));
            }

            if (index < 0 || index > this.CellsProviders.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            this.currentProvider = new ConfiguredCellsProvider(provider);

            this.CellsProviders.Insert(index, this.currentProvider);
            if (!this.NamedProviders.ContainsKey(title))
            {
                this.NamedProviders[title] = this.currentProvider;
            }

            return this;
        }

        protected int GetCellsProviderIndex(string title)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            int index;
            if (!this.NamedProviders.ContainsKey(title) ||
                (index = this.CellsProviders.IndexOf(this.NamedProviders[title])) == -1)
            {
                throw new ArgumentException($"Cells provider with title {title} is not found", nameof(title));
            }

            return index;
        }

        protected ReportCellProperty[] AddGlobalProperties(List<ReportCellProperty> cellProperties)
        {
            List<ReportCellProperty> result = new List<ReportCellProperty>(cellProperties);
            for (int i = 0; i < this.GlobalProperties.Count; i++)
            {
                if (!result.Any(p => p.GetType() == this.GlobalProperties[i].GetType()))
                {
                    result.Add(this.GlobalProperties[i]);
                }
            }

            return result.ToArray();
        }

        protected ComplexHeaderCell[,] BuildComplexHeader(bool transpose)
        {
            return this.complexHeaderBuilder.Build(
                this.CellsProviders.Select(p => p.Provider.Title).ToArray(),
                transpose);
        }

        private void CheckAllPropertiesNotNull<TProperty>(TProperty[] properties)
        {
            if (properties.Any(p => p == null))
            {
                throw new ArgumentException("All properties should not be null", nameof(properties));
            }
        }

        protected class ConfiguredCellsProvider
        {
            public ConfiguredCellsProvider(IReportCellsProvider<TSourceEntity> provider)
            {
                this.Provider = provider;
            }

            public IReportCellsProvider<TSourceEntity> Provider { get; }

            public List<ReportCellProperty> CellProperties { get; } = new List<ReportCellProperty>();

            public List<ReportCellProperty> HeaderProperties { get; } = new List<ReportCellProperty>();

            public List<IReportCellProcessor<TSourceEntity>> CellProcessors { get; } = new List<IReportCellProcessor<TSourceEntity>>();

            public List<IReportCellProcessor<TSourceEntity>> HeaderProcessors { get; } = new List<IReportCellProcessor<TSourceEntity>>();
        }
    }
}
