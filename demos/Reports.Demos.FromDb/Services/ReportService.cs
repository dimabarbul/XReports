using System.Threading.Tasks;
using Reports.Demos.FromDb.ReportModels;
using Reports.Demos.FromDb.Reports.Properties;
using Reports.Enums;
using Reports.Extensions.Builders.BuilderHelpers;
using Reports.Extensions.Properties;
using Reports.Interfaces;
using Reports.Models;
using Reports.SchemaBuilders;

namespace Reports.Demos.FromDb.Services
{
    public class ReportService
    {
        private readonly UserService userService;
        private readonly ProductService productService;

        public ReportService(UserService userService, ProductService productService)
        {
            this.userService = userService;
            this.productService = productService;
        }

        public async Task<IReportTable<ReportCell>> GetUsersAsync(int? limit = null)
        {
            EntityAttributeBuilderHelper helper = new EntityAttributeBuilderHelper();
            VerticalReportSchemaBuilder<UsersListReport> builder = helper.BuildVerticalReport<UsersListReport>();

            return builder.BuildSchema().BuildReportTable(await this.userService.GetActiveUsersAsync(limit));
        }

        public async Task<IReportTable<ReportCell>> GetProductsAsync(int? limit = null)
        {
            EntityAttributeBuilderHelper helper = new EntityAttributeBuilderHelper();
            VerticalReportSchemaBuilder<ProductListReport> builder = helper.BuildVerticalReport<ProductListReport>();

            AlignmentProperty centerAlignmentProperty = new AlignmentProperty(AlignmentType.Center);
            builder.ForColumn("Price").AddProperties(new DecimalFormatProperty(2), centerAlignmentProperty);
            builder.ForColumn("Active").AddProperties(new YesNoProperty());
            builder.ForColumn("ID").AddHeaderProperties(centerAlignmentProperty);
            builder.ForColumn("Title").AddHeaderProperties(centerAlignmentProperty);
            builder.ForColumn("Description").AddHeaderProperties(centerAlignmentProperty);
            builder.ForColumn("Active").AddHeaderProperties(centerAlignmentProperty);

            return builder.BuildSchema().BuildReportTable(await this.productService.GetAllAsync(limit));
        }

        public async Task<IReportTable<ReportCell>> GetOrdersDetailsAsync(int? limit = null)
        {
            EntityAttributeBuilderHelper helper = new EntityAttributeBuilderHelper();
            VerticalReportSchemaBuilder<OrdersDetailsReport> builder = helper.BuildVerticalReport<OrdersDetailsReport>();

            builder.ForColumn("Ordered On")
                .AddProperties(new DateTimeFormatProperty("dd MMM yyyy"));

            return builder.BuildSchema().BuildReportTable(await this.productService.GetOrdersDetailsAsync(limit));
        }
    }
}
