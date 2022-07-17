using System;
using System.Collections.Generic;
using System.Data;
using XReports.Interfaces;

namespace XReports.Models
{
    public class VerticalReportSchema<TSourceEntity> : ReportSchema<TSourceEntity>
    {
        private ReportCell[] row;

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
            this.row = new ReportCell[this.CellsProviders.Length];
            foreach (TSourceEntity entity in source)
            {
                this.GetRow(entity);
                yield return this.row;
            }

            this.row = null;
        }

        private void GetRow(TSourceEntity entity)
        {
            for (int i = 0; i < this.CellsProviders.Length; i++)
            {
                this.row[i] = this.AddGlobalProperties(this.CellsProviders[i].CreateCell(entity));
            }
        }

        private IEnumerable<IEnumerable<ReportCell>> GetRows(IDataReader dataReader)
        {
            if (!typeof(IDataReader).IsAssignableFrom(typeof(TSourceEntity)))
            {
                throw new InvalidOperationException("Report schema should should be of IDataReader");
            }

            this.row = new ReportCell[this.CellsProviders.Length];
            while (dataReader.Read())
            {
                this.GetRow(dataReader);
                yield return this.row;
            }

            this.row = null;
            dataReader.Close();
        }

        private void GetRow(IDataReader dataReader)
        {
            for (int i = 0; i < this.CellsProviders.Length; i++)
            {
                this.row[i] = this.AddGlobalProperties(this.CellsProviders[i].CreateCell((TSourceEntity)dataReader));
            }
        }
    }
}
