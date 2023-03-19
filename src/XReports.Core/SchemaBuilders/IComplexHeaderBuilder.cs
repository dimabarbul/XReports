using System.Collections.Generic;
using XReports.Schema;

namespace XReports.SchemaBuilders
{
    public interface IComplexHeaderBuilder
    {
        void AddGroup(int rowIndex, string title, int fromColumn, int? toColumn = null);
        void AddGroup(int rowIndex, string title, string fromColumn, string toColumn = null);
        void AddGroup(int rowIndex, string title, ColumnId fromColumn, ColumnId toColumn = null);
        void AddGroup(int rowIndex, int rowSpan, string title, int fromColumn, int? toColumn = null);
        void AddGroup(int rowIndex, int rowSpan, string title, string fromColumn, string toColumn = null);
        void AddGroup(int rowIndex, int rowSpan, string title, ColumnId fromColumn, ColumnId toColumn = null);
        ComplexHeaderCell[,] Build(IReadOnlyList<string> columnNames, IReadOnlyList<ColumnId> columnIds);
    }
}
