using System.Linq;
using Reports.Core.Interfaces;
using Reports.Core.Models;

namespace Reports.Core.SchemaBuilders
{
    public class HorizontalReportSchemaBuilder<TSourceEntity> : ReportSchemaBuilder<TSourceEntity>
    {
        public HorizontalReportSchemaBuilder<TSourceEntity> AddRow(IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertRow(this.CellsProviders.Count, provider);
        }

        public HorizontalReportSchemaBuilder<TSourceEntity> InsertRow(int index, IReportCellsProvider<TSourceEntity> provider)
        {
            this.CurrentProvider = new ConfiguredCellsProvider(provider);

            this.CellsProviders.Insert(index, this.CurrentProvider);
            this.NamedProviders[this.CurrentProvider.Provider.Title] = this.CurrentProvider;

            return this;
        }

        public HorizontalReportSchemaBuilder<TSourceEntity> InsertRowBefore(string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertRow(this.CellsProviders.IndexOf(this.NamedProviders[title]), provider);
        }

        public HorizontalReportSchemaBuilder<TSourceEntity> ForRow(string title)
        {
            this.CurrentProvider = this.NamedProviders[title];

            return this;
        }

        public HorizontalReportSchemaBuilder<TSourceEntity> AddHeaderRow(int rowIndex, IReportCellsProvider<TSourceEntity> provider)
        {
            this.CurrentProvider = new ConfiguredCellsProvider(provider);
            this.HeaderProviders.Insert(rowIndex, this.CurrentProvider);

            return this;
        }

        public HorizontalReportSchema<TSourceEntity> BuildSchema()
        {
            return ReportSchema<TSourceEntity>.CreateHorizontal(
                this.CellsProviders
                    .Select(r => new ReportSchemaCellsProvider<TSourceEntity>(
                        r.Provider,
                        r.CellProperties.ToArray(),
                        r.HeaderProperties.ToArray(),
                        r.CellProcessors.ToArray(),
                        r.HeaderProcessors.ToArray()
                    ))
                    .ToArray(),
                this.TableProperties.ToArray(),
                this.HeaderProviders
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
    }
}
