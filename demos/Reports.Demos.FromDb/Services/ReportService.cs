using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.Builders;
using Reports.Demos.FromDb.ReportModels;
using Reports.Enums;
using Reports.Extensions;
using Reports.Extensions.Builders.BuilderHelpers;
using Reports.Extensions.Properties;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.Demos.FromDb.Services
{
    public class ReportService
    {
        private readonly UserService userService;

        public ReportService(UserService userService)
        {
            this.userService = userService;
        }

        public async Task<IReportTable<ReportCell>> GetUsersAsync(int? limit = null)
        {
            VerticalReportBuilder<UsersListReport> builder = new VerticalReportBuilder<UsersListReport>();
            EntityAttributeBuilderHelper helper = new EntityAttributeBuilderHelper();
            helper.BuildVerticalReport(builder);

            return builder.Build(await this.userService.GetActiveUsersAsync(limit));
        }
    }
}
