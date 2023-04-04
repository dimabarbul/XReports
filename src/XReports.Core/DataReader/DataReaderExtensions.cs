using System.Collections.Generic;
using System.Data;

namespace XReports.DataReader
{
    /// <summary>
    /// Extensions for <see cref="IDataReader"/>.
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Creates <see cref="IEnumerable{IDataReader}"/> which allows iterating over data reader as over collection.
        /// </summary>
        /// <param name="dataReader">Data reader to iterate over.</param>
        /// <returns>IEnumerable object that allows iterating over the data reader.</returns>
        public static IEnumerable<IDataReader> AsEnumerable(this IDataReader dataReader)
        {
            return new EnumerableDataReader(dataReader);
        }
    }
}
