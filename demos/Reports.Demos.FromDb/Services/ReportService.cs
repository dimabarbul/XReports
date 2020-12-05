using System.Collections.Generic;
using Reports.Core.Interfaces;
using Reports.Core.Models;
using Reports.Extensions.AttributeBasedBuilder;

namespace Reports.Demos.FromDb.Services
{
    public class ReportService
    {
        private readonly AttributeBasedBuilder builderHelper;

        public ReportService(AttributeBasedBuilder builderHelper)
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
