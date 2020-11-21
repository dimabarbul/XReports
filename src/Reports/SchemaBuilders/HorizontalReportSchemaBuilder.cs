using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.SchemaBuilders
{
    public class HorizontalReportSchemaBuilder<TSourceEntity>
    {
        private readonly List<CellsProviderWithCellsConfig> rows = new List<CellsProviderWithCellsConfig>();
        private readonly List<CellsProviderWithCellsConfig> headerRows = new List<CellsProviderWithCellsConfig>();

        private CellsProviderWithCellsConfig currentConfig;

        public HorizontalReportSchemaBuilder<TSourceEntity> AddRow(IReportCellsProvider<TSourceEntity> provider)
        {
            this.currentConfig = new CellsProviderWithCellsConfig(provider);

            this.rows.Add(this.currentConfig);

            return this;
        }

        public HorizontalReportSchemaBuilder<TSourceEntity> ForRow(string title)
        {
            this.currentConfig = this.rows
                .First(c => c.Provider.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            return this;
        }

        public HorizontalReportSchemaBuilder<TSourceEntity> AddHeaderRow(int rowIndex, IReportCellsProvider<TSourceEntity> provider)
        {
            this.currentConfig = new CellsProviderWithCellsConfig(provider);
            this.headerRows.Insert(rowIndex, this.currentConfig);

            return this;
        }

        public HorizontalReportSchemaBuilder<TSourceEntity> AddProperties(params ReportCellProperty[] properties)
        {
            this.currentConfig.CellProperties.AddRange(properties);

            return this;
        }

        public HorizontalReportSchemaBuilder<TSourceEntity> AddHeaderProperties(params ReportCellProperty[] properties)
        {
            this.currentConfig.HeaderProperties.AddRange(properties);

            return this;
        }

        public HorizontalReportSchemaBuilder<TSourceEntity> AddProcessors(params IReportCellProcessor<TSourceEntity>[] processors)
        {
            this.currentConfig.CellProcessors.AddRange(processors);

            return this;
        }

        public HorizontalReportSchemaBuilder<TSourceEntity> AddHeaderProcessors(params IReportCellProcessor<TSourceEntity>[] processors)
        {
            this.currentConfig.HeaderProcessors.AddRange(processors);

            return this;
        }

        public HorizontalReportSchema<TSourceEntity> BuildSchema()
        {
            return ReportSchema<TSourceEntity>.CreateHorizontal(
                this.rows
                    .Select(r => new ReportSchemaCellsProvider<TSourceEntity>(
                        r.Provider,
                        r.CellProperties.ToArray(),
                        r.HeaderProperties.ToArray(),
                        r.CellProcessors.ToArray(),
                        r.HeaderProcessors.ToArray()
                    ))
                    .ToArray(),
                new ReportCellProperty[0],
                this.headerRows
                    .Select(r => new ReportSchemaCellsProvider<TSourceEntity>(
                        r.Provider,
                        r.CellProperties.ToArray(),
                        r.HeaderProperties.ToArray(),
                        r.CellProcessors.ToArray(),
                        r.HeaderProcessors.ToArray()
                    ))
                    .ToArray()
            );
        }

        // public void AddTitleProperty(string title, params ReportCellProperty[] properties)
        // {
        //     if (!this.titleCellProperties.ContainsKey(title))
        //     {
        //         this.titleCellProperties.Add(title, new List<ReportCellProperty>());
        //     }
        //
        //     this.titleCellProperties[title].AddRange(properties);
        // }
        //
        // public void AddTitleProcessor(IReportCellProcessor<TSourceEntity> processor)
        // {
        //     this.titleCellProcessors.Add(processor);
        // }
        //
        // public void AddHeaderProperty(string title, params ReportCellProperty[] properties)
        // {
        //     IReportCellsProvider<TSourceEntity> row = this.headerRows.FirstOrDefault(r => r.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        //     if (row == null)
        //     {
        //         throw new ArgumentException($"Cannot find column {title}");
        //     }
        //
        //     foreach (ReportCellProperty property in properties)
        //     {
        //         row.AddProperty(property);
        //     }
        // }

        // private IEnumerable<IEnumerable<ReportCell>> GetRows(IEnumerable<TSourceEntity> source)
        // {
        //     return this.rows
        //         .Select(row => Enumerable.Repeat(this.CreateTitleCell(row), 1)
        //             .Concat(source.Select(e => row.CellSelector(e)))
        //         );
        // }
        //
        // private ReportCell CreateTitleCell(IReportCellsProvider<TSourceEntity> row)
        // {
        //     ReportCell<string> cell = new ReportCell<string>(row.Title);
        //
        //     if (this.titleCellProperties.ContainsKey(row.Title))
        //     {
        //         cell.Properties.AddRange(this.titleCellProperties[row.Title]);
        //     }
        //
        //     foreach (IReportCellProcessor<TSourceEntity> reportCellProcessor in this.titleCellProcessors)
        //     {
        //         reportCellProcessor.Process(cell, default);
        //     }
        //
        //     return cell;
        // }
        //
        // private IEnumerable<IEnumerable<ReportCell>> GetHeaderRows(IEnumerable<TSourceEntity> source)
        // {
        //     return this.headerRows
        //         .Select(row => Enumerable.Repeat(this.CreateTitleCell(row), 1)
        //             .Concat(source.Select(e => row.CellSelector(e)))
        //         );
        // }

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
