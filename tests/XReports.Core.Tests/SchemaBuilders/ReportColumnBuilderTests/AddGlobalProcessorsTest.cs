using System;
using System.Collections.Generic;
using FluentAssertions;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.ReportCellProviders;
using XReports.Table;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportColumnBuilderTests
{
    public class AddGlobalProcessorsTest
    {
        [Fact]
        public void AddGlobalProcessorsShouldAddPropertiesToAllColumnsAndRows()
        {
            ReportColumnBuilder<int> builder = new ReportColumnBuilder<int>(
                "Value", new ComputedValueReportCellProvider<int, int>(x => x));

            CustomProcessor1 processor1 = new CustomProcessor1();
            CustomProcessor2 processor2 = new CustomProcessor2();
            IReportColumn<int> provider = builder.Build(
                Array.Empty<IReportCellProperty>(),
                new IReportCellProcessor<int>[] { processor1, processor2 });

            provider.CreateHeaderCell();
            provider.CreateCell(0);

            processor1.ProcessedCells.Should().BeEquivalentTo(0);
            processor2.ProcessedCells.Should().BeEquivalentTo(0);
        }

        [Fact]
        public void AddGlobalProcessorsShouldAddOnlyPropertiesOfTypesNotAddedPreviously()
        {
            ReportColumnBuilder<int> builder = new ReportColumnBuilder<int>(
                "Value", new ComputedValueReportCellProvider<int, int>(x => x));

            CustomProcessor1 addedPreviously = new CustomProcessor1();
            CustomProcessor1 processor1 = new CustomProcessor1();
            CustomProcessor2 processor2 = new CustomProcessor2();
            builder.AddProcessors(addedPreviously);
            IReportColumn<int> provider = builder.Build(
                Array.Empty<IReportCellProperty>(),
                new IReportCellProcessor<int>[] { processor1, processor2 });

            provider.CreateHeaderCell();
            provider.CreateCell(0);

            addedPreviously.ProcessedCells.Should().BeEquivalentTo(0);
            processor1.ProcessedCells.Should().BeEmpty();
            processor2.ProcessedCells.Should().BeEquivalentTo(0);
        }

        [Fact]
        public void AddGlobalProcessorsShouldAddOnlyFirstPropertyOfTypeFromGlobalProcessorsList()
        {
            ReportColumnBuilder<int> builder = new ReportColumnBuilder<int>(
                "Value", new ComputedValueReportCellProvider<int, int>(x => x));

            CustomProcessor1 processor1 = new CustomProcessor1();
            CustomProcessor1 anotherProcessor1 = new CustomProcessor1();
            IReportColumn<int> provider = builder.Build(
                Array.Empty<IReportCellProperty>(),
                new IReportCellProcessor<int>[] { processor1, anotherProcessor1 });

            provider.CreateHeaderCell();
            provider.CreateCell(0);

            processor1.ProcessedCells.Should().BeEquivalentTo(0);
            anotherProcessor1.ProcessedCells.Should().BeEmpty();
        }

        [Fact]
        public void AddGlobalProcessorsShouldThrowWhenSomePropertyIsNull()
        {
            ReportColumnBuilder<int> builder = new ReportColumnBuilder<int>(
                "Value", new ComputedValueReportCellProvider<int, int>(x => x));

            Action action = () => builder.Build(
                Array.Empty<IReportCellProperty>(),
                new IReportCellProcessor<int>[] { new CustomProcessor1(), new CustomProcessor2(), null });

            action.Should().ThrowExactly<ArgumentException>();
        }

        private class BaseCustomProcessor : IReportCellProcessor<int>
        {
            public List<object> ProcessedCells { get; } = new List<object>();

            public void Process(ReportCell cell, int item)
            {
                this.ProcessedCells.Add(cell.GetValue<object>());
            }
        }

        private class CustomProcessor1 : BaseCustomProcessor
        {
        }

        private class CustomProcessor2 : BaseCustomProcessor
        {
        }
    }
}
