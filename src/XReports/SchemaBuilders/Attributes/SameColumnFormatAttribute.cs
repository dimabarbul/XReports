using System;

namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Attributes that specifies that whole column should have same format as the first cell in the column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class SameColumnFormatAttribute : Attribute
    {
    }
}
