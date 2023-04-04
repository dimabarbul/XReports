using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace XReports.DataReader
{
    internal class EnumerableDataReader : IEnumerable<IDataReader>
    {
        private readonly IDataReader dataReader;

        public EnumerableDataReader(IDataReader dataReader)
        {
            this.dataReader = dataReader;
        }

        public IEnumerator<IDataReader> GetEnumerator()
        {
            return new DataReaderEnumerator(this.dataReader);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
