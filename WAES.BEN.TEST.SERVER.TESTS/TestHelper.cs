using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAES.BEN.TEST.SERVER.TESTS
{
    public static class TestHelper
    {
        /// <summary>
        /// Compares two dictionaries by length and KeyValues.
        /// Throws assert exception if not equal
        /// </summary>
        /// <param name="expectedDictionary"></param>
        /// <param name="actualDictionary"></param>
        public static void CompareDictionaries(Dictionary<int, int> expectedDictionary, Dictionary<int, int> actualDictionary)
        {
            if (actualDictionary == null && expectedDictionary == null)
            {
                return;
            }
            if (actualDictionary == null || expectedDictionary == null)
            {
                Assert.Fail("One of the dictionaries is null");
            }

            Assert.AreEqual(expectedDictionary.Count, actualDictionary.Count,
                string.Format("Expected dictionary count is: {0} .Actual dictionary count is {1}", expectedDictionary.Count, actualDictionary.Count));

            for (int i = 0; i < expectedDictionary.Count; i++)
            {
                Assert.AreEqual(expectedDictionary.ElementAt(i).Key, actualDictionary.ElementAt(i).Key
                    , string.Format("Expected dictionary key at index {0} is {1}, while key in actual dictionary is {2}",
                    i, expectedDictionary.ElementAt(i).Key, expectedDictionary.ElementAt(i).Key, actualDictionary.ElementAt(0).Key));

                Assert.AreEqual(expectedDictionary.ElementAt(i).Value, actualDictionary.ElementAt(i).Value
                   , string.Format("Expected dictionary Value at index {0} is {1}, while Value in actual dictionary is {2}",
                   i, expectedDictionary.ElementAt(i).Value, expectedDictionary.ElementAt(i).Value, actualDictionary.ElementAt(0).Value));
            }
        }

        /// <summary>
        /// Encodes the content of a binary file as base64
        /// </summary>
        /// <param name="fileName">The full path of the file</param>
        /// <returns></returns>
        public static string EncodeFileAsBase64(string fileName)
        {
            try
            {
                var bytes = File.ReadAllBytes(fileName);
                return Convert.ToBase64String(bytes);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
