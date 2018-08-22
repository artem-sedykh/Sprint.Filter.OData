using System;
using System.Xml;
// ReSharper disable once CheckNamespace

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class DateTimeValueWriter : ValueWriterBase<DateTime>
    {
        public override string Write(object value)
        {
            var dateTimeValue = (DateTime)value;

            return $"datetime'{XmlConvert.ToString(dateTimeValue, XmlDateTimeSerializationMode.Utc)}'";
        }
    }
}
