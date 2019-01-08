using imohsenb.iso8583.utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Iso8583Test
{
    [TestClass]
    public class StringUtilUnitTest
    {

        byte[] sampleBytes = new byte[] { 96, 0, 7, 128 };
        string sampleHex = "60000780";

        [TestMethod]
        public void HexToByte()
        {
            byte[] b = StringUtil.HexStringToByteArray(sampleHex);
            CollectionAssert.AreEqual(b, sampleBytes);
        }

        [TestMethod]
        public void ByteToHex()
        {
            string hex = StringUtil.FromByteArray(sampleBytes);
            Assert.AreEqual(hex, sampleHex);
        }
    }
}
