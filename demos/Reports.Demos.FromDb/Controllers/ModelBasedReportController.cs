using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reports.Demos.FromDb.Services;
using Reports.Demos.FromDb.ViewModels.ModelBasedReport;
using Reports.Excel.EpplusWriter;
using Reports.Excel.Models;
using Reports.Html.Models;
using Reports.Interfaces;
using Reports.Models;
using StringWriter = Reports.Html.StringWriter.StringWriter;

namespace Reports.Demos.FromDb.Controllers
{
    public class ModelBasedReportController : Controller
    {
        private readonly ReportService reportService;
        private readonly ReportConverter<HtmlReportCell> htmlConverter;
        private readonly ReportConverter<ExcelReportCell> excelConverter;
        private readonly StringWriter stringWriter;
        private readonly EpplusWriter excelWriter;

        public ModelBasedReportController(ReportService reportService, ReportConverter<HtmlReportCell> htmlConverter, StringWriter stringWriter, ReportConverter<ExcelReportCell> excelConverter, EpplusWriter excelWriter)
        {
            this.reportService = reportService;
            this.htmlConverter = htmlConverter;
            this.stringWriter = stringWriter;
            this.excelConverter = excelConverter;
            this.excelWriter = excelWriter;
        }

        public async Task<IActionResult> Index()
        {
            IReportTable<ReportCell> report = await this.reportService.GetUsersAsync(50);
            IReportTable<HtmlReportCell> htmlTable = this.htmlConverter.Convert(report);
            string reportHtml = await this.stringWriter.WriteToStringAsync(htmlTable);

            return this.View(new IndexViewModel()
            {
                ReportHtml = reportHtml,
            });
        }

        public async Task<IActionResult> DownloadExcel()
        {
            IReportTable<ReportCell> report = await this.reportService.GetUsersAsync();
            IReportTable<ExcelReportCell> excelTable = this.excelConverter.Convert(report);

            Stream excelStream = this.excelWriter.WriteToStream(excelTable);

            return File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Excel.xlsx");
        }
    }
}
