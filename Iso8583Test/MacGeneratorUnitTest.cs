using imohsenb.iso8583.crypto;
using imohsenb.iso8583.utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Iso8583Test
{
    [TestClass]
    public class MacGeneratorUnitTest
    {
        string _message1 = "4E6F77206973207468652074696D6520666F7220616C6C20";
        string _message2 = "4E6F77206973207468652074696D6520666F7220616C6C20ABCD";
        string _key1 = "0123456789ABCDEF";
        string _key2 = "FEDCBA9876543210";

        [TestMethod]
        public void TestAnsi99Pad()
        {
            var data = StringUtil.HexStringToByteArray(_message1);
            var key = StringUtil.HexStringToByteArray(_key1);
            var expected = StringUtil.HexStringToByteArray("70A30640CC76DD8B");
            var mac = new Ansi99MacGenerator(key).Generate(data);

            CollectionAssert.AreEqual(mac, expected);
        }
        [TestMethod]
        public void TestAnsi99NoPad()
        {
            var data = StringUtil.HexStringToByteArray(_message2);
            var key = StringUtil.HexStringToByteArray(_key1);
            var expected = StringUtil.HexStringToByteArray("36611DBB2D0AC1E6");
            var mac = new Ansi99MacGenerator(key).Generate(data);

            CollectionAssert.AreEqual(mac, expected);
        }


        [TestMethod]
        public void TestAnsi919Pad()
        {
            var data = StringUtil.HexStringToByteArray(_message1);
            var key1 = StringUtil.HexStringToByteArray(_key1);
            var key2 = StringUtil.HexStringToByteArray(_key2);
            var expected = StringUtil.HexStringToByteArray("A1C72E74EA3FA9B6");
            var mac = new Ansi919MacGenerator(key1, key2).Generate(data);

            CollectionAssert.AreEqual(mac, expected);
        }
        [TestMethod]
        public void TestAnsi919NoPad()
        {
            var data = StringUtil.HexStringToByteArray(_message2);
            var key1 = StringUtil.HexStringToByteArray(_key1);
            var key2 = StringUtil.HexStringToByteArray(_key2);
            var expected = StringUtil.HexStringToByteArray("1C050879D95816B8");
            var mac = new Ansi919MacGenerator(key1, key2).Generate(data);

            CollectionAssert.AreEqual(mac, expected);
        }
    }
}
