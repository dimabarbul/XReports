namespace XReports.BenchmarksCore.Interfaces;

public interface IReportService
{
    Task EnumHtmlAsync();
    Task EnumExcelAsync();
    Task<string> ToHtmlStringAsync();
    Task ToHtmlFileAsync(string fileName);
    Task ToExcelFileAsync(string fileName);
    Task<Stream> ToExcelStreamAsync();
}
