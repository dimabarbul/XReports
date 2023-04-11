using System;

namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Attribute that specifies column that should become header row. Makes sense only for horizontal report.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class HeaderRowAttribute : Attribute
    {
    }
}
