using XReports.Converter;
using XReports.Table;

namespace XReports.Core.Tests.Converter
{
    public partial class ReportConverterTest
    {
        private class MyHandler : IPropertyHandler<NewReportCell>
        {
            private readonly bool markPropertyProcessed;

            public delegate void HandleDelegate(MyHandler handler, IReportCellProperty property);

            public event HandleDelegate OnHandle;

            public MyHandler(string name, int priority, bool markPropertyProcessed)
            {
                this.markPropertyProcessed = markPropertyProcessed;
                this.Priority = priority;
                this.Name = name;
            }

            public MyHandler(int priority, bool markPropertyProcessed, HandleDelegate onHandle = null)
            {
                this.markPropertyProcessed = markPropertyProcessed;
                this.Priority = priority;
                this.OnHandle += onHandle;
            }

            public int Priority { get; }
            public string Name { get; }

            public bool Handle(IReportCellProperty property, NewReportCell cell)
            {
                this.OnHandle?.Invoke(this, property);

                if (this.Name != null)
                {
                    cell.Data.Add(this.Name);
                }

                return this.markPropertyProcessed;
            }
        }
    }
}
