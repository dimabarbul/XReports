using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using XReports.Converter;
using XReports.Demos.FromDb.ViewModels;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilder;
using XReports.Table;

namespace XReports.Demos.FromDb.Controllers
{
    public class GenericController : Controller
    {
        private readonly IReportConverter<HtmlReportCell> htmlConverter;
        private readonly IHtmlStringWriter htmlStringWriter;

        public GenericController(IReportConverter<HtmlReportCell> htmlConverter, IHtmlStringWriter htmlStringWriter)
        {
            this.htmlConverter = htmlConverter;
            this.htmlStringWriter = htmlStringWriter;
        }

        public async Task<IActionResult> Index()
        {
            await using SqliteConnection connection = new SqliteConnection("Data Source=Test.db");
            IDataReader dataReader = await connection.ExecuteReaderAsync("SELECT Id, Title FROM Products", CommandBehavior.CloseConnection);
            ReportSchemaBuilder<IDataReader> builder = new ReportSchemaBuilder<IDataReader>();
            builder.AddColumn("ID", x => x.GetInt32(0));
            builder.AddColumn("Title", x => x.GetString(1));
            IReportTable<HtmlReportCell> reportTable = this.htmlConverter.Convert(builder.BuildVerticalSchema().BuildReportTable(dataReader));
            string tableHtml = this.htmlStringWriter.WriteToString(reportTable);

            return this.View(new ReportViewModel()
            {
                ReportHtml = tableHtml,
            });
        }
    }
}
