using System;

namespace Reports.Excel.EpplusWriter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class SameColumnFormatAttribute : Attribute
    {
    }
}
