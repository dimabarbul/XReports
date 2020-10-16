using System;
using System.Reflection;
using System.Reflection.Emit;
using Reports.Interfaces;
using Reports.Models.Cells;
using Reports.Models.Cells.Simple;

namespace Reports.Factories
{
    public static class ReportCellFactory
    {
        public static IReportCell Create<TValue>(TValue value)
        {
            IReportCell cell;

            if (typeof(int).IsAssignableFrom(typeof(TValue)))
            {
                cell = new IntegerReportCell();
            }
            else if (typeof(string).IsAssignableFrom(typeof(TValue)))
            {
                cell = new TextReportCell();
            }
            else if (typeof(decimal).IsAssignableFrom(typeof(TValue)))
            {
                cell = new DecimalReportCell();
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            (cell as ReportCell<TValue>)?.SetValue(value);

            return cell;
        }
    }
}
