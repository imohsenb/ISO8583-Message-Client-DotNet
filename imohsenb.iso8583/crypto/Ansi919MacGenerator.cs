using System;
using System.Security.Cryptography;

namespace imohsenb.iso8583.crypto
{
    public class Ansi919MacGenerator : Ansi99MacGenerator
    {
        private readonly byte[] _key1;
        private readonly byte[] _key2;
        public Ansi919MacGenerator(byte[] key1, byte[] key2) : base(key1)
        {
            _key2 = key2 ?? throw new ArgumentNullException(nameof(key2));
            _key1 = key1;
            if (key2.Length != 8)
                throw new ArgumentException("Key length must be 8.");
        }

        public override byte[] Generate(byte[] data)
        {
            var des = new DESCryptoServiceProvider();
            des.Padding = PaddingMode.None;
            des.Mode = CipherMode.ECB;
            var decryptor = des.CreateDecryptor(_key2, new byte[8]);
            var encryptor = des.CreateEncryptor(_key1, new byte[8]);
            var result = base.Generate(data);
            decryptor.TransformBlock(result, 0, 8, result, 0);
            encryptor.TransformBlock(result, 0, 8, result, 0);
            return result;
        }
    }
}
