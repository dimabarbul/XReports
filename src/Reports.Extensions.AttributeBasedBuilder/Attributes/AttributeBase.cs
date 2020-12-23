using System;

namespace Reports.Extensions.AttributeBasedBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public abstract class AttributeBase : Attribute
    {
        public bool IsHeader { get; set; }
    }
}
