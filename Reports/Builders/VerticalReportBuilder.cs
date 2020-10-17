using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;
using Reports.Models;
using Reports.Models.Columns;

namespace Reports.Builders
{
    public partial class VerticalReportBuilder<TSourceEntity>
    {
        private readonly List<IReportColumn<TSourceEntity>> columns = new List<IReportColumn<TSourceEntity>>();

        public void AddColumn<TColumnType>(string title, Func<TSourceEntity, TColumnType> valueSelector)
        {
            this.columns.Add(new EntityPropertyReportColumn<TSourceEntity, TColumnType>(title, valueSelector));
        }

        public void AddColumn<TValue>(string title, IValueProvider<TValue> provider)
        {
            this.columns.Add(new ValueProviderReportColumn<TSourceEntity, TValue>(title, provider));
        }

        public void AddColumn<TValue>(string title, IComputedValueProvider<TSourceEntity, TValue> provider)
        {
            this.columns.Add(new ComputedValueProviderReportColumn<TSourceEntity,TValue>(title, provider));
        }

        public void SetColumnValueFormatter<TColumnType>(string title, IValueFormatter<TColumnType> formatter)
        {
            this.columns
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

        private void BuildBody(ReportTable table, IEnumerable<TSourceEntity> source)
        {
            foreach (TSourceEntity entity in source)
            {
                table.Cells.Add(new List<IReportCell>(
                        this.columns
                            .Select(c => c.CellSelector(entity))
                    )
                );
            }
        }
    }

    internal class ComplexHeader
    {
        public int RowIndex { get; set; }
        public string Title { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
    }
}
