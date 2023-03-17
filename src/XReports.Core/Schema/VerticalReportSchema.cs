using System.Collections;
using System.Collections.Generic;
using XReports.Helpers;
using XReports.Table;

namespace XReports.Schema
{
    internal class VerticalReportSchema<TSourceEntity> : IReportSchema<TSourceEntity>
    {
        private readonly IReadOnlyList<IReportColumn<TSourceEntity>> columns;
        private readonly IReadOnlyList<ReportTableProperty> tableProperties;
        private readonly ComplexHeaderCell[,] complexHeader;
        private readonly IReadOnlyDictionary<string, ReportCellProperty[]> complexHeaderProperties;
        private readonly IReadOnlyList<ReportCellProperty> commonComplexHeaderProperties;

        public VerticalReportSchema(
            IReadOnlyList<IReportColumn<TSourceEntity>> columns,
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

        public IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source)
        {
            ReportTable<ReportCell> table = new ReportTable<ReportCell>
            {
                Properties = this.tableProperties,
                HeaderRows = ComplexHeaderHelper.CreateCells(
                    this.columns,
                    this.complexHeader,
                    this.complexHeaderProperties,
                    this.commonComplexHeaderProperties,
                    isTransposed: false),
                Rows = new RowsFromEntityCollection(this, source),
            };

            return table;
        }

        private class RowsFromEntityCollection : IEnumerable<IEnumerable<ReportCell>>
        {
            private readonly VerticalReportSchema<TSourceEntity> schema;
            private readonly IEnumerable<TSourceEntity> source;

            public RowsFromEntityCollection(VerticalReportSchema<TSourceEntity> schema, IEnumerable<TSourceEntity> source)
            {
                this.schema = schema;
                this.source = source;
            }

            public IEnumerator<IEnumerable<ReportCell>> GetEnumerator()
            {
                return new RowsFromEntityEnumerator(this.schema, this.source);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private class RowsFromEntityEnumerator : IEnumerator<IEnumerable<ReportCell>>
        {
            private readonly IEnumerator<TSourceEntity> enumerator;
            private readonly CellsFromEntityEnumerator cellsEnumerator;

            public RowsFromEntityEnumerator(VerticalReportSchema<TSourceEntity> schema, IEnumerable<TSourceEntity> source)
            {
                this.enumerator = source.GetEnumerator();
                this.cellsEnumerator = new CellsFromEntityEnumerator(schema);
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

        private class CellsFromEntityEnumerator : IEnumerator<ReportCell>, IEnumerable<ReportCell>
        {
            private readonly VerticalReportSchema<TSourceEntity> schema;
            private TSourceEntity entity;
            private int index = -1;

            public CellsFromEntityEnumerator(VerticalReportSchema<TSourceEntity> schema)
            {
                this.schema = schema;
            }

            public ReportCell Current => this.schema.columns[this.index].CreateCell(this.entity);

            object IEnumerator.Current => this.Current;

            public IEnumerable<ReportCell> WithEntity(TSourceEntity entity)
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
