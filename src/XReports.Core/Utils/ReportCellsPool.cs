using System;
using System.Collections.Generic;
using XReports.Models;

namespace XReports.Utils
{
    public class ReportCellsPool<TReportCell>
        where TReportCell : BaseReportCell
    {
        private readonly Stack<TReportCell> available = new Stack<TReportCell>();

        public TReportCell GetOrCreate(Func<TReportCell> create, Action<TReportCell> update)
        {
            if (this.available.TryPop(out TReportCell result))
            {
                update(result);
                return result;
            }

            return create();
        }

        public void Release(TReportCell reportCell)
        {
            this.available.Push(reportCell);
        }
    }
}
