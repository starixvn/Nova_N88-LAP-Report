using System;

namespace Fanex.Athena.Report.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class SqlColReaderAttribute : Attribute
    {
        public bool Ignored { get; set; }
        public string ColName { get; set; }
    }
}