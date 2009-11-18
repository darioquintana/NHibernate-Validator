using NHibernate.Validator.Constraints;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class IBANFixture : BaseValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			var v = new IBANAttribute();

			//True value tests:
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsTrue(v.IsValid("", null));
			Assert.IsTrue(v.IsValid("AD12 0001 2030 2003 5910 0100", null));
			Assert.IsTrue(v.IsValid("AD12-0001-2030-2003-5910-0100", null));
			Assert.IsTrue(v.IsValid("AT61 1904 3002 3457 3201 ", null));
			Assert.IsTrue(v.IsValid("BE68 5390 0754 7034 ", null));
			Assert.IsTrue(v.IsValid("BA39 1290 0794 0102 8494", null));
			Assert.IsTrue(v.IsValid("BG80 BNBG 9661 1020 3456 78 ", null));
			Assert.IsTrue(v.IsValid("HR12 1001 0051 8630 0016 0", null));
			Assert.IsTrue(v.IsValid("CY17 0020 0128 0000 0012 0052 7600", null));
			Assert.IsTrue(v.IsValid("CZ65 0800 0000 1920 0014 5399", null));
			Assert.IsTrue(v.IsValid("DK50 0040 0440 1162 43", null));
			Assert.IsTrue(v.IsValid("EE38 2200 2210 2014 5685", null));
			Assert.IsTrue(v.IsValid("FI21 1234 5600 0007 85 ", null));
			Assert.IsTrue(v.IsValid("FR14 2004 1010 0505 0001 3M02 606", null));
			Assert.IsTrue(v.IsValid("DE89 3704 0044 0532 0130 00", null));
			Assert.IsTrue(v.IsValid("GI75 NWBK 0000 0000 7099 453", null));
			Assert.IsTrue(v.IsValid("GR16 0110 1250 0000 0001 2300 695", null));
			Assert.IsTrue(v.IsValid("HU42 1177 3016 1111 1018 0000 0000", null));
			Assert.IsTrue(v.IsValid("IS14 0159 2600 7654 5510 7303 39 ", null));
			Assert.IsTrue(v.IsValid("IE29 AIBK 9311 5212 3456 78", null));
			Assert.IsTrue(v.IsValid("IL62 0108 0000 0009 9999 999 ", null));
			Assert.IsTrue(v.IsValid("IT60 X054 2811 1010 0000 0123 456 ", null));
			Assert.IsTrue(v.IsValid("LV80 BANK 0000 4351 9500 1", null));
			Assert.IsTrue(v.IsValid("LI21 0881 0000 2324 013A A ", null));
			Assert.IsTrue(v.IsValid("LT12 1000 0111 0100 1000", null));
			Assert.IsTrue(v.IsValid("LU28 0019 4006 4475 0000 ", null));
			Assert.IsTrue(v.IsValid("MK072 5012 0000 0589 84", null));
			Assert.IsTrue(v.IsValid("MT84 MALT 0110 0001 2345 MTLC AST0 01S", null));
			Assert.IsTrue(v.IsValid("MU17 BOMM 0101 1010 3030 0200 000M UR", null));
			Assert.IsTrue(v.IsValid("MC11 1273 9000 7000 1111 1000 h79", null));
			Assert.IsTrue(v.IsValid("ME25 5050 0001 2345 6789 51", null));
			Assert.IsTrue(v.IsValid("NL91 ABNA 0417 1643 00", null));
			Assert.IsTrue(v.IsValid("NO93 8601 1117 947", null));
			Assert.IsTrue(v.IsValid("PL61 1090 1014 0000 0712 1981 2874 ", null));
			Assert.IsTrue(v.IsValid("PT50 0002 0123 1234 5678 9015 4 ", null));
			Assert.IsTrue(v.IsValid("RO49 AAAA 1B31 0075 9384 0000 ", null));
			Assert.IsTrue(v.IsValid("SM86 U032 2509 8000 0000 0270 100 ", null));
			Assert.IsTrue(v.IsValid("RS35 2600 0560 1001 6113 79", null));
			Assert.IsTrue(v.IsValid("SK31 1200 0000 1987 4263 7541", null));
			Assert.IsTrue(v.IsValid("SI56 1910 0000 0123 438 ", null));
			Assert.IsTrue(v.IsValid("ES91 2100 0418 4502 0005 1332", null));
			Assert.IsTrue(v.IsValid("SE53 5000 0000 0543 9100 1276", null));
			Assert.IsTrue(v.IsValid("CH86 0486 2058 1124 0100 1", null));
			Assert.IsTrue(v.IsValid("TN59 1420 7207 1007 0712 9648", null));
			Assert.IsTrue(v.IsValid("TR33 0006 1005 1978 6457 8413 26", null));
			Assert.IsTrue(v.IsValid("GB29 NWBK 6016 1331 9268 19", null));
			
			//Invalid:
			Assert.IsFalse(v.IsValid("AD12 0001 2030 2003 5910 01005", null));
			Assert.IsFalse(v.IsValid("GB29/NWBK/6016/1331=9268;19", null));
			Assert.IsFalse(v.IsValid("CH39 0076 2d011 6238 5295 7", null));
			Assert.IsFalse(v.IsValid("SE12 1231 2345 6789 0123 4561", null));
			Assert.IsFalse(v.IsValid("NL91 AB5A 0417 1643 01", null));
			Assert.IsFalse(v.IsValid("NO93 8601 1117 9478", null));
			Assert.IsFalse(v.IsValid("PL31 1090 1314 0000 0712 1981 2874 ", null));
			Assert.IsFalse(v.IsValid("PT20 0002 0123 1234 5178 9015 4 ", null));
			Assert.IsFalse(v.IsValid("RO49 AAAA 1B31 0575 9384 0000 ", null));
			Assert.IsFalse(v.IsValid("SM86 U032 2509 8010 0000 0270 100 2", null));
			Assert.IsFalse(v.IsValid("RS15 2600 0560 1001 6113 79 4", null));
			Assert.IsFalse(v.IsValid("ÒMT84 MALT 0110 0001 2345 MTLC AST0 01S", null));
			Assert.IsFalse(v.IsValid("   ", null));
			Assert.IsFalse(v.IsValid(1, null));
		}
	}
}