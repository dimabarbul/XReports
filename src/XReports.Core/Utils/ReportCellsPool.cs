using System;
using System.Collections.Generic;
using XReports.Models;

namespace XReports.Utils
{
    public class ReportCellsPool
    {
        private readonly List<BaseReportCell> items = new List<BaseReportCell>();
        private readonly List<bool> reservations = new List<bool>();

        public TReportCell GetOrCreate<TReportCell>(Func<TReportCell> create, Action<TReportCell> update, bool isReserved = true)
            where TReportCell : BaseReportCell
        {
            TReportCell result = null;

            for (int i = 0; i < this.items.Count; i++)
            {
                if (!this.reservations[i] && this.items[i] is TReportCell typedReportCell)
                {
                    this.reservations[i] = isReserved;
                    result = typedReportCell;
                    result.Reset();
                    update?.Invoke(result);

                    break;
                }
            }

            if (result == null)
            {
                result = create();
                this.items.Add(result);
                this.reservations.Add(true);
            }

            return result;
        }

        public void Release<TReportCell>(TReportCell reportCell)
            where TReportCell : BaseReportCell
        {
            for (int i = 0; i < this.items.Count; i++)
            {
                if (this.items[i] == reportCell)
                {
                    this.reservations[i] = false;

                    break;
                }
            }
        }
    }
}
