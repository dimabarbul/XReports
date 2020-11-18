using System.Threading.Tasks;
using Reports.Builders;
using Reports.Demos.FromDb.ReportModels;
using Reports.Demos.FromDb.Reports.Properties;
using Reports.Enums;
using Reports.Extensions.Builders.BuilderHelpers;
using Reports.Extensions.Properties;
using Reports.Interfaces;
using Reports.Models;

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
            VerticalReportBuilder<UsersListReport> builder = new VerticalReportBuilder<UsersListReport>();
            EntityAttributeBuilderHelper helper = new EntityAttributeBuilderHelper();
            helper.BuildVerticalReport(builder);

            return builder.Build(await this.userService.GetActiveUsersAsync(limit));
        }

        public async Task<IReportTable<ReportCell>> GetProductsAsync(int? limit = null)
        {
            VerticalReportBuilder<ProductListReport> builder = new VerticalReportBuilder<ProductListReport>();
            EntityAttributeBuilderHelper helper = new EntityAttributeBuilderHelper();
            helper.BuildVerticalReport(builder);

            AlignmentProperty centerAlignmentProperty = new AlignmentProperty(AlignmentType.Center);
            builder.GetColumn("Price")
                .AddProperty(new DecimalFormatProperty(2))
                .AddProperty(centerAlignmentProperty);

            builder.GetColumn("Active")
                .AddProperty(new YesNoProperty());

            builder.AddHeaderProperty("ID", centerAlignmentProperty);
            builder.AddHeaderProperty("Title", centerAlignmentProperty);
            builder.AddHeaderProperty("Description", centerAlignmentProperty);
            builder.AddHeaderProperty("Active", centerAlignmentProperty);

            return builder.Build(await this.productService.GetAllAsync(limit));
        }
    }
}
