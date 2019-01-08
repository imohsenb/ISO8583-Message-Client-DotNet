using imohsenb.iso8583.builders;
using imohsenb.iso8583.entities;
using imohsenb.iso8583.enums;
using imohsenb.iso8583.utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Iso8583Test
{
    [TestClass]
    public class IsoBuilderUnitTest
    {
        string message =
            "123456789008002020058000C00001930000123456123F123F1231" +
            "32333435363738313233343536373839303132333435AB6A53FC65" +
            "5F1487";

        [TestMethod]
        public void TestPackMessage()
        {
            ISOMessage iso = ISOMessageBuilder.Packer(VERSION.V1987)
                .NetworkManagement()
                .MTI(MESSAGE_FUNCTION.Request, MESSAGE_ORIGIN.Acquirer)
                .ProcessCode("930000")
                .SetField(FIELDS.F11_STAN, "123456")
                .SetField(FIELDS.F22_EntryMode, "123F")
                .SetField(FIELDS.F24_NII_FunctionCode, "123F")
                .SetField(FIELDS.F25_POS_ConditionCode, "12")
                .SetField(FIELDS.F41_CA_TerminalID, "12345678")
                .SetField(FIELDS.F42_CA_ID, "123456789012345")
                .GenerateMac(data =>
                {
                    return StringUtil.HexStringToByteArray("AB6A53FC655F1487");
                })
                .SetHeader("1234567890")
                .Build();

            Assert.AreEqual(message, iso.ToString());
        }

        [TestMethod]
        public void TestUnpackMessage()
        {
            ISOMessage iso = ISOMessageBuilder.Unpacker()
                .SetMessage(message)
                .Build();

            Assert.AreEqual(message, iso.ToString());
            Assert.AreEqual(iso.GetStringField(FIELDS.F11_STAN, true), "123456");
            Assert.AreEqual(iso.GetStringField(FIELDS.F22_EntryMode, true), "123F");
            Assert.AreEqual(iso.GetStringField(FIELDS.F24_NII_FunctionCode, true), "123F");
            Assert.AreEqual(iso.GetStringField(FIELDS.F25_POS_ConditionCode, true), "12");
            Assert.AreEqual(iso.GetStringField(FIELDS.F41_CA_TerminalID, true), "12345678");
            Assert.AreEqual(iso.GetStringField(FIELDS.F42_CA_ID, true), "123456789012345");

        }
    }
}
