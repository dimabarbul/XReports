using System.Collections.Generic;

namespace Reports.Core.Interfaces
{
    public interface IReportTable<out TReportCell>
    {
        public IEnumerable<IEnumerable<TReportCell>> HeaderRows { get; }
        public IEnumerable<IEnumerable<TReportCell>> Rows { get; }
    }
}
