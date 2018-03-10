using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WAES.BEN.TEST.SERVER.Controllers;
using WAES.BEN.TEST.SERVER.Logic;
using WAES.BEN.TEST.SERVER.Models;

namespace WAES.BEN.TEST.SERVER.TESTS
{
    /// <summary>
    /// Checks all loading scenario at the integration level
    /// </summary>
    [TestClass]
    public class LoadingIntegrationTests
    {
        private static V1Controller _controller;

        private static ComparisonRequestModel StubRequestModel = new ComparisonRequestModel() { Text = "TGF1cmVtIElwc3Vt" };
        private static TestFilesProcessor fileProcessor = new TestFilesProcessor();

        [TestInitialize]
        public void InitializeTest()
        {
            _controller = new V1Controller();
        }

        #region private methods

        

       

        #endregion

        /// <summary>
        /// Checks that when ComparisonRequestModel is null the method will return NotAcceptable
        /// </summary>
        [TestMethod]
        public void V1ControllerDiffComparisonRequestModelIsNullReturnsNotAccepted()
        {
          
            var result = _controller.Diff("Test1", "Right", null) as TextActionResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
        }

        /// <summary>
        /// Checks that null or empty test id returns NotAcceptable
        /// </summary>
        /// <param name="testId"></param>
        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        public void V1ControllerDiffTestIdIsNullOrEmptyReturnsNotAccepted(string testId)
        {
            var result = _controller.Diff(testId, "Right", StubRequestModel) as TextActionResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
        }

        /// <summary>
        /// Cheks that invalid side returns NotAcceptable
        /// </summary>
        /// <param name="side"></param>
        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        [DataRow("righ")]
        public void V1ControllerDiffInvalidSideReturnsNotAccepted(string side)
        {
       
            var result = _controller.Diff("Test1", side, StubRequestModel) as TextActionResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
        }

        /// <summary>
        /// Checks that invalid base 64 text returns NotAcceptable
        /// </summary>
        /// <param name="text"></param>
        [TestMethod]
        [DataRow("kaeka")]
        public void V1ControllerDiffInvalidStringReturnsNotaccepted(string text)
        {
            var requestModel = new ComparisonRequestModel
            {
                Text = text
            };
            var result = _controller.Diff("Test1", "Right", requestModel) as TextActionResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
        }

        /// <summary>
        /// Checks that the loading succeeds on both right and left sides
        /// </summary>
        /// <param name="testId"></param>
        [TestMethod]
        [DataRow("Test1")]
        public void V1ControllerDiffLoadingReturnsOk(string testId)
        {
            TestCompareContainer container = fileProcessor.GetTestFiles(testId);
            var rightResult = _controller.Diff(testId, "right", container.Right) as TextActionResult;
            var leftResult = _controller.Diff(testId, "left", container.Left) as TextActionResult;

            Assert.IsNotNull(rightResult);
            Assert.AreEqual(HttpStatusCode.OK, rightResult.StatusCode);

            Assert.IsNotNull(leftResult);
            Assert.AreEqual(HttpStatusCode.OK, leftResult.StatusCode);
        }

       








    }
}
