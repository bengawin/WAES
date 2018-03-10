using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WAES.BEN.TEST.SERVER.Services
{
    public class Base64StringComparerService : IStringComparerService
    {
        public bool AreEqual(string str1, string str2)
        {
            return str1.CompareTo(str2) == 0;
        }

        public bool AreOfEqualSize(string str1, string str2)
        {
            return str1.Length == str2.Length;
        }

        public Dictionary<int,int> FindDiffinEqualSizeStrings(string str1, string str2)
        {
            if (str1 == null || str2 == null)
            {
                throw new ArgumentException("One or more of the strings is null");
            }
            if (!AreOfEqualSize(str1,str2))
            {
                throw new ArgumentException("The two Base64 strings must be the same size");
            }
            var diffs = new Dictionary<int, int>();
            var changeLength = 0;
            for(var i=1;i<=str1.Length;i++)
            {
               
                bool isDifferent = str1[i-1] != str2[i-1];
                bool isNextDifferent = (i < str1.Length && str1[i] != str2[i]);
                if (isDifferent)
                {
                    changeLength++;
                    if(!isNextDifferent)
                    {
                        diffs.Add(i-changeLength, changeLength);
                        changeLength = 0;
                    }
                }
                
                //else if()
                //{
                //    if (changeLength > 0)
                //    {
                //        diffs.Add(diffOffset, changeLength);
                //        changeLength = 0;
                //    }
                //    diffOffset = -1;
                //}              
            }
            return diffs;
        }

        public bool IsValidString(string str)
        {
            //If the string is null or has a length which is dividable by 4or contains white spaces or contains line break etc
            //it is not valid see https://msdn.microsoft.com/en-us/library/system.convert.frombase64string(v=vs.110).aspx
            if (str == null || 
                str.Length == 0 || 
                str.Length % 4 != 0
       ||       str.Contains(" ") || 
                str.Contains("\t") || 
                str.Contains("\r") || 
                str.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(str);
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}