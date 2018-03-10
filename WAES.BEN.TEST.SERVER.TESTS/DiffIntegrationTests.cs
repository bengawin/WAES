using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WAES.BEN.TEST.SERVER.Controllers;
using WAES.BEN.TEST.SERVER.Logic;
using WAES.BEN.TEST.SERVER.Models;

namespace WAES.BEN.TEST.SERVER.TESTS
{
    [TestClass]
    public class DiffIntegrationTests
    {
        private static V1Controller _controller;
        private static TestFilesProcessor fileProcessor = new TestFilesProcessor();

        [TestInitialize]
        public void InitializeTest()
        {
            _controller = new V1Controller();
        }

        /// <summary>
        /// Compares two result models
        /// </summary>
        /// <param name="expected">The expected ComparisonResultMode</param>
        /// <param name="actual">The actual ComparisonResultMode</param>
        private void CompareResultModels(ComparisonResultModel expected,ComparisonResultModel actual)
        {
            if (expected == null && actual == null)
            {
                return;
            }
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Result, actual.Result);
            TestHelper.CompareDictionaries(expected.Diff, actual.Diff);
            Assert.AreEqual(expected.Info, actual.Info);
        }

        [TestMethod]
        [DataRow("Test1")]
        [DataRow("Test2")]
        public void V1ControllerDiffCompareReturnsOK(string testId)
        {
           var container= fileProcessor.GetTestFiles(testId);
            //loads the files to the processor
           var rightResult = _controller.Diff(testId, "right", container.Right) as TextActionResult;
           var leftResult = _controller.Diff(testId, "left", container.Left) as TextActionResult;
            //compares the files
            var result = _controller.Diff(testId) as TextActionResult;
           
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var comparisonResultModel= JsonConvert.DeserializeObject<ComparisonResultModel>(result.Text);
            Assert.IsNotNull(comparisonResultModel);
            var expectedResultsComparisonModel = JsonConvert.DeserializeObject<ComparisonResultModel>(fileProcessor.ReadExpectedResultsFile(testId));
            CompareResultModels(expectedResultsComparisonModel, comparisonResultModel);
        }

       

        [TestMethod]
        [DataRow("Test1000")]
        public void V1ControllerDiffCatchesArgumentException(string testId)
        {
             var result=_controller.Diff(testId) as TextActionResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
        }

        [TestMethod]
        public void V1ControllerDiffMultipleRequests()
        {
            List<TestCompareContainer> testFiles = fileProcessor.GetAllTestFiles();
            List<ComparisonRequestModel> requestModels = new List<ComparisonRequestModel>();
            testFiles.ForEach(t =>
            {
                requestModels.Add(t.Left);
                requestModels.Add(t.Right);
            });
            //foreach(var request in requestModels)
            Parallel.ForEach(requestModels, (request) =>
            {
                //gets the matching ComparisonRequestModel
                var testContainer = testFiles.First(t => t.Left.Text == request.Text || t.Right.Text == request.Text);
                var side = string.Empty;
                if (string.ReferenceEquals(testContainer.Left.Text, request.Text))
                {
                    side = "left";
                }
                if (string.ReferenceEquals(testContainer.Right.Text, request.Text))
                {
                    side = "right";
                }
                _controller.Diff(testContainer.TestId, side, request);
            });

            //compare the files in the repository
            Parallel.ForEach(testFiles, (comparion) =>
            {
                var result = _controller.Diff(comparion.TestId) as TextActionResult;
                if(result==null)
                {
                    Assert.Fail("Resulting TextActionResult was null");
                }
                var expectedResultsComparisonModel = JsonConvert.DeserializeObject<ComparisonResultModel>(fileProcessor.ReadExpectedResultsFile(comparion.TestId));
                var actualResulComparisonModel= JsonConvert.DeserializeObject<ComparisonResultModel>(result.Text);
                CompareResultModels(expectedResultsComparisonModel, actualResulComparisonModel);
            });
        }
    }
}
