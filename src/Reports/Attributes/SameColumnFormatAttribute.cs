using System;

namespace Reports.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class SameColumnFormatAttribute : Attribute
    {
    }
}
