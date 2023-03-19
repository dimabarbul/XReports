using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace XReports.DataReader
{
#pragma warning disable IDE0032 // having Current as not auto-property is more readable
    internal readonly struct DataReaderEnumerator : IEnumerator<IDataReader>
    {
        private readonly IDataReader dataReader;

        public DataReaderEnumerator(IDataReader dataReader)
        {
            this.dataReader = dataReader;
        }

        public bool MoveNext()
        {
            return this.dataReader.Read();
        }

        public void Reset()
        {
            throw new InvalidOperationException();
        }

        public IDataReader Current => this.dataReader;

        object IEnumerator.Current => this.Current;

        public void Dispose()
        {
        }
    }
}
#pragma warning restore IDE0032
