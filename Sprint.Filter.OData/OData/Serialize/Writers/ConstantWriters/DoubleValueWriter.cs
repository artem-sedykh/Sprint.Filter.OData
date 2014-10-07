using System.Globalization;

namespace Sprint.Filter.OData.Serialize.Writers
{
    public class DoubleValueWriter : ValueWriterBase<double>
    {
        public override string Write(object value)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}", value);
        }
    }
}
