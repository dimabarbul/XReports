using System.Collections.Generic;

namespace Reports.Interfaces
{
    public interface IReportTable
    {
        public IEnumerable<IEnumerable<IReportCell>> HeaderRows { get; }
        public IEnumerable<IEnumerable<IReportCell>> Rows { get; }
    }
}
