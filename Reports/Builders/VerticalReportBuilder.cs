using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;
using Reports.Models;
using Reports.Models.Cells;
using Reports.Models.Columns;

namespace Reports.Builders
{
    public class VerticalReportBuilder<TSourceEntity>
    {
        private List<IReportColumn<TSourceEntity>> Columns { get; set; } = new List<IReportColumn<TSourceEntity>>();

        public void AddColumn<TColumnType>(string title, Func<TSourceEntity, TColumnType> valueSelector)
        {
            this.Columns.Add(new EntityPropertyReportColumn<TSourceEntity, TColumnType>(title, valueSelector));
        }

        public void AddColumn<TValue>(string title, IValueProvider<TValue> provider)
        {
            this.Columns.Add(new ValueProviderReportColumn<TSourceEntity, TValue>(title, provider));
        }

        public void AddColumn<TValue>(string title, IComputedValueProvider<TSourceEntity, TValue> provider)
        {
            this.Columns.Add(new ComputedValueProviderReportColumn<TSourceEntity,TValue>(title, provider));
        }

        public void SetColumnValueFormatter<TColumnType>(string title, IValueFormatter<TColumnType> formatter)
        {
            this.Columns
                .OfType<IReportColumn<TSourceEntity, TColumnType>>()
                .First(c => c.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                .SetValueFormatter(formatter);
        }

        public ReportTable Build(IEnumerable<TSourceEntity> source)
        {
            ReportTable table = new ReportTable();

            this.BuildHeader(table);
            this.BuildBody(table, source);

            return table;
        }

        private void BuildHeader(ReportTable table)
        {
            table.Cells.Add(new List<IReportCell>(
                this.Columns
                    .Select(c => c.Title)
                    .Select(t => new HeaderReportCell(t))
                )
            );
        }

        private void BuildBody(ReportTable table, IEnumerable<TSourceEntity> source)
        {
            foreach (TSourceEntity entity in source)
            {
                table.Cells.Add(new List<IReportCell>(
                        this.Columns
                            .Select(c => c.CellSelector(entity))
                    )
                );
            }
        }
    }
}
