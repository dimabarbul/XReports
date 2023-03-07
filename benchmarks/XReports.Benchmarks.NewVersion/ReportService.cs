using System.Data;
using XReports.Benchmarks.Core.Interfaces;
using XReports.Benchmarks.Core.Models;
using XReports.Benchmarks.Core.ReportStructure;
using XReports.Benchmarks.Core.ReportStructure.Models;
using XReports.Benchmarks.Core.Utils;
using XReports.Benchmarks.NewVersion.XReportsProperties;
using XReports.Enums;
using XReports.Interfaces;
using XReports.Models;
using XReports.Properties;
using XReports.PropertyHandlers.Excel;
using XReports.PropertyHandlers.Html;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Writers;
using Source = XReports.Benchmarks.Core.ReportStructure.Models.Properties;
using SourceEnums = XReports.Benchmarks.Core.ReportStructure.Enums;

namespace XReports.Benchmarks.NewVersion;

public class ReportService : IReportService, IDisposable
{
    private readonly IEnumerable<Person> data;
    private readonly DataTable dataTable;
    private readonly IReportConverter<HtmlReportCell> htmlConverter;
    private readonly IEpplusWriter excelWriter;
    private readonly IReportConverter<ExcelReportCell> excelConverter;
    private readonly IHtmlStringWriter htmlStringWriter;
    private readonly IHtmlStreamWriter htmlStreamWriter;

    private readonly Dictionary<Source.ReportCellsSourceProperty, ReportCellProperty> mappedProperties = new();
    private readonly ReportStructureProvider reportStructureProvider = new();

    private IDataReader dataReader;

    public ReportService(IEnumerable<Person> data, DataTable dataTable)
    {
        this.data = data;
        this.dataTable = dataTable;

        this.htmlStreamWriter = new HtmlStreamWriter(new HtmlStreamCellWriter());
        this.excelWriter = new EpplusWriter();
        this.htmlStringWriter = new HtmlStringWriter(new HtmlStringCellWriter());
        this.htmlConverter = new ReportConverter<HtmlReportCell>(new IPropertyHandler<HtmlReportCell>[]
        {
            new AlignmentPropertyHtmlHandler(),
            new BoldPropertyHtmlHandler(),
            new ColorPropertyHtmlHandler(),
            new DecimalPrecisionPropertyHtmlHandler(),
            new PercentFormatPropertyHtmlHandler(),
            new DateTimeFormatPropertyHtmlHandler(),
            new CustomFormatPropertyHtmlHandler()
        });
        this.excelConverter = new ReportConverter<ExcelReportCell>(new IPropertyHandler<ExcelReportCell>[]
        {
            new AlignmentPropertyExcelHandler(),
            new BoldPropertyExcelHandler(),
            new ColorPropertyExcelHandler(),
            new DecimalPrecisionPropertyExcelHandler(),
            new PercentFormatPropertyExcelHandler(),
            new DateTimeFormatPropertyExcelHandler(),
            new CustomFormatPropertyExcelHandler()
        });
    }

    public Task VerticalFromEntitiesHtmlEnumAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromEntities();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);

        foreach (ReportTableProperty _ in htmlReportTable.Properties)
        {
        }

        foreach (IEnumerable<HtmlReportCell> row in htmlReportTable.HeaderRows)
        {
            foreach (HtmlReportCell _ in row)
            {
            }
        }

        foreach (IEnumerable<HtmlReportCell> row in htmlReportTable.Rows)
        {
            foreach (HtmlReportCell _ in row)
            {
            }
        }

        return Task.CompletedTask;
    }

    public Task VerticalFromEntitiesExcelEnumAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromEntities();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        foreach (ReportTableProperty _ in excelReportTable.Properties)
        {
        }

        foreach (IEnumerable<ExcelReportCell> row in excelReportTable.HeaderRows)
        {
            foreach (ExcelReportCell _ in row)
            {
            }
        }

        foreach (IEnumerable<ExcelReportCell> row in excelReportTable.Rows)
        {
            foreach (ExcelReportCell _ in row)
            {
            }
        }

        return Task.CompletedTask;
    }

    public Task<string> VerticalFromEntitiesHtmlToStringAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromEntities();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);

        return Task.FromResult(this.htmlStringWriter.WriteToString(htmlReportTable));
    }

    public async Task VerticalFromEntitiesHtmlToFileAsync(string fileName)
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromEntities();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);
        await using Stream fileStream = File.Create(fileName);

        await this.htmlStreamWriter.WriteAsync(htmlReportTable, fileStream);
    }

    public Task<Stream> VerticalFromEntitiesExcelToStreamAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromEntities();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        return Task.FromResult(this.excelWriter.WriteToStream(excelReportTable));
    }

    public Task VerticalFromEntitiesExcelToFileAsync(string fileName)
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromEntities();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        this.excelWriter.WriteToFile(excelReportTable, fileName);

        return Task.CompletedTask;
    }

    public Task VerticalFromDataReaderHtmlEnumAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromDataReader();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);

        foreach (ReportTableProperty _ in htmlReportTable.Properties)
        {
        }

        foreach (IEnumerable<HtmlReportCell> row in htmlReportTable.HeaderRows)
        {
            foreach (HtmlReportCell _ in row)
            {
            }
        }

        foreach (IEnumerable<HtmlReportCell> row in htmlReportTable.Rows)
        {
            foreach (HtmlReportCell _ in row)
            {
            }
        }

        return Task.CompletedTask;
    }

    public Task VerticalFromDataReaderExcelEnumAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromDataReader();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        foreach (ReportTableProperty _ in excelReportTable.Properties)
        {
        }

        foreach (IEnumerable<ExcelReportCell> row in excelReportTable.HeaderRows)
        {
            foreach (ExcelReportCell _ in row)
            {
            }
        }

        foreach (IEnumerable<ExcelReportCell> row in excelReportTable.Rows)
        {
            foreach (ExcelReportCell _ in row)
            {
            }
        }

        return Task.CompletedTask;
    }

    public Task<string> VerticalFromDataReaderHtmlToStringAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromDataReader();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);

        return Task.FromResult(this.htmlStringWriter.WriteToString(htmlReportTable));
    }

    public async Task VerticalFromDataReaderHtmlToFileAsync(string fileName)
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromDataReader();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);
        await using Stream fileStream = File.Create(fileName);

        await this.htmlStreamWriter.WriteAsync(htmlReportTable, fileStream);
    }

    public Task<Stream> VerticalFromDataReaderExcelToStreamAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromDataReader();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        return Task.FromResult(this.excelWriter.WriteToStream(excelReportTable));
    }

    public Task VerticalFromDataReaderExcelToFileAsync(string fileName)
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromDataReader();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        this.excelWriter.WriteToFile(excelReportTable, fileName);

        return Task.CompletedTask;
    }

    public Task HorizontalHtmlEnumAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildHorizontalReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);

        foreach (ReportTableProperty _ in htmlReportTable.Properties)
        {
        }

        foreach (IEnumerable<HtmlReportCell> row in htmlReportTable.HeaderRows)
        {
            foreach (HtmlReportCell _ in row)
            {
            }
        }

        foreach (IEnumerable<HtmlReportCell> row in htmlReportTable.Rows)
        {
            foreach (HtmlReportCell _ in row)
            {
            }
        }

        return Task.CompletedTask;
    }

    public Task HorizontalExcelEnumAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildHorizontalReport();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        foreach (ReportTableProperty _ in excelReportTable.Properties)
        {
        }

        foreach (IEnumerable<ExcelReportCell> row in excelReportTable.HeaderRows)
        {
            foreach (ExcelReportCell _ in row)
            {
            }
        }

        foreach (IEnumerable<ExcelReportCell> row in excelReportTable.Rows)
        {
            foreach (ExcelReportCell _ in row)
            {
            }
        }

        return Task.CompletedTask;
    }

    public Task<string> HorizontalHtmlToStringAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildHorizontalReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);

        return Task.FromResult(this.htmlStringWriter.WriteToString(htmlReportTable));
    }

    public async Task HorizontalHtmlToFileAsync(string fileName)
    {
        IReportTable<ReportCell> reportTable = this.BuildHorizontalReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);
        await using Stream fileStream = File.Create(fileName);

        await this.htmlStreamWriter.WriteAsync(htmlReportTable, fileStream);
    }

    private IReportTable<ReportCell> BuildVerticalReportFromEntities()
    {
        ReportSchemaBuilder<Person> reportBuilder = new();

        foreach (ReportCellsSource<Person> source in this.reportStructureProvider.GetEntitiesCellsSources())
        {
            Type valueType = source.ValueType;
            IReportCellsProvider<Person> column = (IReportCellsProvider<Person>)Activator.CreateInstance(
                typeof(ComputedValueReportCellsProvider<,>)
                    .MakeGenericType(typeof(Person), valueType),
                TypeUtils.InvokeGenericMethod(source, nameof(source.ConvertValueSelector), valueType)
            );

            reportBuilder.AddColumn(source.Title, column).AddProperties(this.MapProperties(source.Properties));
        }

        reportBuilder.AddGlobalProperties(this.MapProperties(this.reportStructureProvider.GetGlobalProperties()));

        IReportTable<ReportCell> reportTable = reportBuilder.BuildVerticalSchema().BuildReportTable(this.data);

        return reportTable;
    }

    private IReportTable<ReportCell> BuildVerticalReportFromDataReader()
    {
        ReportSchemaBuilder<IDataReader> reportBuilder = new();

        foreach (ReportCellsSource<IDataReader> source in this.reportStructureProvider.GetDataReaderCellsSources())
        {
            Type valueType = source.ValueType;
            IReportCellsProvider<IDataReader> column = (IReportCellsProvider<IDataReader>)Activator.CreateInstance(
                typeof(ComputedValueReportCellsProvider<,>)
                    .MakeGenericType(typeof(IDataReader), valueType),
                TypeUtils.InvokeGenericMethod(source, nameof(source.ConvertValueSelector), valueType)
            );

            reportBuilder.AddColumn(source.Title, column).AddProperties(this.MapProperties(source.Properties));
        }

        reportBuilder.AddGlobalProperties(this.MapProperties(this.reportStructureProvider.GetGlobalProperties()));

        this.dataReader = new DataTableReader(this.dataTable);
        IReportTable<ReportCell> reportTable = reportBuilder.BuildVerticalSchema().BuildReportTable(this.dataReader);

        return reportTable;
    }

    private IReportTable<ReportCell> BuildHorizontalReport()
    {
        ReportSchemaBuilder<Person> reportBuilder = new();

        foreach (ReportCellsSource<Person> source in this.reportStructureProvider.GetEntitiesCellsSources())
        {
            Type valueType = source.ValueType;
            IReportCellsProvider<Person> row = (IReportCellsProvider<Person>)Activator.CreateInstance(
                typeof(ComputedValueReportCellsProvider<,>)
                    .MakeGenericType(typeof(Person), valueType),
                TypeUtils.InvokeGenericMethod(source, nameof(source.ConvertValueSelector), valueType)
            );

            reportBuilder.AddColumn(source.Title, row).AddProperties(this.MapProperties(source.Properties));
        }

        reportBuilder.AddGlobalProperties(this.MapProperties(this.reportStructureProvider.GetGlobalProperties()));

        IReportTable<ReportCell> reportTable = reportBuilder.BuildHorizontalSchema(0).BuildReportTable(this.data);

        return reportTable;
    }

    private IReportTable<HtmlReportCell> ConvertToHtml(IReportTable<ReportCell> reportTable)
    {
        return this.htmlConverter.Convert(reportTable);
    }

    private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
    {
        return this.excelConverter.Convert(reportTable);
    }

    private ReportCellProperty[] MapProperties(IEnumerable<Source.ReportCellsSourceProperty> sourceProperties)
    {
        return sourceProperties.Select(this.MapProperty).ToArray();
    }

    private ReportCellProperty MapProperty(Source.ReportCellsSourceProperty sourceProperty)
    {
        if (!this.mappedProperties.ContainsKey(sourceProperty))
        {
            this.mappedProperties[sourceProperty] = sourceProperty switch
            {
                Source.AlignmentProperty alignmentProperty => new AlignmentProperty(
                    alignmentProperty.Alignment switch
                    {
                        SourceEnums.Alignment.Left => Alignment.Left,
                        SourceEnums.Alignment.Right => Alignment.Right,
                        SourceEnums.Alignment.Center => Alignment.Center,
                        _ => throw new ArgumentOutOfRangeException(nameof(sourceProperty), $"Invalid alignment {alignmentProperty.Alignment}"),
                    }),
                Source.BoldProperty => new BoldProperty(),
                Source.ColorProperty colorProperty => new ColorProperty(colorProperty.ForegroundColor, colorProperty.BackgroundColor),
                Source.CustomFormatProperty => new CustomFormatProperty(),
                Source.DecimalPrecisionProperty decimalPrecisionProperty => new DecimalPrecisionProperty(decimalPrecisionProperty.Precision),
                Source.DateTimeFormatProperty dateTimeFormatProperty => new DateTimeFormatProperty(dateTimeFormatProperty.Format),
                Source.SameColumnFormatProperty => new SameColumnFormatProperty(),
                _ => throw new ArgumentOutOfRangeException(nameof(sourceProperty)),
            };
        }

        return this.mappedProperties[sourceProperty];
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.dataTable?.Dispose();
            this.dataReader?.Dispose();
        }
    }
}
