using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;

namespace XReports.Demos.FromDb.Controllers
{
    public class GenericController : Controller
    {
        private readonly IReportConverter<HtmlReportCell> htmlConverter;
        private readonly IStringWriter stringWriter;

        public GenericController(IReportConverter<HtmlReportCell> htmlConverter, IStringWriter stringWriter)
        {
            this.htmlConverter = htmlConverter;
            this.stringWriter = stringWriter;
        }

        public async Task<IActionResult> Index()
        {
            await using SqliteConnection connection = new SqliteConnection("Data Source=Test.db");
            IDataReader dataReader = await connection.ExecuteReaderAsync("SELECT Id, Title FROM Products", CommandBehavior.CloseConnection);
            VerticalReportSchemaBuilder<IDataReader> builder = new VerticalReportSchemaBuilder<IDataReader>();
            builder.AddColumn("ID", x => x.GetInt32(0));
            builder.AddColumn("Title", x => x.GetString(1));
            IReportTable<HtmlReportCell> reportTable = this.htmlConverter.Convert(builder.BuildSchema().BuildReportTable(dataReader));
            string tableHtml = this.stringWriter.WriteToString(reportTable);

            return this.View(new IndexViewModel()
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
