using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WAES.BEN.TEST.SERVER.Models;

namespace WAES.BEN.TEST.SERVER.TESTS
{
    public class TestFilesProcessor
    {
        public string GetTestFilesFolderPath()
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            string callingAssemblyName = callingAssembly.GetName().Name;
            string codeBase = callingAssembly.CodeBase.Replace("file:///", string.Empty);
            int assemblyPathIndex = codeBase.IndexOf(callingAssemblyName);
            string assemblyPath = codeBase.Substring(0, assemblyPathIndex + callingAssemblyName.Length);
            return assemblyPath + "/TestFiles";
        }

        /// <summary>
        /// Loads the 2 test files byte array as Base64encoded.
        /// </summary>
        /// <param name="testId">The test id which servers as the folder in which the two files to compare reside</param>
        /// <returns></returns>
        public TestCompareContainer GetTestFiles(string testId)
        {
            string TestFolderPath = GetTestFilesFolderPath() + "/" + testId ;
            var dirInfo = new DirectoryInfo(TestFolderPath);
            string rightFileName=string.Empty;
            string leftFileName = string.Empty;
            foreach (var file in dirInfo.GetFiles())
            {
                if(file.Name.ToLower().Contains("-right."))
                {
                    rightFileName = file.FullName;
                }
                if(file.Name.ToLower().Contains("-left."))
                {
                    leftFileName = file.FullName;
                }
            }
            //string rightFileName = (TestFolderPath + "/" + testId + "/" + testId + "-Right.txt").Replace("/", "\\");
           // string leftFileName = (TestFolderPath + "/" + testId + "/" + testId + "-Left.txt").Replace("/", "\\");
            string base64RightFileEncoded;
            string base64LeftFileEncoded;
            base64RightFileEncoded = TestHelper.EncodeFileAsBase64(rightFileName);
            base64LeftFileEncoded = TestHelper.EncodeFileAsBase64(leftFileName);
            return new TestCompareContainer
                (
                    testId,
                    new ComparisonRequestModel { Text = base64RightFileEncoded },
                    new ComparisonRequestModel { Text = base64LeftFileEncoded }
                );
        }

        /// <summary>
        /// Gets all test files from all test folders
        /// </summary>
        /// <returns></returns>
        public List<TestCompareContainer> GetAllTestFiles()
        {
            string testFolderPath = GetTestFilesFolderPath();
            var dirInfo = new DirectoryInfo(testFolderPath);
            var comparisonList = new List<TestCompareContainer>();
            foreach(var dir in dirInfo.GetDirectories())
            {
                comparisonList.Add(GetTestFiles(dir.Name));
            }
            return comparisonList;
        }

        /// <summary>
        /// Loads and reads the content of the results file.
        /// The result file contains the expected results
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        public string ReadExpectedResultsFile(string testId)
        {
            string TestFolderPath = GetTestFilesFolderPath();
            string fileName = TestFolderPath + "/" + testId + "/" + testId + "-Results.json";
            return File.ReadAllText(fileName);
        }
    }
}
