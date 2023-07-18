using System;

namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Attribute that specifies report column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ReportColumnAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportColumnAttribute"/> class.
        /// </summary>
        /// <param name="order">Report column order.</param>
        /// <param name="title">Report column title. If null, property name will be used.</param>
        public ReportColumnAttribute(int order, string title = null)
        {
            this.Order = order;
            this.Title = title;
        }

        /// <summary>
        /// Gets report column order.
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Gets report column title.
        /// </summary>
        public string Title { get; }
    }
}
