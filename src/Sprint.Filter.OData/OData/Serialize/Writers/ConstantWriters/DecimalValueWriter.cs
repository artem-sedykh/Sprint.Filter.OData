using System.Globalization;

namespace Sprint.Filter.OData.Serialize.Writers
{
    public class DecimalValueWriter:ValueWriterBase<decimal>
    {
        public override string Write(object value)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}m", value);
        }
    }
}
