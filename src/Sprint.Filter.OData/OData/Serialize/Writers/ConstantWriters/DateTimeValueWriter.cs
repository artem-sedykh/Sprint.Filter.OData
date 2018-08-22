using System;
using System.Xml;

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class DateTimeValueWriter : ValueWriterBase<DateTime>
    {
        public override string Write(object value)
        {
            var dateTimeValue = (DateTime)value;

            return string.Format("datetime'{0}'", XmlConvert.ToString(dateTimeValue, XmlDateTimeSerializationMode.Utc));
        }
    }
}
