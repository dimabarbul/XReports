using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Reports.Core;
using Reports.Core.Extensions;
using Reports.Core.Interfaces;
using Reports.Core.Models;
using Reports.Core.SchemaBuilders;
using Reports.Demos.MVC.Reports;
using Reports.Html.StringWriter;

namespace Reports.Demos.MVC.Controllers.StringWriterExtensions
{
    public class CustomTableClassController : Controller
    {
        private const int RecordsCount = 10;

        public async Task<IActionResult> Index()
        {
            IReportTable<ReportCell> reportTable = this.BuildReport();
            IReportTable<HtmlReportCell> htmlReport = this.Convert(reportTable);
            StringWriter bootstrapStringWriter = new BootstrapStringWriter(new StringCellWriter());
            StringWriter stringWriter = new StringWriter(new StringCellWriter());

            ViewModel vm = new ViewModel()
            {
                RegularTableHtml = await stringWriter.WriteToStringAsync(htmlReport),
                TableHtml = await bootstrapStringWriter.WriteToStringAsync(htmlReport),
            };
            return this.View(vm);
        }

        private IReportTable<ReportCell> BuildReport()
        {
            VerticalReportSchemaBuilder<Entity> builder = new VerticalReportSchemaBuilder<Entity>();
            builder.AddColumn("First name", e => e.FirstName);
            builder.AddColumn("Last name", e => e.LastName);
            builder.AddColumn("Email", e => e.Email);
            builder.AddColumn("Age", e => e.Age);

            return builder.BuildSchema().BuildReportTable(this.GetData());
        }

        private IEnumerable<Entity> GetData()
        {
            return new Faker<Entity>()
                .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                .RuleFor(e => e.LastName, f => f.Name.LastName())
                .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName, e.LastName))
                .RuleFor(e => e.Age, f => f.Random.Int(18, 57))
                .Generate(RecordsCount);
        }

        private IReportTable<HtmlReportCell> Convert(IReportTable<ReportCell> reportTable)
        {
            return new ReportConverter<HtmlReportCell>(Enumerable.Empty<IPropertyHandler<HtmlReportCell>>())
                .Convert(reportTable);
        }

        public class ViewModel
        {
            public string RegularTableHtml { get; set; }
            public string TableHtml { get; set; }
        }

        private class Entity
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
            public string Email { get; set; }
        }
    }
}
