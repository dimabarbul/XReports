using System.Collections.Generic;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.Demos.FromDb.Services
{
    public class ReportService
    {
        private readonly IAttributeBasedBuilder builderHelper;

        public ReportService(IAttributeBasedBuilder builderHelper)
        {
            this.builderHelper = builderHelper;
        }

        public IReportTable<ReportCell> GetReport<TEntity>(IEnumerable<TEntity> entities)
        {
            IReportSchema<TEntity> schema = this.builderHelper.BuildSchema<TEntity>();

            return schema.BuildReportTable(entities);
        }
    }
}
