using System;

namespace Reports.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public abstract class AttributeBase : Attribute
    {
        public bool IsHeader { get; set; }
    }
}
