using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.SchemaBuilders
{
    public class VerticalReportSchemaBuilder<TSourceEntity> : ReportSchemaBuilder<TSourceEntity>
    {
        private readonly List<ComplexHeader> complexHeaders = new List<ComplexHeader>();
        private readonly Dictionary<string, List<ReportCellProperty>> complexHeadersProperties = new Dictionary<string, List<ReportCellProperty>>();

        public VerticalReportSchemaBuilder<TSourceEntity> AddColumn(IReportCellsProvider<TSourceEntity> provider)
        {
            this.CurrentProvider = new ConfiguredCellsProvider(provider);

            this.CellsProviders.Add(this.CurrentProvider);

            return this;
        }

        public VerticalReportSchemaBuilder<TSourceEntity> ForColumn(string title)
        {
            this.CurrentProvider = this.CellsProviders
                .First(c => c.Provider.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

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
                StartIndex = this.CellsProviders.FindIndex(c => c.Provider.Title.Equals(fromColumn, StringComparison.OrdinalIgnoreCase)),
                EndIndex = this.CellsProviders.FindIndex(c => c.Provider.Title.Equals(toColumn ?? fromColumn, StringComparison.OrdinalIgnoreCase)),
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
                this.CellsProviders
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
    }
}
