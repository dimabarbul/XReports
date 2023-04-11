using System;

namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Base class for property attributes that can be applied to header or body cells.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public abstract class BasePropertyAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether attribute action should be applied to header cells (if true) or to body cells (if false).
        /// </summary>
        public bool IsHeader { get; set; }
    }
}
