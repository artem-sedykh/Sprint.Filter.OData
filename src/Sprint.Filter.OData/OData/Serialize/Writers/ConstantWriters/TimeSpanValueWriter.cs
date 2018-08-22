using System;
using System.Xml;
// ReSharper disable once CheckNamespace
namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class TimeSpanValueWriter : ValueWriterBase<TimeSpan>
    {
        public override string Write(object value)
        {
            return $"time'{XmlConvert.ToString((TimeSpan) value)}'";
        }
    }
}
