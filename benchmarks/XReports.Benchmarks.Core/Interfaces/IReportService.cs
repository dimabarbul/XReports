namespace XReports.Benchmarks.Core.Interfaces;

public interface IReportService
{
    Task VerticalFromEntitiesHtmlEnumAsync();
    Task VerticalFromEntitiesExcelEnumAsync();
    Task<string> VerticalFromEntitiesHtmlToStringAsync();
    Task VerticalFromEntitiesHtmlToFileAsync(string fileName);
    Task VerticalFromEntitiesExcelToFileAsync(string fileName);
    Task<Stream> VerticalFromEntitiesExcelToStreamAsync();

    Task VerticalFromDataReaderHtmlEnumAsync();
    Task VerticalFromDataReaderExcelEnumAsync();
    Task<string> VerticalFromDataReaderHtmlToStringAsync();
    Task VerticalFromDataReaderHtmlToFileAsync(string fileName);
    Task VerticalFromDataReaderExcelToFileAsync(string fileName);
    Task<Stream> VerticalFromDataReaderExcelToStreamAsync();

    Task HorizontalHtmlEnumAsync();
    Task HorizontalExcelEnumAsync();
    Task<string> HorizontalHtmlToStringAsync();
    Task HorizontalHtmlToFileAsync(string fileName);
}
