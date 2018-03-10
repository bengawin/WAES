using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WAES.BEN.TEST.SERVER.Models;
using WAES.BEN.TEST.SERVER.Services;

namespace WAES.BEN.TEST.SERVER.Logic
{
    public class ComparisonUnit
    {
        private readonly IStringComparerService _stringComparerService;

        #region ctor

        //Note: I am making preperation for dependecy injection. 
        //This will allow for a much more flexible system.

        public ComparisonUnit(IStringComparerService stringComparerService)
        {
            _stringComparerService = stringComparerService;
        }
        #endregion

        #region properties

            /// <summary>
            /// Gets or sets the string to compare
            /// </summary>
            public string Right { get; set; }

            /// <summary>
            /// Gets or sets the ref string to compare to
            /// </summary>
            public string Left { get; set; }

        #endregion

        /// <summary>
        /// Compares the Right and Left strings
        /// </summary>
        /// <returns></returns>
        public ComparisonResultModel Compare()
        {
           
            var result = new ComparisonResultModel();
            if (string.IsNullOrEmpty(Right))
            {
                result.Info = "Unable to compare,Right string is missing";
            }
            if (string.IsNullOrEmpty(Left))
            {
                result.Info = "Unable to compare,Left string is missing";
            }

            //check that the strings are equal
            if (_stringComparerService.AreEqual(Right,Left))
            {
                result.Result = StringComparisonResult.Equal;
                result.Info = "The two sides are equal";
                return result;
            }

            //check that the strings are not of equal size
            if(!_stringComparerService.AreOfEqualSize(Right,Left))
            {
                result.Result = StringComparisonResult.NotOfEqualSize;
                result.Info = "The two sides are not of equal size";
            }
            else
            {
                result.Result = StringComparisonResult.EqualSize;
                //check for differences in the strings, if any
                var diff = _stringComparerService.FindDiffinEqualSizeStrings(Right, Left);
                result.Info = "The two sides are of equal size";
                if (diff!=null && diff.Any())
                {
                    result.Diff = diff;
                    result.Info = result.Info+=", but some differences in content were detected.";
                }
            }
            return result;
        }
    }
}