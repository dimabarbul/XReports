using System;

namespace XReports.SchemaBuilders.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public abstract class BasePropertyAttribute : Attribute
    {
        public bool IsHeader { get; set; }
    }
}
