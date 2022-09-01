using System;

namespace XReports.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class SameColumnFormatAttribute : Attribute
    {
    }
}
