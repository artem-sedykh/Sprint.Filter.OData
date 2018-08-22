// ReSharper disable once CheckNamespace
namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class ByteValueWriter : ValueWriterBase<byte>
    {
        public override string Write(object value)
        {
            var byteValue = (byte)value;

            return byteValue.ToString("X");
        }
    }
}
