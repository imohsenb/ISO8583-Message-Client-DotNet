using imohsenb.iso8583.interfaces;
using System;
using System.Security.Cryptography;

namespace imohsenb.iso8583.crypto
{
    public class Ansi99MacGenerator : ISOMacGenerator
    {
        private readonly byte[] _key;
        public Ansi99MacGenerator(byte[] key)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key));
            if (key.Length != 8)
                throw new ArgumentException("Key length must be 8.");
        }

        public virtual byte[] Generate(byte[] data)
        {
            var des = new DESCryptoServiceProvider();
            des.Padding = PaddingMode.None;
            des.Mode = CipherMode.ECB;
            var encryptor = des.CreateEncryptor(_key, new byte[8]);
            var result = new byte[8];
            for (int i = 0; i < (int)Math.Ceiling(data.Length / (decimal)8) * 8; i += 8)
            {
                for (int j = 0; j < 8; j++)
                {
                    var val = (byte)0;
                    if (i + j < data.Length)
                    {
                        val = data[i + j];
                    }
                    result[j] ^= val;
                }
                encryptor.TransformBlock(result, 0, 8, result, 0);
            }
            return result;
        }
    }
}
