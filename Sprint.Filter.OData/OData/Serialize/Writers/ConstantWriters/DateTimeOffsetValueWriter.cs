using System;
using System.Xml;

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class DateTimeOffsetValueWriter : ValueWriterBase<DateTimeOffset>
    {
        public override string Write(object value)
        {
            return string.Format("datetimeoffset'{0}'", XmlConvert.ToString((DateTimeOffset)value));
        }
    }
}
