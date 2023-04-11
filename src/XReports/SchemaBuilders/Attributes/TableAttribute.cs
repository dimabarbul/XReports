using System;

namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Base class for table-level attributes, so not related to any column. They are processed after attributes for columns.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class TableAttribute : Attribute
    {
    }
}
