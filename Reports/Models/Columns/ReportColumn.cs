using System;
using Reports.Interfaces;

namespace Reports.Models.Columns
{
    public abstract class ReportColumn<TSourceEntity> : IReportColumn<TSourceEntity>
    {
        protected object ValueFormatOptions;

        public void SetValueFormatOptions(object options)
        {
            this.ValueFormatOptions = options;
        }

        protected IReportCell DecorateCell(IReportCell cell)
        {
            if (this.ValueFormatOptions != null)
            {
                cell.SetValueFormatOptions(this.ValueFormatOptions);
            }

            return cell;
        }

        public abstract string Title { get; }
        public abstract Func<TSourceEntity, IReportCell> CellSelector { get; }
    }
}
