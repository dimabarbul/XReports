using XReports.Schema;
using XReports.Table;

namespace XReports.SchemaBuilders
{
    /// <summary>
    /// Interface for report schema builder.
    /// </summary>
    /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
    public interface IReportSchemaBuilder<TSourceItem>
    {
        /// <summary>
        /// Adds report global properties. They will be applied to all columns.
        /// </summary>
        /// <param name="properties">Properties to add.</param>
        /// <returns>The report schema builder.</returns>
        IReportSchemaBuilder<TSourceItem> AddGlobalProperties(params IReportCellProperty[] properties);

        /// <summary>
        /// Adds report global processors. They will be applied to all column to
        /// body cells only.
        /// </summary>
        /// <param name="processors">Processors to add.</param>
        /// <returns>The report schema builder.</returns>
        IReportSchemaBuilder<TSourceItem> AddGlobalProcessors(params IReportCellProcessor<TSourceItem>[] processors);

        /// <summary>
        /// Adds report table properties.
        /// </summary>
        /// <param name="properties">Table properties to add.</param>
        /// <returns>The report schema builder.</returns>
        IReportSchemaBuilder<TSourceItem> AddTableProperties(params IReportTableProperty[] properties);

        /// <summary>
        /// Adds complex header group. <seealso cref="IComplexHeaderBuilder.AddGroup(int,string,int,System.Nullable{int})"/>
        /// </summary>
        /// <param name="rowIndex">Complex header row index to add to. The lower the index the higher the row.</param>
        /// <param name="content">Complex header cell content.</param>
        /// <param name="fromColumn">0-based index of left-most report column the group spans.</param>
        /// <param name="toColumn">0-based index of right-most report column the group spans.</param>
        /// <returns>The report schema builder.</returns>
        IReportSchemaBuilder<TSourceItem> AddComplexHeader(
            int rowIndex, string content, int fromColumn, int? toColumn = null);

        /// <summary>
        /// Adds complex header group. <seealso cref="IComplexHeaderBuilder.AddGroup(int,string,string,string)"/>
        /// </summary>
        /// <param name="rowIndex">Complex header row index to add to. The lower the index the higher the row.</param>
        /// <param name="content">Complex header cell content.</param>
        /// <param name="fromColumn">Title of left-most report column the group spans.</param>
        /// <param name="toColumn">Title of right-most report column the group spans.</param>
        /// <returns>The report schema builder.</returns>
        IReportSchemaBuilder<TSourceItem> AddComplexHeader(
            int rowIndex, string content, string fromColumn, string toColumn = null);

        /// <summary>
        /// Adds complex header group. <seealso cref="IComplexHeaderBuilder.AddGroup(int,string,ColumnId,ColumnId)"/>
        /// </summary>
        /// <param name="rowIndex">Complex header row index to add to. The lower the index the higher the row.</param>
        /// <param name="content">Complex header cell content.</param>
        /// <param name="fromColumn">Identifier of left-most report column the group spans.</param>
        /// <param name="toColumn">Identifier of right-most report column the group spans.</param>
        /// <returns>The report schema builder.</returns>
        IReportSchemaBuilder<TSourceItem> AddComplexHeader(
            int rowIndex, string content, ColumnId fromColumn, ColumnId toColumn = null);

        /// <summary>
        /// Adds complex header group. <seealso cref="IComplexHeaderBuilder.AddGroup(int,int,string,int,System.Nullable{int})"/>
        /// </summary>
        /// <param name="rowIndex">Complex header row index to add to. The lower the index the higher the row.</param>
        /// <param name="rowSpan">How many rows the group spans.</param>
        /// <param name="content">Complex header cell content.</param>
        /// <param name="fromColumn">0-based index of left-most report column the group spans.</param>
        /// <param name="toColumn">0-based index of right-most report column the group spans.</param>
        /// <returns>The report schema builder.</returns>
        IReportSchemaBuilder<TSourceItem> AddComplexHeader(
            int rowIndex, int rowSpan, string content, int fromColumn, int? toColumn = null);

        /// <summary>
        /// Adds complex header group. <seealso cref="IComplexHeaderBuilder.AddGroup(int,int,string,string,string)"/>
        /// </summary>
        /// <param name="rowIndex">Complex header row index to add to. The lower the index the higher the row.</param>
        /// <param name="rowSpan">How many rows the group spans.</param>
        /// <param name="content">Complex header cell content.</param>
        /// <param name="fromColumn">Title of left-most report column the group spans.</param>
        /// <param name="toColumn">Title of right-most report column the group spans.</param>
        /// <returns>The report schema builder.</returns>
        IReportSchemaBuilder<TSourceItem> AddComplexHeader(
            int rowIndex, int rowSpan, string content, string fromColumn, string toColumn = null);

        /// <summary>
        /// Adds complex header group. <seealso cref="IComplexHeaderBuilder.AddGroup(int,int,string,ColumnId,ColumnId)"/>
        /// </summary>
        /// <param name="rowIndex">Complex header row index to add to. The lower the index the higher the row.</param>
        /// <param name="rowSpan">How many rows the group spans.</param>
        /// <param name="content">Complex header cell content.</param>
        /// <param name="fromColumn">Identifier of left-most report column the group spans.</param>
        /// <param name="toColumn">Identifier of right-most report column the group spans.</param>
        /// <returns>The report schema builder.</returns>
        IReportSchemaBuilder<TSourceItem> AddComplexHeader(
            int rowIndex, int rowSpan, string content, ColumnId fromColumn, ColumnId toColumn = null);

        /// <summary>
        /// Adds properties to complex header cells which content equals to <paramref name="content"/>. Properties are only added to cells generated from complex groups, not from report column headers.
        /// </summary>
        /// <param name="content">Complex header cell content to add properties to.</param>
        /// <param name="properties">Properties to add.</param>
        /// <returns>The report schema builder.</returns>
        IReportSchemaBuilder<TSourceItem> AddComplexHeaderProperties(
            string content, params IReportCellProperty[] properties);

        /// <summary>
        /// Adds properties to all complex header cells. Properties are only added to cells generated from complex groups, not from report column headers.
        /// </summary>
        /// <param name="properties">Properties to add.</param>
        /// <returns>The report schema builder.</returns>
        IReportSchemaBuilder<TSourceItem> AddComplexHeaderProperties(params IReportCellProperty[] properties);

        /// <summary>
        /// Adds column to the end of the report.
        /// </summary>
        /// <param name="title">Report column title.</param>
        /// <param name="provider">Provider for cells of the report column.</param>
        /// <returns>Builder for added column.</returns>
        IReportColumnBuilder<TSourceItem> AddColumn(string title, IReportCellProvider<TSourceItem> provider);

        /// <summary>
        /// Adds column to the end of the report.
        /// </summary>
        /// <param name="id">Identifier of the column.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="provider">Provider for cells of the report column.</param>
        /// <returns>Builder for added column.</returns>
        IReportColumnBuilder<TSourceItem> AddColumn(ColumnId id, string title, IReportCellProvider<TSourceItem> provider);

        /// <summary>
        /// Inserts column at specified position.
        /// </summary>
        /// <param name="index">0-based index to insert column at.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="provider">Provider for cells of the report column.</param>
        /// <returns>Builder for inserted column.</returns>
        IReportColumnBuilder<TSourceItem> InsertColumn(int index, string title, IReportCellProvider<TSourceItem> provider);

        /// <summary>
        /// Inserts column at specified position.
        /// </summary>
        /// <param name="index">0-based index to insert column at.</param>
        /// <param name="id">Identifier of the column.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="provider">Provider for cells of the report column.</param>
        /// <returns>Builder for inserted column.</returns>
        IReportColumnBuilder<TSourceItem> InsertColumn(int index, ColumnId id, string title, IReportCellProvider<TSourceItem> provider);

        /// <summary>
        /// Inserts column before existing column with specified title.
        /// </summary>
        /// <param name="beforeTitle">Title of existing column to insert new column before.</param>
        /// <param name="title">New report column title.</param>
        /// <param name="provider">Provider for cells of the report column.</param>
        /// <returns>Builder for inserted column.</returns>
        IReportColumnBuilder<TSourceItem> InsertColumnBefore(string beforeTitle, string title, IReportCellProvider<TSourceItem> provider);

        /// <summary>
        /// Inserts column before existing column with specified identifier.
        /// </summary>
        /// <param name="beforeId">Identifier of existing column to insert new column before.</param>
        /// <param name="title">New report column title.</param>
        /// <param name="provider">Provider for cells of the report column.</param>
        /// <returns>Builder for inserted column.</returns>
        IReportColumnBuilder<TSourceItem> InsertColumnBefore(ColumnId beforeId, string title, IReportCellProvider<TSourceItem> provider);

        /// <summary>
        /// Inserts column before existing column with specified title.
        /// </summary>
        /// <param name="beforeTitle">Title of existing column to insert new column before.</param>
        /// <param name="id">Identifier of the column.</param>
        /// <param name="title">New report column title.</param>
        /// <param name="provider">Provider for cells of the report column.</param>
        /// <returns>Builder for inserted column.</returns>
        IReportColumnBuilder<TSourceItem> InsertColumnBefore(string beforeTitle, ColumnId id, string title, IReportCellProvider<TSourceItem> provider);

        /// <summary>
        /// Inserts column before existing column with specified identifier.
        /// </summary>
        /// <param name="beforeId">Identifier of existing column to insert new column before.</param>
        /// <param name="id">Identifier of the column.</param>
        /// <param name="title">New report column title.</param>
        /// <param name="provider">Provider for cells of the report column.</param>
        /// <returns>Builder for inserted column.</returns>
        IReportColumnBuilder<TSourceItem> InsertColumnBefore(ColumnId beforeId, ColumnId id, string title, IReportCellProvider<TSourceItem> provider);

        /// <summary>
        /// Returns builder for report column with specified title.
        /// </summary>
        /// <param name="title">Report column title.</param>
        /// <returns>Builder for report column.</returns>
        IReportColumnBuilder<TSourceItem> ForColumn(string title);

        /// <summary>
        /// Returns builder for report column at specified index.
        /// </summary>
        /// <param name="index">0-based index of report column.</param>
        /// <returns>Builder for report column.</returns>
        IReportColumnBuilder<TSourceItem> ForColumn(int index);

        /// <summary>
        /// Returns builder for report column with specified title.
        /// </summary>
        /// <param name="id">Report column identifier.</param>
        /// <returns>Builder for report column.</returns>
        IReportColumnBuilder<TSourceItem> ForColumn(ColumnId id);

        /// <summary>
        /// Builds schema for vertical report.
        /// </summary>
        /// <returns>Vertical report schema.</returns>
        IReportSchema<TSourceItem> BuildVerticalSchema();

        /// <summary>
        /// Builds schema for horizontal report.
        /// </summary>
        /// <param name="headerRowsCount">Count of first columns that should be made header rows in horizontal report.</param>
        /// <returns>Horizontal report schema.</returns>
        IReportSchema<TSourceItem> BuildHorizontalSchema(int headerRowsCount);
    }
}
