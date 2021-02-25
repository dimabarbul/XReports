using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using XReports.Interfaces;

namespace XReports.Models
{
    public class VerticalReportSchema<TSourceEntity> : ReportSchema<TSourceEntity>
    {
        public override IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source)
        {
            ReportTable<ReportCell> table = new ReportTable<ReportCell>
            {
                Properties = this.TableProperties,
                HeaderRows = this.CreateComplexHeader(),
                Rows = this.GetRows(source),
            };

            return table;
        }

        public IReportTable<ReportCell> BuildReportTable(IDataReader dataReader)
        {
            ReportTable<ReportCell> table = new ReportTable<ReportCell>
            {
                Properties = this.TableProperties,
                HeaderRows = this.CreateComplexHeader(),
                Rows = this.GetRows(dataReader),
            };

            return table;
        }

        private IEnumerable<IEnumerable<ReportCell>> GetRows(IEnumerable<TSourceEntity> source)
        {
            return source
                .Select(
                    entity => this.CellsProviders
                        .Select(p => this.AddGlobalProperties(p.CreateCell(entity))));
        }

        private IEnumerable<IEnumerable<ReportCell>> GetRows(IDataReader dataReader)
        {
            if (!typeof(IDataReader).IsAssignableFrom(typeof(TSourceEntity)))
            {
                throw new InvalidOperationException("Report schema should should be of IDataReader");
            }

            while (dataReader.Read())
            {
                yield return
                    this.CellsProviders
                        .Select(p => this.AddGlobalProperties(p.CreateCell((TSourceEntity)dataReader)));
            }

            dataReader.Close();
        }
    }
}
