using System;

namespace XReports.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public abstract class BasePropertyAttribute : Attribute
    {
        public bool IsHeader { get; set; }
    }
}
