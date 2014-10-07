using System.Globalization;

namespace Sprint.Filter.OData.Serialize.Writers
{
    public class SingleValueWriter : ValueWriterBase<float>
    {        
        public override string Write(object value)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}f", value);
        }
    }
}
