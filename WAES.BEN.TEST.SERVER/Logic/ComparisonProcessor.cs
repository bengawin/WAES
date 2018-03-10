using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WAES.BEN.TEST.SERVER.Models;
using WAES.BEN.TEST.SERVER.Services;

namespace WAES.BEN.TEST.SERVER.Logic
{
    public enum ComparisonSide
    {
        Right,
        Left
    }

    /// <summary>
    /// Manages the comparison of all the requests
    /// </summary>
    public class ComparisonProcessor
    {
        /// <summary>
        /// Retains a list of previus and pending comparisons
        /// </summary>
        private readonly Dictionary<string, ComparisonUnit> ComparisonRepository = new Dictionary<string, ComparisonUnit>();

        private readonly IStringComparerService _stringCoparerService;

        public ComparisonProcessor(IStringComparerService stringCoparerService)
        {
            _stringCoparerService = stringCoparerService;
        }


        private static readonly object LockObj = new object();

        /// <summary>
        /// Creates a new comparison item and add it to the CopmparisonRepository
        /// </summary>
        /// <param name="id">The comparison unit Id</param>
        /// <param name="comparsionSide">The side of the first string to compare</param>
        /// <param name="str">The string to compare</param>
        private void CreateNewComparisonUnit(string id, ComparisonSide comparsionSide, string str)
        {
            lock (LockObj)//for thread saftey
            {
                var comparisonUnit = new ComparisonUnit(_stringCoparerService);
                //Adds the string to the proper side
                AssignString(comparisonUnit, comparsionSide, str);
                ComparisonRepository.Add(id, comparisonUnit);
            }
        }

        private void AssignString(ComparisonUnit comparisonUnit, ComparisonSide comparsionSide, string str)
        {
            switch (comparsionSide)
            {
                case ComparisonSide.Right:
                    comparisonUnit.Right = str;
                    break;
                case ComparisonSide.Left:
                    comparisonUnit.Left = str;
                    break;
            }
        }

        private bool IsValidString(string str,out string message)
        {
            if (!_stringCoparerService.IsValidString(str))
            {
                message ="String is not valid";
                return false;
            }
            message = null;
            return true;
        }

        /// <summary>
        /// Creates a new comparison item and add it to the CopmparisonRepository
        /// </summary>
        /// <param name="id">The comparison unit Id</param>
        /// <param name="comparsionSide">The side of the first string to compare</param>
        /// <param name="str">The string to compare</param>
        public bool LoadString(string id, ComparisonSide comparsionSide, string str,out string message)
        {
            //check the string validity:
            if(!IsValidString(str,out message))
            {
                message = "String is not valid";
                return false;
            }
            lock(LockObj)
            {
                if (ComparisonRepository.ContainsKey(id))
                {
                    //places the string in the correct side of the unit
                    var comparisonUnit = ComparisonRepository[id];
                    AssignString(comparisonUnit, comparsionSide, str);
                    return true;
                }
                else
                {
                    //creates a new comparison unit in the repository
                    CreateNewComparisonUnit(id, comparsionSide, str);
                    return true;
                }
            }
            
        }

        /// <summary>
        /// Compares two string of a specific comparisonUnit
        /// </summary>
        /// <param name="unitId">The Id of the two strings in the repository to comapre</param>
        /// <returns></returns>
        public ComparisonResultModel Compare(string unitId)
        {
            if (ComparisonRepository.ContainsKey(unitId))
            {
                lock (LockObj)
                {
                    var comparisonUnit = ComparisonRepository[unitId];
                    var result= comparisonUnit.Compare();
                    //I am assuming that we will want to remove the comprison unit if the comparison completed. 
                    //This is to avoid the repository from  overloading the memory. (TBD)
                    if(result.Result!=StringComparisonResult.Unknown)
                    {
                        ComparisonRepository.Remove(unitId);
                    }
                    return result;
                }
            }
            else
            {
                throw new ArgumentException(string.Format("There is no id {0} in the comparison repository", unitId));
            }
        }

       
    }
}
