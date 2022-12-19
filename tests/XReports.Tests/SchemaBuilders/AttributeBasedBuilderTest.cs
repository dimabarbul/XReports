using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using XReports.Models;

namespace XReports.Tests.SchemaBuilders
{
    public partial class AttributeBasedBuilderTest
    {
        private readonly IServiceProvider serviceProvider = new ServiceCollection().BuildServiceProvider();

        private ReportCell[][] GetCellsAsArray(IEnumerable<IEnumerable<ReportCell>> cells)
        {
            return cells.Select(row => row.Select(c => c?.Clone() as ReportCell).ToArray()).ToArray();
        }
    }
}
