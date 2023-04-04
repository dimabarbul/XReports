using System.Collections.Generic;
using XReports.Schema;

namespace XReports.SchemaBuilders
{
    /// <summary>
    /// Interface for complex header builder.
    /// </summary>
    public interface IComplexHeaderBuilder
    {
        /// <summary>
        /// Adds complex header group.
        /// </summary>
        /// <param name="rowIndex">Complex header row index to add to. The lower the index the higher the row.</param>
        /// <param name="content">Complex header cell content.</param>
        /// <param name="fromColumn">0-based index of left-most report column the group spans.</param>
        /// <param name="toColumn">0-based index of right-most report column the group spans.</param>
        void AddGroup(int rowIndex, string content, int fromColumn, int? toColumn = null);

        /// <summary>
        /// Adds complex header group.
        /// </summary>
        /// <param name="rowIndex">Complex header row index to add to. The lower the index the higher the row.</param>
        /// <param name="content">Complex header cell content.</param>
        /// <param name="fromColumn">Title of left-most report column the group spans.</param>
        /// <param name="toColumn">Title of right-most report column the group spans.</param>
        void AddGroup(int rowIndex, string content, string fromColumn, string toColumn = null);

        /// <summary>
        /// Adds complex header group.
        /// </summary>
        /// <param name="rowIndex">Complex header row index to add to. The lower the index the higher the row.</param>
        /// <param name="content">Complex header cell content.</param>
        /// <param name="fromColumn">Identifier of left-most report column the group spans.</param>
        /// <param name="toColumn">Identifier of right-most report column the group spans.</param>
        void AddGroup(int rowIndex, string content, ColumnId fromColumn, ColumnId toColumn = null);

        /// <summary>
        /// Adds complex header group.
        /// </summary>
        /// <param name="rowIndex">Complex header row index to add to. The lower the index the higher the row.</param>
        /// <param name="rowSpan">How many rows the group spans.</param>
        /// <param name="content">Complex header cell content.</param>
        /// <param name="fromColumn">0-based index of left-most report column the group spans.</param>
        /// <param name="toColumn">0-based index of right-most report column the group spans.</param>
        void AddGroup(int rowIndex, int rowSpan, string content, int fromColumn, int? toColumn = null);

        /// <summary>
        /// Adds complex header group.
        /// </summary>
        /// <param name="rowIndex">Complex header row index to add to. The lower the index the higher the row.</param>
        /// <param name="rowSpan">How many rows the group spans.</param>
        /// <param name="content">Complex header cell content.</param>
        /// <param name="fromColumn">Title of left-most report column the group spans.</param>
        /// <param name="toColumn">Title of right-most report column the group spans.</param>
        void AddGroup(int rowIndex, int rowSpan, string content, string fromColumn, string toColumn = null);

        /// <summary>
        /// Adds complex header group.
        /// </summary>
        /// <param name="rowIndex">Complex header row index to add to. The lower the index the higher the row.</param>
        /// <param name="rowSpan">How many rows the group spans.</param>
        /// <param name="content">Complex header cell content.</param>
        /// <param name="fromColumn">Identifier of left-most report column the group spans.</param>
        /// <param name="toColumn">Identifier of right-most report column the group spans.</param>
        void AddGroup(int rowIndex, int rowSpan, string content, ColumnId fromColumn, ColumnId toColumn = null);

        /// <summary>
        /// Builds complex header based on groups provided earlier.
        /// </summary>
        /// <param name="columnNames">Report column names.</param>
        /// <param name="columnIds">Report column identifiers.</param>
        /// <returns>Complex header.</returns>
        ComplexHeaderCell[,] Build(IReadOnlyList<string> columnNames, IReadOnlyList<ColumnId> columnIds);
    }
}
