using System.Collections.Generic;
using System.Linq;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.SchemaBuilders
{
    public class HorizontalReportSchemaBuilder<TSourceEntity> : ReportSchemaBuilder<TSourceEntity>, IHorizontalReportSchemaBuilder<TSourceEntity>
    {
        private readonly List<ConfiguredCellsProvider> headerProviders = new List<ConfiguredCellsProvider>();

        public IHorizontalReportSchemaBuilder<TSourceEntity> AddRow(IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertRow(this.CellsProviders.Count, provider);
        }

        public IHorizontalReportSchemaBuilder<TSourceEntity> InsertRow(int index, IReportCellsProvider<TSourceEntity> provider)
        {
            this.InsertCellsProvider(index, provider);

            return this;
        }

        public IHorizontalReportSchemaBuilder<TSourceEntity> InsertRowBefore(string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertRow(this.GetCellsProviderIndex(title), provider);
        }

        public IHorizontalReportSchemaBuilder<TSourceEntity> ForRow(string title)
        {
            this.SelectProvider(title);

            return this;
        }

        public IHorizontalReportSchemaBuilder<TSourceEntity> AddHeaderRow(IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertHeaderRow(this.headerProviders.Count, provider);
        }

        public IHorizontalReportSchemaBuilder<TSourceEntity> InsertHeaderRow(int rowIndex, IReportCellsProvider<TSourceEntity> provider)
        {
            ConfiguredCellsProvider configuredCellsProvider = new ConfiguredCellsProvider(provider);
            this.headerProviders.Insert(rowIndex, configuredCellsProvider);

            this.SelectProvider(configuredCellsProvider);

            return this;
        }

        public IHorizontalReportSchemaBuilder<TSourceEntity> ForHeaderRow(int index)
        {
            this.SelectProvider(this.headerProviders[index]);

            return this;
        }

        public HorizontalReportSchema<TSourceEntity> BuildSchema()
        {
            return ReportSchema<TSourceEntity>.CreateHorizontal(
                this.CellsProviders
                    .Select(
                        r => new ReportSchemaCellsProvider<TSourceEntity>(
                            r.Provider,
                            r.CellProperties.ToArray(),
                            r.HeaderProperties.ToArray(),
                            r.CellProcessors.ToArray(),
                            r.HeaderProcessors.ToArray()))
                    .ToArray(),
                this.TableProperties.ToArray(),
                this.ComplexHeaders.ToArray(),
                this.ComplexHeadersProperties
                    .ToDictionary(x => x.Key, x => x.Value.ToArray()),
                this.headerProviders
                    .Select(
                        r => new ReportSchemaCellsProvider<TSourceEntity>(
                            r.Provider,
                            r.CellProperties.ToArray(),
                            r.HeaderProperties.ToArray(),
                            r.CellProcessors.ToArray(),
                            r.HeaderProcessors.ToArray()))
                    .ToArray(),
                this.CommonComplexHeadersProperties.ToArray());
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
    }
}
