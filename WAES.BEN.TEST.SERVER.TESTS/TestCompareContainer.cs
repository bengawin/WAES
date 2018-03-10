using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAES.BEN.TEST.SERVER.Models;

namespace WAES.BEN.TEST.SERVER.TESTS
{
    /// <summary>
    /// This class contains the two base 64 sides which needs to be compared
    /// </summary>
    public struct TestCompareContainer
    {
        #region ctor

        public TestCompareContainer(string testId,ComparisonRequestModel right, ComparisonRequestModel left)
        {
            TestId = testId;
            Right = right;
            Left = left;
        }
        #endregion

        #region class properties

            /// <summary>
            /// Gets the unique id of the test
            /// </summary>
            public string TestId { get; private set; }

            /// <summary>
            /// Gets the right side of the comparison
            /// </summary>
            public ComparisonRequestModel Right { get; private set; }

            /// <summary>
            /// Gets the left side of the comapriosn
            /// </summary>
            public ComparisonRequestModel Left { get; private set; }
        #endregion
    }
}
