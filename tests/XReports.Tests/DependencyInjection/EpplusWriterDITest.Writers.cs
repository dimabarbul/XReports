using System.Collections.Generic;
using XReports.Interfaces;
using XReports.Writers;

namespace XReports.Tests.DependencyInjection
{
    public partial class EpplusWriterDITest
    {
        private class CustomEpplusWriter : EpplusWriter
        {
            public CustomEpplusWriter(IEnumerable<IEpplusFormatter> formatters)
                : base(formatters)
            {
            }
        }

        private interface IMyEpplusWriter : IEpplusWriter
        {
        }

        private class MyEpplusWriter : EpplusWriter, IMyEpplusWriter
        {
            public MyEpplusWriter(IEnumerable<IEpplusFormatter> formatters)
                : base(formatters)
            {
            }
        }

        private class Dependency
        {
        }

        private class EpplusWriterWithDependency : EpplusWriter
        {
            public EpplusWriterWithDependency(IEnumerable<IEpplusFormatter> formatters, Dependency _)
                : base(formatters)
            {
            }
        }
    }
}
