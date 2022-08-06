using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using XReports.Interfaces;

namespace XReports.Models
{
    public class VerticalReportSchema<TSourceEntity> : ReportSchema<TSourceEntity>
    {
        public override IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source)
        {
            ReportTable<ReportCell> table = new ReportTable<ReportCell>
            {
                Properties = this.TableProperties,
                HeaderRows = this.CreateComplexHeader(),
                Rows = new RowsFromEntityEnumerator(this, source),
            };

            return table;
        }

        public IReportTable<ReportCell> BuildReportTable(IDataReader dataReader)
        {
            ReportTable<ReportCell> table = new ReportTable<ReportCell>
            {
                Properties = this.TableProperties,
                HeaderRows = this.CreateComplexHeader(),
                Rows = new RowsFromDataReaderEnumerator(this, dataReader),
            };

            return table;
        }

        private class RowsFromEntityEnumerator : IEnumerator<IEnumerable<ReportCell>>, IEnumerable<IEnumerable<ReportCell>>
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

            public IEnumerator<IEnumerable<ReportCell>> GetEnumerator() => this;

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            public bool MoveNext()
            {
                return this.enumerator.MoveNext();
            }

            public void Reset()
            {
                this.enumerator.Reset();
            }

            public void Dispose()
            {
                this.enumerator.Dispose();
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

            public ReportCell Current => this.schema.AddGlobalProperties(this.schema.CellsProviders[this.index].CreateCell(this.entity));

            object IEnumerator.Current => this.Current;

            public IEnumerable<ReportCell> WithEntity(TSourceEntity entity)
            {
                this.entity = entity;
                this.Reset();

                return this;
            }

            public bool MoveNext()
            {
                return ++this.index < this.schema.CellsProviders.Length;
            }

            public void Reset()
            {
                this.index = -1;
            }

            public void Dispose()
            {
            }

            public IEnumerator<ReportCell> GetEnumerator() => this;

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }

        private class RowsFromDataReaderEnumerator : IEnumerator<IEnumerable<ReportCell>>, IEnumerable<IEnumerable<ReportCell>>
        {
            private readonly IDataReader dataReader;
            private readonly CellsFromDataReaderEnumerator cellsEnumerator;

            public RowsFromDataReaderEnumerator(VerticalReportSchema<TSourceEntity> schema, IDataReader dataReader)
            {
                if (!typeof(IDataReader).IsAssignableFrom(typeof(TSourceEntity)))
                {
                    throw new InvalidOperationException("Report schema should should be of IDataReader");
                }

                this.dataReader = dataReader;
                this.cellsEnumerator = new CellsFromDataReaderEnumerator(schema);
            }

            public IEnumerable<ReportCell> Current => this.cellsEnumerator.WithDataReader(this.dataReader);

            object IEnumerator.Current => this.Current;

            public IEnumerator<IEnumerable<ReportCell>> GetEnumerator() => this;

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            public bool MoveNext()
            {
                return this.dataReader.Read();
            }

            public void Reset()
            {
                throw new InvalidOperationException("Data reader cannot be reset");
            }

            public void Dispose()
            {
                this.cellsEnumerator.Dispose();
            }
        }

        private class CellsFromDataReaderEnumerator : IEnumerator<ReportCell>, IEnumerable<ReportCell>
        {
            private readonly VerticalReportSchema<TSourceEntity> schema;
            private int index = -1;
            private IDataReader dataReader;

            public CellsFromDataReaderEnumerator(VerticalReportSchema<TSourceEntity> schema)
            {
                this.schema = schema;
            }

            public ReportCell Current => this.schema.AddGlobalProperties(this.schema.CellsProviders[this.index].CreateCell((TSourceEntity)this.dataReader));

            object IEnumerator.Current => this.Current;

            public IEnumerable<ReportCell> WithDataReader(IDataReader dataReader)
            {
                this.Reset();
                this.dataReader = dataReader;

                return this;
            }

            public bool MoveNext()
            {
                return ++this.index < this.schema.CellsProviders.Length;
            }

            public void Reset()
            {
                this.index = -1;
            }

            public void Dispose()
            {
            }

            public IEnumerator<ReportCell> GetEnumerator() => this;

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }
}
