using System;
using System.Xml;
// ReSharper disable once CheckNamespace

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class DateTimeOffsetValueWriter : ValueWriterBase<DateTimeOffset>
    {
        public override string Write(object value)
        {
            return $"datetimeoffset'{XmlConvert.ToString((DateTimeOffset) value)}'";
        }
    }
}
