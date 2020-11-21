using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Reports.Extensions;
using Reports.Html.Models;
using Reports.Html.StringWriter;
using Reports.Interfaces;
using Reports.SchemaBuilders;

namespace Reports.Demos.FromDb.Controllers
{
    public class GenericController : Controller
    {
        private ReportConverter<HtmlReportCell> htmlConverter;
        private readonly StringWriter stringWriter;

        public GenericController(ReportConverter<HtmlReportCell> htmlConverter, StringWriter stringWriter)
        {
            this.htmlConverter = htmlConverter;
            this.stringWriter = stringWriter;
        }

        public async Task<IActionResult> Index()
        {
            await using SqliteConnection connection = new SqliteConnection("Data Source=Test.db");
            IEnumerable<dynamic> products = await connection.QueryAsync("SELECT * FROM Products");
            VerticalReportSchemaBuilder<dynamic> builder = new VerticalReportSchemaBuilder<dynamic>();
            builder.AddColumn("ID", x => x.Id);
            builder.AddColumn("Title", x => x.Title);
            IReportTable<HtmlReportCell> reportTable = this.htmlConverter.Convert(builder.BuildSchema().BuildReportTable(products));
            string tableHtml = await this.stringWriter.WriteToStringAsync(reportTable);

            return View(new IndexViewModel()
            {
                ReportHtml = tableHtml,
            });
        }

        public class IndexViewModel
        {
            public string ReportHtml { get; set; }
        }
    }
}
