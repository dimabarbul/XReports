using System;
using System.Collections.Generic;
using System.Data;
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
            foreach (TSourceEntity entity in source)
            {
                yield return this.GetRow(entity);
            }
        }

        private IEnumerable<ReportCell> GetRow(TSourceEntity entity)
        {
            for (int i = 0; i < this.CellsProviders.Length; i++)
            {
                yield return this.AddGlobalProperties(this.CellsProviders[i].CreateCell(entity));
            }
        }

        private IEnumerable<IEnumerable<ReportCell>> GetRows(IDataReader dataReader)
        {
            if (!typeof(IDataReader).IsAssignableFrom(typeof(TSourceEntity)))
            {
                throw new InvalidOperationException("Report schema should should be of IDataReader");
            }

            while (dataReader.Read())
            {
                yield return this.GetRow(dataReader);
            }

            dataReader.Close();
        }

        private IEnumerable<ReportCell> GetRow(IDataReader dataReader)
        {
            for (int i = 0; i < this.CellsProviders.Length; i++)
            {
                yield return this.AddGlobalProperties(this.CellsProviders[i].CreateCell((TSourceEntity)dataReader));
            }
        }
    }
}
