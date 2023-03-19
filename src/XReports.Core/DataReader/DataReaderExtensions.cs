using System.Data;

namespace XReports.DataReader
{
    public static class DataReaderExtensions
    {
        public static EnumerableDataReader AsEnumerable(this IDataReader dataReader)
        {
            return new EnumerableDataReader(dataReader);
        }
    }
}
