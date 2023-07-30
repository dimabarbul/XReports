using System.Data;
using XReports.Benchmarks.Models;
using XReports.Benchmarks.ReportStructure;
using XReports.Benchmarks.ReportStructure.Models;
using XReports.Benchmarks.Utils;
using XReports.Converter;
using XReports.DataReader;
using XReports.DependencyInjection;
using XReports.Excel;
using XReports.Excel.PropertyHandlers;
using XReports.Excel.Writers;
using XReports.Html;
using XReports.Html.PropertyHandlers;
using XReports.Html.Writers;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.ReportCellProviders;
using XReports.Table;

namespace XReports.Benchmarks;

public sealed class ReportService : IDisposable
{
    private readonly IEnumerable<Person> data;
    private readonly DataTable dataTable;
    private readonly IReportConverter<HtmlReportCell> htmlConverter;
    private readonly IEpplusWriter excelWriter;
    private readonly IReportConverter<ExcelReportCell> excelConverter;
    private readonly IHtmlStringWriter htmlStringWriter;
    private readonly IHtmlStreamWriter htmlStreamWriter;

    private IDataReader dataReader;
    private bool disposed;

    public ReportService(IEnumerable<Person> data, DataTable dataTable)
    {
        this.data = data;
        this.dataTable = dataTable;

        this.htmlStreamWriter = new HtmlStreamWriter(new HtmlStreamCellWriter());
        this.excelWriter = new EpplusWriter();
        this.htmlStringWriter = new HtmlStringWriter(new HtmlStringCellWriter());
        this.htmlConverter = new ReportConverter<HtmlReportCell>(
            new TypesCollection<IPropertyHandler<HtmlReportCell>>()
                .AddFromAssembly(typeof(AlignmentPropertyHtmlHandler).Assembly)
                .Select(t => (IPropertyHandler<HtmlReportCell>)Activator.CreateInstance(t)));
        this.excelConverter = new ReportConverter<ExcelReportCell>(
            new TypesCollection<IPropertyHandler<ExcelReportCell>>()
                .AddFromAssembly(typeof(AlignmentPropertyExcelHandler).Assembly)
                .Select(t => (IPropertyHandler<ExcelReportCell>)Activator.CreateInstance(t)));
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

        return Task.FromResult(this.htmlStringWriter.Write(htmlReportTable));
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

        return Task.FromResult(this.htmlStringWriter.Write(htmlReportTable));
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

        return Task.FromResult(this.htmlStringWriter.Write(htmlReportTable));
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

        foreach (ReportCellsSource<Person> source in ReportStructureProvider.GetEntitiesCellsSources())
        {
            Type valueType = source.ValueType;
            IReportCellProvider<Person> column = (IReportCellProvider<Person>)Activator.CreateInstance(
                typeof(ComputedValueReportCellProvider<,>)
                    .MakeGenericType(typeof(Person), valueType),
                TypeUtils.InvokeGenericMethod(source, nameof(source.ConvertValueSelector), valueType)
            );

            reportBuilder.AddColumn(source.Title, column).AddProperties(source.Properties);
        }

        reportBuilder.AddGlobalProperties(ReportStructureProvider.GetGlobalProperties());

        IReportTable<ReportCell> reportTable = reportBuilder.BuildVerticalSchema().BuildReportTable(this.data);

        return reportTable;
    }

    private IReportTable<ReportCell> BuildVerticalReportFromDataReader()
    {
        ReportSchemaBuilder<IDataReader> reportBuilder = new();

        foreach (ReportCellsSource<IDataReader> source in ReportStructureProvider.GetDataReaderCellsSources())
        {
            Type valueType = source.ValueType;
            IReportCellProvider<IDataReader> column = (IReportCellProvider<IDataReader>)Activator.CreateInstance(
                typeof(ComputedValueReportCellProvider<,>)
                    .MakeGenericType(typeof(IDataReader), valueType),
                TypeUtils.InvokeGenericMethod(source, nameof(source.ConvertValueSelector), valueType)
            );

            reportBuilder.AddColumn(source.Title, column).AddProperties(source.Properties);
        }

        reportBuilder.AddGlobalProperties(ReportStructureProvider.GetGlobalProperties());

        this.dataReader = new DataTableReader(this.dataTable);
        IReportTable<ReportCell> reportTable = reportBuilder.BuildVerticalSchema().BuildReportTable(this.dataReader.AsEnumerable());

        return reportTable;
    }

    private IReportTable<ReportCell> BuildHorizontalReport()
    {
        ReportSchemaBuilder<Person> reportBuilder = new();

        foreach (ReportCellsSource<Person> source in ReportStructureProvider.GetEntitiesCellsSources())
        {
            Type valueType = source.ValueType;
            IReportCellProvider<Person> row = (IReportCellProvider<Person>)Activator.CreateInstance(
                typeof(ComputedValueReportCellProvider<,>)
                    .MakeGenericType(typeof(Person), valueType),
                TypeUtils.InvokeGenericMethod(source, nameof(source.ConvertValueSelector), valueType)
            );

            reportBuilder.AddColumn(source.Title, row).AddProperties(source.Properties);
        }

        reportBuilder.AddGlobalProperties(ReportStructureProvider.GetGlobalProperties());

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

    public void Dispose()
    {
        if (this.disposed)
        {
            return;
        }

        this.dataTable?.Dispose();
        this.dataReader?.Dispose();

        this.disposed = true;
    }
}
