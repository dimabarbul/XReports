using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.SchemaBuilders
{
    public class VerticalReportSchemaBuilder<TSourceEntity>
    {
        private readonly List<CellsProviderWithCellsConfig> columns = new List<CellsProviderWithCellsConfig>();
        private readonly List<ComplexHeader> complexHeaders = new List<ComplexHeader>();
        private readonly Dictionary<string, List<ReportCellProperty>> complexHeadersProperties = new Dictionary<string, List<ReportCellProperty>>();

        private CellsProviderWithCellsConfig currentConfig;

        public VerticalReportSchemaBuilder<TSourceEntity> AddColumn(IReportCellsProvider<TSourceEntity> provider)
        {
            this.currentConfig = new CellsProviderWithCellsConfig(provider);

            this.columns.Add(this.currentConfig);

            return this;
        }

        public VerticalReportSchemaBuilder<TSourceEntity> ForColumn(string title)
        {
            this.currentConfig = this.columns
                .First(c => c.Provider.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            return this;
        }

        public VerticalReportSchemaBuilder<TSourceEntity> AddProperties(params ReportCellProperty[] properties)
        {
            this.currentConfig.CellProperties.AddRange(properties);

            return this;
        }

        public VerticalReportSchemaBuilder<TSourceEntity> AddHeaderProperties(params ReportCellProperty[] properties)
        {
            this.currentConfig.HeaderProperties.AddRange(properties);

            return this;
        }

        public VerticalReportSchemaBuilder<TSourceEntity> AddProcessors(params IReportCellProcessor<TSourceEntity>[] processors)
        {
            this.currentConfig.CellProcessors.AddRange(processors);

            return this;
        }

        public VerticalReportSchemaBuilder<TSourceEntity> AddHeaderProcessors(params IReportCellProcessor<TSourceEntity>[] processors)
        {
            this.currentConfig.HeaderProcessors.AddRange(processors);

            return this;
        }

        public VerticalReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, int fromColumn, int? toColumn = null)
        {
            this.complexHeaders.Add(new ComplexHeader()
            {
                RowIndex = rowIndex,
                Title = title,
                StartIndex = fromColumn,
                EndIndex = toColumn ?? fromColumn,
            });

            return this;
        }

        public VerticalReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, string fromColumn, string toColumn = null)
        {
            this.complexHeaders.Add(new ComplexHeader()
            {
                RowIndex = rowIndex,
                Title = title,
                StartIndex = this.columns.FindIndex(c => c.Provider.Title.Equals(fromColumn, StringComparison.OrdinalIgnoreCase)),
                EndIndex = this.columns.FindIndex(c => c.Provider.Title.Equals(toColumn ?? fromColumn, StringComparison.OrdinalIgnoreCase)),
            });

            return this;
        }

        public VerticalReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(string title, params ReportCellProperty[] properties)
        {
            if (!this.complexHeadersProperties.ContainsKey(title))
            {
                this.complexHeadersProperties.Add(title, new List<ReportCellProperty>());
            }

            this.complexHeadersProperties[title].AddRange(properties);

            return this;
        }

        public VerticalReportSchema<TSourceEntity> BuildSchema()
        {
            return ReportSchema<TSourceEntity>.CreateVertical
            (
                this.columns
                    .Select(c => new ReportSchemaCellsProvider<TSourceEntity>(
                        c.Provider,
                        c.CellProperties.ToArray(),
                        c.HeaderProperties.ToArray(),
                        c.CellProcessors.ToArray(),
                        c.HeaderProcessors.ToArray()
                    ))
                    .ToArray(),
                new ReportCellProperty[0],
                this.complexHeaders.ToArray(),
                this.complexHeadersProperties
                    .ToDictionary(x => x.Key, x => x.Value.ToArray())
            );
        }

        private class CellsProviderWithCellsConfig
        {
            public IReportCellsProvider<TSourceEntity> Provider { get; set; }
            public List<ReportCellProperty> CellProperties { get; set; } = new List<ReportCellProperty>();
            public List<ReportCellProperty> HeaderProperties { get; set; } = new List<ReportCellProperty>();
            public List<IReportCellProcessor<TSourceEntity>> CellProcessors { get; set; } = new List<IReportCellProcessor<TSourceEntity>>();
            public List<IReportCellProcessor<TSourceEntity>> HeaderProcessors { get; set; } = new List<IReportCellProcessor<TSourceEntity>>();

            public CellsProviderWithCellsConfig(IReportCellsProvider<TSourceEntity> provider)
            {
                this.Provider = provider;
            }
        }
    }
}
