using System.Data;
using System.IO;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XReports.Converter;
using XReports.DataReader;
using XReports.Demos.MVC.Data;
using XReports.Demos.MVC.Filters;
using XReports.Demos.MVC.Models.Shared;
using XReports.Demos.MVC.XReports;
using XReports.Excel;
using XReports.Excel.Writers;
using XReports.Html;
using XReports.Html.Writers;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;

namespace XReports.Demos.MVC.Controllers.DataSources
{
    [DatabaseDependent]
    public class DataReaderController : Controller
    {
        private readonly AppDbContext appDbContext;

        public DataReaderController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            using IDataReader dataReader = await this.appDbContext.Database.GetDbConnection()
                .ExecuteReaderAsync("SELECT Id, FirstName, LastName, Email FROM Users LIMIT 10");
            IReportTable<ReportCell> reportTable = this.BuildReport(dataReader);
            IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);
            string tableHtml = this.WriteHtmlReportToString(htmlReportTable);

            return this.View(new ReportViewModel()
            {
                ReportTableHtml = tableHtml,
            });
        }

        public async Task<IActionResult> Download()
        {
            using IDataReader dataReader = await this.appDbContext.Database.GetDbConnection()
                .ExecuteReaderAsync("SELECT Id, FirstName, LastName, Email FROM Users");
            IReportTable<ReportCell> reportTable = this.BuildReport(dataReader);
            IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

            Stream excelStream = this.WriteExcelReportToStream(excelReportTable);
            return this.File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DataReader.xlsx");
        }

        private IReportTable<ReportCell> BuildReport(IDataReader dataReader)
        {
            ReportSchemaBuilder<IDataReader> builder = new ReportSchemaBuilder<IDataReader>();
            builder.AddColumn("ID", x => x.GetInt32(0));
            builder.AddColumn("First Name", x => x.GetString(1));
            builder.AddColumn("Last Name", x => x.GetString(2));
            builder.AddColumn("Email", x => x.GetString(3));

            IReportSchema<IDataReader> schema = builder.BuildVerticalSchema();
            IReportTable<ReportCell> reportTable = schema.BuildReportTable(dataReader.AsEnumerable());
            return reportTable;
        }

        private IReportTable<HtmlReportCell> ConvertToHtml(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<HtmlReportCell> htmlConverter = new ReportConverter<HtmlReportCell>();

            return htmlConverter.Convert(reportTable);
        }

        private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<ExcelReportCell> excelConverter = new ReportConverter<ExcelReportCell>();

            return excelConverter.Convert(reportTable);
        }

        private string WriteHtmlReportToString(IReportTable<HtmlReportCell> htmlReportTable)
        {
            return new BootstrapHtmlStringWriter(new HtmlStringCellWriter()).WriteToString(htmlReportTable);
        }

        private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
        {
            return new EpplusWriter().WriteToStream(reportTable);
        }
    }
}
