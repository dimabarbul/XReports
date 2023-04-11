using System;

namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Attribute that specifies complex header.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ComplexHeaderAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexHeaderAttribute"/> class.
        /// </summary>
        /// <param name="rowIndex">Complex header row index.</param>
        /// <param name="content">Complex header content.</param>
        /// <param name="startIndex">Column index that complex header starts at.</param>
        public ComplexHeaderAttribute(int rowIndex, string content, int startIndex)
        {
            this.RowIndex = rowIndex;
            this.Content = content;
            this.StartIndex = startIndex;

            this.UseIndexes = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexHeaderAttribute"/> class.
        /// </summary>
        /// <param name="rowIndex">Complex header row index.</param>
        /// <param name="content">Complex header content.</param>
        /// <param name="startIndex">Column index that complex header starts at.</param>
        /// <param name="endIndex">Column index that complex header ends at.</param>
        public ComplexHeaderAttribute(int rowIndex, string content, int startIndex, int endIndex)
        {
            this.RowIndex = rowIndex;
            this.Content = content;
            this.StartIndex = startIndex;
            this.EndIndex = endIndex;

            this.UseIndexes = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexHeaderAttribute"/> class.
        /// </summary>
        /// <param name="rowIndex">Complex header row index.</param>
        /// <param name="content">Complex header content.</param>
        /// <param name="startTitle">Column title that complex header starts at.</param>
        /// <param name="useId">True if <paramref name="startTitle"/> is a column identifier, false if <paramref name="startTitle"/> is column title.</param>
        public ComplexHeaderAttribute(int rowIndex, string content, string startTitle, bool useId = false)
        {
            this.RowIndex = rowIndex;
            this.Content = content;
            this.StartTitle = startTitle;
            this.UseId = useId;

            this.UseIndexes = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexHeaderAttribute"/> class.
        /// </summary>
        /// <param name="rowIndex">Complex header row index.</param>
        /// <param name="content">Complex header content.</param>
        /// <param name="startTitle">Column title that complex header starts at.</param>
        /// <param name="endTitle">Column title that complex header ends at.</param>
        /// <param name="useId">True if <paramref name="startTitle"/> and <paramref name="endTitle"/> are a column identifier, false if <paramref name="startTitle"/> and <paramref name="endTitle"/> are column title.</param>
        public ComplexHeaderAttribute(int rowIndex, string content, string startTitle, string endTitle, bool useId = false)
        {
            this.RowIndex = rowIndex;
            this.Content = content;
            this.StartTitle = startTitle;
            this.EndTitle = endTitle;
            this.UseId = useId;

            this.UseIndexes = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexHeaderAttribute"/> class.
        /// </summary>
        /// <param name="rowIndex">Complex header row index.</param>
        /// <param name="rowSpan">Count of rows that complex header spans.</param>
        /// <param name="content">Complex header content.</param>
        /// <param name="startIndex">Column index that complex header starts at.</param>
        public ComplexHeaderAttribute(int rowIndex, int rowSpan, string content, int startIndex)
        {
            this.RowIndex = rowIndex;
            this.RowSpan = rowSpan;
            this.Content = content;
            this.StartIndex = startIndex;

            this.UseIndexes = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexHeaderAttribute"/> class.
        /// </summary>
        /// <param name="rowIndex">Complex header row index.</param>
        /// <param name="rowSpan">Count of rows that complex header spans.</param>
        /// <param name="content">Complex header content.</param>
        /// <param name="startIndex">Column index that complex header starts at.</param>
        /// <param name="endIndex">Column index that complex header ends at.</param>
        public ComplexHeaderAttribute(int rowIndex, int rowSpan, string content, int startIndex, int endIndex)
        {
            this.RowIndex = rowIndex;
            this.RowSpan = rowSpan;
            this.Content = content;
            this.StartIndex = startIndex;
            this.EndIndex = endIndex;

            this.UseIndexes = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexHeaderAttribute"/> class.
        /// </summary>
        /// <param name="rowIndex">Complex header row index.</param>
        /// <param name="rowSpan">Count of rows that complex header spans.</param>
        /// <param name="content">Complex header content.</param>
        /// <param name="startTitle">Column title that complex header starts at.</param>
        /// <param name="useId">True if <paramref name="startTitle"/> is a column identifier, false if <paramref name="startTitle"/> is column title.</param>
        public ComplexHeaderAttribute(int rowIndex, int rowSpan, string content, string startTitle, bool useId = false)
        {
            this.RowIndex = rowIndex;
            this.RowSpan = rowSpan;
            this.Content = content;
            this.StartTitle = startTitle;
            this.UseId = useId;

            this.UseIndexes = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexHeaderAttribute"/> class.
        /// </summary>
        /// <param name="rowIndex">Complex header row index.</param>
        /// <param name="rowSpan">Count of rows that complex header spans.</param>
        /// <param name="content">Complex header content.</param>
        /// <param name="startTitle">Column title that complex header starts at.</param>
        /// <param name="endTitle">Column title that complex header ends at.</param>
        /// <param name="useId">True if <paramref name="startTitle"/> and <paramref name="endTitle"/> are a column identifier, false if <paramref name="startTitle"/> and <paramref name="endTitle"/> are column title.</param>
        public ComplexHeaderAttribute(int rowIndex, int rowSpan, string content, string startTitle, string endTitle, bool useId = false)
        {
            this.RowIndex = rowIndex;
            this.RowSpan = rowSpan;
            this.Content = content;
            this.StartTitle = startTitle;
            this.EndTitle = endTitle;
            this.UseId = useId;

            this.UseIndexes = false;
        }

        /// <summary>
        /// Gets row index of complex header.
        /// </summary>
        public int RowIndex { get; }

        /// <summary>
        /// Gets complex header content.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Gets count of rows the complex header spans.
        /// </summary>
        public int RowSpan { get; } = 1;

        /// <summary>
        /// Gets column index that complex header starts at. Not null only if complex header position was specified using indexes.
        /// </summary>
        public int? StartIndex { get; }

        /// <summary>
        /// Gets column index that complex header ends at. Not null only if complex header position was specified using indexes.
        /// </summary>
        public int? EndIndex { get; }

        /// <summary>
        /// Gets column title (if <see cref="UseId"/> is false) or ID (if <see cref="UseId"/> is true) that complex header starts at. Not null only if complex header position was specified using title or ID.
        /// </summary>
        public string StartTitle { get; }

        /// <summary>
        /// Gets column title (if <see cref="UseId"/> is false) or ID (if <see cref="UseId"/> is true) that complex header ends at. Not null only if complex header position was specified using title or ID.
        /// </summary>
        public string EndTitle { get; }

        /// <summary>
        /// Gets a value indicating whether <see cref="StartTitle"/> and <see cref="EndTitle"/> should be treated as column identifiers (if true) or column titles (if false).
        /// </summary>
        public bool UseId { get; }

        /// <summary>
        /// Gets a value indicating whether indexes are used to specify complex header position.
        /// </summary>
        public bool UseIndexes { get; }
    }
}
