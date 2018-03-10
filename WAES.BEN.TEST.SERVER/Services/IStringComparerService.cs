using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAES.BEN.TEST.SERVER.Services
{
    public interface IStringComparerService
    {

        /// <summary>
        /// Checks that the string has a valid format
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        bool IsValidString(string str);

        /// <summary>
        /// Checks if two strings are equal in both sequence and content
        /// </summary>
        /// <param name="str1">The first string to compare</param>
        /// <param name="str2">The second string to compare</param>
        /// <returns></returns>
        bool AreEqual(string str1, string str2);

        /// <summary>
        /// Checks that the two  strings are of equal size only
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        bool AreOfEqualSize(string str1, string str2);

        /// <summary>
        /// Gets a list of indexes in which one string differs another
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns>A dictionary in which The key is the offset of the change and the value is the length of the change</returns>
        Dictionary<int,int> FindDiffinEqualSizeStrings(string str1, string str2);


    }
}
