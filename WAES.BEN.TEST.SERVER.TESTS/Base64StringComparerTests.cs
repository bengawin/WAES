using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAES.BEN.TEST.SERVER.Services;

namespace WAES.BEN.TEST.SERVER.TESTS
{
    [TestClass]
    public class Base64StringComparerTests
    {
        private static IStringComparerService _comparer;

        private static readonly List<Dictionary<int, int>> ExpectedDiifs = new List<Dictionary<int, int>>
        {
            new Dictionary<int, int>{ { 2, 2 } },// diff is at index 2 and length is 2
            new Dictionary<int, int>{ { 0,1} },//diff is at index 0 and length is 1
            new Dictionary<int, int>{ { 0,3} },//diff is at index 0 and length is 3
            new Dictionary<int, int>
            {
                { 7,1},//diff is at index 7 and length 1
                {14,2 }//diff is at index 14 and length is 2
            },
            new Dictionary<int, int>{ { 0,16} },//diff is at index 0 and length is 16
            new Dictionary<int, int>
            {
                {0,3},//diff is at index 0 and length 3
                {5,3 },//diff is at index 5 and length is 3
                {9,1 },//diff is at index 9 and length is 1
                {15,1 }//diff is at index 15 and length is 1
            }
        };

        [ClassInitialize]
        public static void InitializeTest(TestContext testContext)
        {
            _comparer = new Base64StringComparerService();
        }

        [TestMethod]
        [DataRow("SGVsbG8=")]
        [DataRow("TXkgbmFtZSBpcyBCZW4NCndoYXQgaXMgeW91cnM/DQo=")]
        public void Base64StringComparerIsValidReturnsTrue(string base64String)
        {
            Assert.IsTrue(_comparer.IsValidString(base64String), string.Format("{0} is not a valid base64 string", base64String));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("TXkgbmFtZSBp cyBCZW4NCndoYXQgaXMgeW91cnM/DQo=")]
        [DataRow("TXkgbmFtZSBpcyBCZW4NCndoY\nXQgaXMgeW91cnM/DQo=")]
        [DataRow("TXkgbmFtZSBpcyBCZW4NCndoYXQgaXMgeW91cnM/D\tQo=")]
        [DataRow("T\rXkgbmFtZSBpcyBCZW4NCndoYXQgaXMgeW91cnM/DQo=")]
        [DataRow("T\rXkgbmFtZSBpcyBCZW4NCndoYXQgaXMgeW91cnM/DQo=")]
        [DataRow("SGVbG8=")]
        public void Base64StringComparerIsNotValidReturnsFalse(string base64String)
        {
            Assert.IsFalse(_comparer.IsValidString(base64String), "Expected a non valid base64 string");
        }

        [TestMethod]
        [DataRow("R29vZCBOaWdodA==", "R29vZCBOaWdodA==")]
        public void Base64StringComparerAreEqualReturnTrue(string str1, string str2)
        {
            Assert.IsTrue(_comparer.AreEqual(str1, str2), string.Format("strings are not equal str1 is {0} and str2 is {1}", str1, str2));
        }

        [TestMethod]
        [DataRow("R29vZCBOaWdodA==", "R29vZCBuaWdodA==")]
        [DataRow("R29vZCBOaWdodA==", "r29vZCBuaWdodA==")]
        [DataRow("R29vZCBOaWdodA==", "T29vZCBuaWdodA==")]
        public void Base64StringComparerAreNotEqualReturnFalse(string str1, string str2)
        {
            Assert.IsFalse(_comparer.AreEqual(str1, str2), "Expected the two strings not to be equal");
        }

        [TestMethod]
        [DataRow("R29vZCBOaWdodA==", "R29vZCBOaWdodA==")]
        public void Base64StringComparerAreOfEqualSizeReturnsTrue(string str1, string str2)
        {
            Assert.IsTrue(_comparer.AreOfEqualSize(str1, str2), string.Format("strings are not equal in size: str1  size is {0} and str2  size is {1}", str1.Length, str2.Length));
        }


        [TestMethod]
        [DataRow("R29vZCBOaWdodA==", null)]
        [DataRow(null, "R29vZCBOaWdodA==")]
        [DataRow("R29vZCBOaWdodA=", "R29vZCBOaWdodA==")]
        public void Base64StringComparerFindDiffinEqualSizeStringThrowsArgumentExceptions(string str1, string str2)
        {
            var parameters = new object[] { str1, str2 };
            Assert.ThrowsException<ArgumentException>(() =>
            {
                _comparer.FindDiffinEqualSizeStrings(str1, str2);

            }, string.Empty, null);
        }

        /// <summary>
        /// Tests that two matching string will return no diffs
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        [TestMethod]
        [DataRow("R29vZCBOaWdodA==", "R29vZCBOaWdodA==")]
        public void Base64StringComparerFindDiffinEqualSizeStringReturnsNoDiff(string str1, string str2)
        {
            Assert.IsFalse(_comparer.FindDiffinEqualSizeStrings(str1, str2).Any());
        }

       
        /// <summary>
        /// Tests that two non matching string will return diffs
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        [TestMethod]
        [DataRow("R29vZCBOaWdodA==", "R28VZCBOaWdodA==", 0)]
        [DataRow("R29vZCBOaWdodA==", "r29vZCBOaWdodA==", 1)]
        [DataRow("R29vZCBOaWdodA==", "r30vZCBOaWdodA==", 2)]
        [DataRow("R29vZCBOaWdodA==", "R29vZCBuaWdodABB", 3)]
        [DataRow("R29vZCBOaWdodA==", "r30VzcboAwDODa11", 4)]
        [DataRow("R29vZCBOaWdodA==", "r30vZcboawdodA=0", 5)]
        public void Base64StringComparerFindDiffinEqualSizeStringReturnsDiff(string str1, string str2,int caseId)
        {
            var expectedDictionary = ExpectedDiifs.ElementAt(caseId);
            var actualDictionary=_comparer.FindDiffinEqualSizeStrings(str1, str2);
            TestHelper.CompareDictionaries(expectedDictionary, actualDictionary);
        }
    }
}
