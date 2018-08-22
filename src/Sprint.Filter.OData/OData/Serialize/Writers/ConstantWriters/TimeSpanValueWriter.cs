using System;
using System.Xml;

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class TimeSpanValueWriter : ValueWriterBase<TimeSpan>
    {
        public override string Write(object value)
        {
            return string.Format("time'{0}'", XmlConvert.ToString((TimeSpan)value));
        }
    }
}
