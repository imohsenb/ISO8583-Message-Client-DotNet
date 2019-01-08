using System;
using System.Linq;
using System.Text;

namespace imohsenb.iso8583.utils
{
    public class ByteArray
    {
        private static int frameSize = 512;
        private int size;
        private byte[] data;
        private int _position = 0;

        public ByteArray()
        {
            init();
        }

        private void init()
        {
            this.size = frameSize;
            this.data = new byte[size];
            _position = 0;
        }

        public ByteArray append(byte[] value)
        {
            if (value.Length + _position > size)
                expandBuffer();

            value.CopyTo(data, _position);

            _position += value.Length;
            return this;
        }

        public ByteArray append(byte value)
        {
            append(new byte[] { value });
            return this;
        }

        public ByteArray prepend(byte[] value)
        {
            float vlen = value.Length;
            int newSize = (int)(Math.Ceiling((decimal) ((float)(size + vlen) / frameSize)) * frameSize);
            byte[] dest = new byte[newSize];

            value.CopyTo(dest,0);
            data.CopyTo(dest, (int) vlen);

            data = dest;
            _position = (int) (vlen + _position);
            size = newSize;
            dest = null;

            return this;
        }

        public ByteArray prepend(byte value)
        {
            prepend(new byte[] { value });
            return this;
        }

        private void expandBuffer()
        {

            int newSize = size + frameSize;
            byte[] dest = new byte[size];
            data.CopyTo(dest,0);
            data = dest;
            size = newSize;
            dest = null;
        }

        public int position()
        {
            return _position;
        }

        public int limit()
        {
            return size;
        }

        public byte[] array()
        {
            return data.Take(_position).ToArray();
        }


        public ByteArray compact()
        {
            data = data.Take(_position).ToArray();
            size = _position;
            return this;
        }

        public string ToString()
        {
            return Encoding.ASCII.GetString(array());
        }

        public ByteArray clear()
        {
            data.Initialize();
            init();
            return this;
        }

        public ByteArray replace(byte[] value)
        {

            init();
            append(value);
            return this;
        }
    }
}
