using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XReports.Demos.FromDb.Services;
using XReports.Demos.FromDb.ViewModels;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.Demos.FromDb.Controllers
{
    public class ModelBasedReportController : Controller
    {
        private const string ExcelMimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        private readonly ReportService reportService;
        private readonly ProductService productService;
        private readonly UserService userService;
        private readonly IReportConverter<HtmlReportCell> htmlConverter;
        private readonly IReportConverter<ExcelReportCell> excelConverter;
        private readonly IHtmlStringWriter htmlStringWriter;
        private readonly IEpplusWriter excelWriter;

        public ModelBasedReportController(ReportService reportService, IReportConverter<HtmlReportCell> htmlConverter, IHtmlStringWriter htmlStringWriter, IReportConverter<ExcelReportCell> excelConverter, IEpplusWriter excelWriter, ProductService productService, UserService userService)
        {
            this.reportService = reportService;
            this.htmlConverter = htmlConverter;
            this.htmlStringWriter = htmlStringWriter;
            this.excelConverter = excelConverter;
            this.excelWriter = excelWriter;
            this.productService = productService;
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            IReportTable<ReportCell> report = this.reportService.GetReport(await this.userService.GetActiveUsersAsync(50));
            IReportTable<HtmlReportCell> htmlTable = this.htmlConverter.Convert(report);
            string reportHtml = this.htmlStringWriter.WriteToString(htmlTable);

            return this.View(new ReportViewModel() { ReportHtml = reportHtml });
        }

        public async Task<IActionResult> DownloadUsers()
        {
            IReportTable<ReportCell> report = this.reportService.GetReport(await this.userService.GetActiveUsersAsync());
            IReportTable<ExcelReportCell> excelTable = this.excelConverter.Convert(report);

            Stream excelStream = this.excelWriter.WriteToStream(excelTable);

            return this.File(excelStream, ExcelMimeType, "Users.xlsx");
        }

        public async Task<IActionResult> Products()
        {
            IReportTable<ReportCell> report = this.reportService.GetReport(await this.productService.GetAllAsync(50));
            IReportTable<HtmlReportCell> htmlTable = this.htmlConverter.Convert(report);
            string reportHtml = this.htmlStringWriter.WriteToString(htmlTable);

            return this.View(new ReportViewModel() { ReportHtml = reportHtml });
        }

        public async Task<IActionResult> DownloadProducts()
        {
            IReportTable<ReportCell> report = this.reportService.GetReport(await this.productService.GetAllAsync());
            IReportTable<ExcelReportCell> excelTable = this.excelConverter.Convert(report);
            Stream stream = this.excelWriter.WriteToStream(excelTable);

            return new FileStreamResult(stream, ExcelMimeType) { FileDownloadName = "Products.xlsx" };
        }

        public async Task<IActionResult> OrdersDetails()
        {
            IReportTable<ReportCell> report = this.reportService.GetReport(await this.productService.GetOrdersDetailsAsync(50));
            IReportTable<HtmlReportCell> htmlTable = this.htmlConverter.Convert(report);
            string reportHtml = this.htmlStringWriter.WriteToString(htmlTable);

            return this.View(new ReportViewModel() { ReportHtml = reportHtml });
        }

        public async Task<IActionResult> DownloadOrdersDetails()
        {
            IReportTable<ReportCell> report = this.reportService.GetReport(await this.productService.GetOrdersDetailsAsync());
            IReportTable<ExcelReportCell> excelTable = this.excelConverter.Convert(report);
            Stream stream = this.excelWriter.WriteToStream(excelTable);

            return new FileStreamResult(stream, ExcelMimeType) { FileDownloadName = "OrdersDetails.xlsx" };
        }
    }
}
