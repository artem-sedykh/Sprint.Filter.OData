using System;
using System.IO;

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class StreamValueWriter : ValueWriterBase<Stream>
    {
        public override string Write(object value)
        {
            var stream = (Stream)value;
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            var base64 = Convert.ToBase64String(buffer);

            return string.Format("X'{0}'", base64);
        }
    }
}
