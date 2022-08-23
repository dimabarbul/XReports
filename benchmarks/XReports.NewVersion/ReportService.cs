using System.Data;
using System.Reflection;
using XReports.BenchmarksCore.Interfaces;
using XReports.BenchmarksCore.Models;
using XReports.BenchmarksCore.ReportStructure;
using XReports.BenchmarksCore.ReportStructure.Models;
using XReports.BenchmarksCore.Utils;
using XReports.Enums;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.NewVersion.XReportsProperties;
using XReports.Properties;
using XReports.PropertyHandlers.Excel;
using XReports.PropertyHandlers.Html;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Writers;
using Source = XReports.BenchmarksCore.ReportStructure.Models.Properties;
using SourceEnums = XReports.BenchmarksCore.ReportStructure.Enums;

namespace XReports.NewVersion;

public class ReportService : IReportService
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
            foreach (HtmlReportCell cell in row)
            {
            }
        }

        foreach (IEnumerable<HtmlReportCell> row in htmlReportTable.Rows)
        {
            foreach (HtmlReportCell cell in row)
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
            foreach (ExcelReportCell cell in row)
            {
            }
        }

        foreach (IEnumerable<ExcelReportCell> row in excelReportTable.Rows)
        {
            foreach (ExcelReportCell cell in row)
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
            foreach (HtmlReportCell cell in row)
            {
            }
        }

        foreach (IEnumerable<HtmlReportCell> row in htmlReportTable.Rows)
        {
            foreach (HtmlReportCell cell in row)
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
            foreach (ExcelReportCell cell in row)
            {
            }
        }

        foreach (IEnumerable<ExcelReportCell> row in excelReportTable.Rows)
        {
            foreach (ExcelReportCell cell in row)
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
            foreach (HtmlReportCell cell in row)
            {
            }
        }

        foreach (IEnumerable<HtmlReportCell> row in htmlReportTable.Rows)
        {
            foreach (HtmlReportCell cell in row)
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
            foreach (ExcelReportCell cell in row)
            {
            }
        }

        foreach (IEnumerable<ExcelReportCell> row in excelReportTable.Rows)
        {
            foreach (ExcelReportCell cell in row)
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
        VerticalReportSchemaBuilder<Person> reportBuilder = new();

        foreach (BaseReportCellsSourceFromEntities source in this.reportStructureProvider.GetEntitiesCellsSources())
        {
            Type valueType = source.GetValueType();
            IReportCellsProvider<Person> x = (IReportCellsProvider<Person>)Activator.CreateInstance(
                typeof(ComputedValueReportCellsProvider<,>)
                    .MakeGenericType(typeof(Person), valueType),
                source.Title,
                TypeUtils.InvokeGenericMethod(source, nameof(source.GetValueSelector), valueType)
            );

            reportBuilder.AddColumn(x)
                .AddProperties(this.MapProperties(source.Properties));
        }

        reportBuilder.AddGlobalProperties(this.MapProperties(this.reportStructureProvider.GetGlobalProperties()));

        IReportTable<ReportCell> reportTable = reportBuilder.BuildSchema().BuildReportTable(this.data);

        return reportTable;
    }

    private IReportTable<ReportCell> BuildVerticalReportFromDataReader()
    {
        VerticalReportSchemaBuilder<IDataReader> reportBuilder = new();

        foreach (ReportCellsSourceFromDataReader source in this.reportStructureProvider.GetDataReaderCellsSources())
        {
            reportBuilder.AddColumn(source.Title, source.ValueSelector)
                .AddProperties(this.MapProperties(source.Properties));
        }

        reportBuilder.AddGlobalProperties(this.MapProperties(this.reportStructureProvider.GetGlobalProperties()));

        IReportTable<ReportCell> reportTable = reportBuilder.BuildSchema().BuildReportTable(new DataTableReader(this.dataTable));

        return reportTable;
    }

    private IReportTable<ReportCell> BuildHorizontalReport()
    {
        HorizontalReportSchemaBuilder<Person> reportBuilder = new();

        foreach (BaseReportCellsSourceFromEntities source in this.reportStructureProvider.GetEntitiesCellsSources())
        {
            Type valueType = source.GetValueType();
            MethodInfo getValueSelectorMethod = source.GetType().GetMethod(nameof(source.GetValueSelector))
                .MakeGenericMethod(valueType);
            IReportCellsProvider<Person> x = (IReportCellsProvider<Person>)Activator.CreateInstance(
                typeof(ComputedValueReportCellsProvider<,>)
                    .MakeGenericType(typeof(Person), valueType),
                source.Title,
                getValueSelectorMethod.Invoke(source, Array.Empty<object?>())
            );
            reportBuilder.AddRow(x)
                .AddProperties(this.MapProperties(source.Properties));
        }

        reportBuilder.AddGlobalProperties(this.MapProperties(this.reportStructureProvider.GetGlobalProperties()));

        IReportTable<ReportCell> reportTable = reportBuilder.BuildSchema().BuildReportTable(this.data);

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
}
