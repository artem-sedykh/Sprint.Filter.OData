using System;
// ReSharper disable once CheckNamespace

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class ByteArrayValueWriter : ValueWriterBase<byte[]>
    {
        public override string Write(object value)
        {
            var base64 = Convert.ToBase64String((byte[])value);
            return $"X'{base64}'";
        }
    }
}
