namespace XReports.BenchmarksCore.Interfaces;

public interface IReportService
{
    Task EnumAsync();
    Task<string> ToStringAsync();
    Task ToHtmlFileAsync(string fileName);
    Task ToExcelFileAsync(string fileName);
    Task<Stream> ToExcelStreamAsync();
}
