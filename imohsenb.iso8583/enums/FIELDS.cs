using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imohsenb.iso8583.enums
{
    public class FIELDS
    {
        public static readonly SortedDictionary<int, FIELDS> map = new SortedDictionary<int, FIELDS>();

        public static readonly FIELDS F1_Bitmap = new FIELDS("F1_Bitmap", 1, "b", 64, true, null);
        public static readonly FIELDS F2_PAN = new FIELDS("F2_PAN", 2, "n", 19, false, "LL");
        public static readonly FIELDS F3_ProcessCode = new FIELDS("F3_ProcessCode", 3, "n", 6, true, null);
        public static readonly FIELDS F4_AmountTransaction = new FIELDS("F4_AmountTransaction", 4, "n", 12, true, null);
        public static readonly FIELDS F5_AmountSettlement = new FIELDS("F5_AmountSettlement", 5, "n", 12, true, null);
        public static readonly FIELDS F6_AmountCardholder = new FIELDS("F6_AmountCardholder", 6, "n", 12, true, null);
        public static readonly FIELDS F7_TransmissionDataTime = new FIELDS("F7_TransmissionDataTime", 7, "n", 10, true, null);
        public static readonly FIELDS F8_AmountCardholder_BillingFee = new FIELDS("F8_AmountCardholder_BillingFee", 8, "n", 8, true, null);
        public static readonly FIELDS F9_ConversionRate_Settlement = new FIELDS("F9_ConversionRate_Settlement", 9, "n", 8, true, null);
        public static readonly FIELDS F10_ConversionRate_Cardholder = new FIELDS("F10_ConversionRate_Cardholder", 10, "n", 8, true, null);
        public static readonly FIELDS F11_STAN = new FIELDS("F11_STAN", 11, "n", 6, true, null);
        public static readonly FIELDS F12_LocalTime = new FIELDS("F12_LocalTime", 12, "n", 6, true, null);
        public static readonly FIELDS F13_LocalDate = new FIELDS("F13_LocalDate", 13, "n", 4, true, null);
        public static readonly FIELDS F14_ExpirationDate = new FIELDS("F14_ExpirationDate", 14, "n", 4, true, null);
        public static readonly FIELDS F15_SettlementDate = new FIELDS("F15_SettlementDate", 15, "n", 4, true, null);
        public static readonly FIELDS F16_CurrencyConversionDate = new FIELDS("F16_CurrencyConversionDate", 16, "n", 4, true, null);
        public static readonly FIELDS F17_CaptureDate = new FIELDS("F17_CaptureDate", 17, "n", 4, true, null);
        public static readonly FIELDS F18_MerchantType = new FIELDS("F18_MerchantType", 18, "n", 4, true, null);
        public static readonly FIELDS F19_AcquiringInstitution = new FIELDS("F19_AcquiringInstitution", 19, "n", 3, true, null);
        public static readonly FIELDS F20_PANExtended = new FIELDS("F20_PANExtended", 20, "n", 3, true, null);
        public static readonly FIELDS F21_ForwardingInstitution = new FIELDS("F21_ForwardingInstitution", 21, "n", 3, true, null);
        public static readonly FIELDS F22_EntryMode = new FIELDS("F22_EntryMode", 22, "n", 3, true, null);
        public static readonly FIELDS F23_PANSequence = new FIELDS("F23_PANSequence", 23, "n", 3, true, null);
        public static readonly FIELDS F24_NII_FunctionCode = new FIELDS("F24_NII_FunctionCode", 24, "n", 3, true, null);
        public static readonly FIELDS F25_POS_ConditionCode = new FIELDS("F25_POS_ConditionCode", 25, "n", 2, true, null);
        public static readonly FIELDS F26_POS_CaptureCode = new FIELDS("F26_POS_CaptureCode", 26, "n", 2, true, null);
        public static readonly FIELDS F27_AuthIdResponseLength = new FIELDS("F27_AuthIdResponseLength", 27, "n", 1, true, null);
        public static readonly FIELDS F28_Amount_TransactionFee = new FIELDS("F28_Amount_TransactionFee", 28, "x+n", 8, true, null);
        public static readonly FIELDS F29_Amount_SettlementFee = new FIELDS("F29_Amount_SettlementFee", 29, "x+n", 8, true, null);
        public static readonly FIELDS F30_Amount_TransactionProcessingFee = new FIELDS("F30_Amount_TransactionProcessingFee", 30, "x+n", 8, true, null);
        public static readonly FIELDS F31_Amount_SettlementProcessingFee = new FIELDS("F31_Amount_SettlementProcessingFee", 31, "x+n", 8, true, null);
        public static readonly FIELDS F32_AcquiringInstitutionIdCode = new FIELDS("F32_AcquiringInstitutionIdCode", 32, "n", 11, false, "LL");
        public static readonly FIELDS F33_ForwardingInstitutionIdCode = new FIELDS("F33_ForwardingInstitutionIdCode", 33, "n", 11, false, "LL");
        public static readonly FIELDS F34_PAN_Extended = new FIELDS("F34_PAN_Extended", 34, "ns", 28, false, "LL");
        public static readonly FIELDS F35_Track2 = new FIELDS("F35_Track2", 35, "z", 37, false, "LL");
        public static readonly FIELDS F36_Track3 = new FIELDS("F36_Track3", 36, "z", 104, false, "LLL");
        public static readonly FIELDS F37_RRN = new FIELDS("F37_RRN", 37, "an", 12, true, null);
        public static readonly FIELDS F38_AuthIdResponse = new FIELDS("F38_AuthIdResponse", 38, "an", 6, true, null);
        public static readonly FIELDS F39_ResponseCode = new FIELDS("F39_ResponseCode", 39, "an", 2, true, null);
        public static readonly FIELDS F40_ServiceRestrictionCode = new FIELDS("F40_ServiceRestrictionCode", 40, "an", 3, true, null);
        public static readonly FIELDS F41_CA_TerminalID = new FIELDS("F41_CA_TerminalID", 41, "ans", 8, true, null);
        public static readonly FIELDS F42_CA_ID = new FIELDS("F42_CA_ID", 42, "ans", 15, true, null);
        public static readonly FIELDS F43_CardAcceptorInfo = new FIELDS("F43_CardAcceptorInfo", 43, "ans", 40, true, null);
        public static readonly FIELDS F44_AddResponseData = new FIELDS("F44_AddResponseData", 44, "an", 25, false, "LL");
        public static readonly FIELDS F45_Track1 = new FIELDS("F45_Track1", 45, "an", 76, false, "LL");
        public static readonly FIELDS F46_AddData_ISO = new FIELDS("F46_AddData_ISO", 46, "an", 999, false, "LLL");
        public static readonly FIELDS F47_AddData_National = new FIELDS("F47_AddData_National", 47, "an", 999, false, "LLL");
        public static readonly FIELDS F48_AddData_Private = new FIELDS("F48_AddData_Private", 48, "an", 999, false, "LLL");
        public static readonly FIELDS F49_CurrencyCode_Transactoion = new FIELDS("F49_CurrencyCode_Transactoion", 49, "a|n", 3, true, null);
        public static readonly FIELDS F50_CurrencyCode_Settlement = new FIELDS("F50_CurrencyCode_Settlement", 50, "a|n", 3, true, null);
        public static readonly FIELDS F51_CurrencyCode_Cardholder = new FIELDS("F51_CurrencyCode_Cardholder", 51, "a|n", 3, true, null);
        public static readonly FIELDS F52_PIN = new FIELDS("F52_PIN", 52, "b", 8, true, null);
        public static readonly FIELDS F53_SecurityControlInfo = new FIELDS("F53_SecurityControlInfo", 53, "n", 16, true, null);
        public static readonly FIELDS F54_AddAmount = new FIELDS("F54_AddAmount", 54, "an", 120, false, "LLL");
        public static readonly FIELDS F55_ICC = new FIELDS("F55_ICC", 55, "ans", 999, false, "LLL");
        public static readonly FIELDS F56_Reserved_ISO = new FIELDS("F56_Reserved_ISO", 56, "ans", 999, false, "LLL");
        public static readonly FIELDS F57_Reserved_National = new FIELDS("F57_Reserved_National", 57, "ans", 999, false, "LLL");
        public static readonly FIELDS F58_Reserved_National = new FIELDS("F58_Reserved_National", 58, "ans", 999, false, "LLL");
        public static readonly FIELDS F59_Reserved_National = new FIELDS("F59_Reserved_National", 59, "ans", 999, false, "LLL");
        public static readonly FIELDS F60_Reserved_National = new FIELDS("F60_Reserved_National", 60, "ans", 999, false, "LLL");
        public static readonly FIELDS F61_Reserved_Private = new FIELDS("F61_Reserved_Private", 61, "ans", 999, false, "LLL");
        public static readonly FIELDS F62_Reserved_Private = new FIELDS("F62_Reserved_Private", 62, "ans", 999, false, "LLL");
        public static readonly FIELDS F63_Reserved_Private = new FIELDS("F63_Reserved_Private", 63, "ans", 999, false, "LLL");
        public static readonly FIELDS F64_MAC = new FIELDS("F64_MAC", 64, "b", 16, true, null);

        private string _name;
        private int _no;
        private string _type;
        private int _length;
        private bool _fixed;
        private string _format;

        private FIELDS(string name, int no, string type, int length, bool _fixed, string format)
        {
            this._name = name;
            this._no = no;
            this._type = type;
            this._length = length;
            this._fixed = _fixed;
            this._format = format;
            map.Add(no, this);
        }

        public int GetNo()
        {
            return _no;
        }

        public string getType()
        {
            return _type;
        }

        public int GetLength()
        {
            return _length;
        }

        public bool IsFixed()
        {
            return _fixed;
        }

        public string GetFormat()
        {
            return _format;
        }

        public static  FIELDS ValueOf(int no)
        {
            FIELDS value;
            map.TryGetValue(no, out value);
            return value;
        }
    }

}
