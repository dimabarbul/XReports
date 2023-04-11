using System.Collections;
using System.Collections.Generic;
using XReports.Table;

namespace XReports.Schema
{
    internal class VerticalReportSchema<TSourceItem> : IReportSchema<TSourceItem>
    {
        private readonly IReadOnlyList<IReportColumn<TSourceItem>> columns;
        private readonly IReadOnlyList<ReportTableProperty> tableProperties;
        private readonly ComplexHeaderCell[,] complexHeader;
        private readonly IReadOnlyDictionary<string, ReportCellProperty[]> complexHeaderProperties;
        private readonly IReadOnlyList<ReportCellProperty> commonComplexHeaderProperties;

        public VerticalReportSchema(
            IReadOnlyList<IReportColumn<TSourceItem>> columns,
            IReadOnlyList<ReportTableProperty> tableProperties,
            ComplexHeaderCell[,] complexHeader,
            IReadOnlyDictionary<string, ReportCellProperty[]> complexHeaderProperties,
            IReadOnlyList<ReportCellProperty> commonComplexHeaderProperties)
        {
            this.columns = columns;
            this.tableProperties = tableProperties;
            this.complexHeader = complexHeader;
            this.complexHeaderProperties = complexHeaderProperties;
            this.commonComplexHeaderProperties = commonComplexHeaderProperties;
        }

        public IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceItem> source)
        {
            ReportTable<ReportCell> table = new ReportTable<ReportCell>
            {
                Properties = this.tableProperties,
                HeaderRows = this.complexHeader.CreateCells(
                    this.columns,
                    this.complexHeaderProperties,
                    this.commonComplexHeaderProperties,
                    isTransposed: false),
                Rows = new RowsCollection(this, source),
            };

            return table;
        }

        private class RowsCollection : IEnumerable<IEnumerable<ReportCell>>
        {
            private readonly VerticalReportSchema<TSourceItem> schema;
            private readonly IEnumerable<TSourceItem> source;

            public RowsCollection(VerticalReportSchema<TSourceItem> schema, IEnumerable<TSourceItem> source)
            {
                this.schema = schema;
                this.source = source;
            }

            public IEnumerator<IEnumerable<ReportCell>> GetEnumerator()
            {
                return new RowsEnumerator(this.schema, this.source);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private class RowsEnumerator : IEnumerator<IEnumerable<ReportCell>>
        {
            private readonly IEnumerator<TSourceItem> enumerator;
            private readonly CellsEnumerator cellsEnumerator;

            public RowsEnumerator(VerticalReportSchema<TSourceItem> schema, IEnumerable<TSourceItem> source)
            {
                this.enumerator = source.GetEnumerator();
                this.cellsEnumerator = new CellsEnumerator(schema);
            }

            public IEnumerable<ReportCell> Current => this.cellsEnumerator.WithEntity(this.enumerator.Current);

            object IEnumerator.Current => this.Current;

            public bool MoveNext()
            {
                return this.enumerator.MoveNext();
            }

            public void Reset()
            {
                this.enumerator.Reset();
                this.cellsEnumerator.Reset();
            }

            public void Dispose()
            {
                this.enumerator.Dispose();
                this.cellsEnumerator.Dispose();
            }
        }

        private class CellsEnumerator : IEnumerator<ReportCell>, IEnumerable<ReportCell>
        {
            private readonly VerticalReportSchema<TSourceItem> schema;
            private TSourceItem entity;
            private int index = -1;

            public CellsEnumerator(VerticalReportSchema<TSourceItem> schema)
            {
                this.schema = schema;
            }

            public ReportCell Current => this.schema.columns[this.index].CreateCell(this.entity);

            object IEnumerator.Current => this.Current;

            public IEnumerable<ReportCell> WithEntity(TSourceItem entity)
            {
                this.entity = entity;
                this.Reset();

                return this;
            }

            public bool MoveNext()
            {
                return ++this.index < this.schema.columns.Count;
            }

            public void Reset()
            {
                this.index = -1;
            }

            public void Dispose()
            {
            }

            public IEnumerator<ReportCell> GetEnumerator()
            {
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
