using System.Collections.Generic;

namespace XReports.Interfaces
{
    public interface IReportTable<out TReportCell>
    {
        public IEnumerable<IEnumerable<TReportCell>> HeaderRows { get; }
        public IEnumerable<IEnumerable<TReportCell>> Rows { get; }
    }
}
