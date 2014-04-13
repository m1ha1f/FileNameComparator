using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileNameComparator
{
    public class Comparator
    {
        public bool IgnoreWhiteSpaces = true;
        public bool TestForExtension = true;

        /// <summary>
        /// Compares strings s1 and s2 and orders them in an intuitive manner
        /// It keeps track of the presence of numbers in the string and comperes them as such (not as strings)
        /// Can ignore whitespaces if the IgnoreWhiteSpaces property is true(this is the default value)
        /// Can treat the strings as filenames including extension and compare the names(without extension) and the extension separately. 
        /// (The TestForExtension property is true by default).
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public int Compare(string s1, string s2)
        {
            //If the string are filenames(with extension) we first compare the names without the extension and in case of equality we compare the extensions
            if (TestForExtension && s1.LastIndexOf(".") != -1 && s2.LastIndexOf(".") != -1)
            {
                int p1 = s1.LastIndexOf(".");
                int p2 = s2.LastIndexOf(".");
                int ret = Compare(s1, 0, p1, s2, 0, p2);
                
                return ret != 0 ? ret : Compare(s1, p1+1, s1.Length, s2, p2+1, s2.Length); 
            }
            //Otherwise just compare the strings directly
            return Compare(s1, 0, s1.Length, s2, 0, s2.Length);
        }

        /// <summary>
        /// Does the same thing as Compare(s1, s2) but compares only the 
        /// substrings starting from position start to position stop (excluding stop) for each string
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="start1">The start position in s1</param>
        /// <param name="stop1">The stop position in s1</param>
        /// <param name="s2"></param>
        /// <param name="start2">The start position in s2</param>
        /// <param name="stop2">The stop position in s2</param>
        /// <returns></returns>
        public int Compare(string s1, int start1, int stop1, string s2, int start2, int stop2)
        {
            int i1 = start1, i2 = start2;
            int start_no1 = -1, end_no1 = 0, start_no2 = -1, end_no2 = 0; //the start and end position of the current numbers in the strings 
            while (i1 < stop1 || i2 < stop2)
            {
                //Ignore whitespaces
                if (IgnoreWhiteSpaces && i1 < stop1 && char.IsWhiteSpace(s1[i1])) { ++i1; continue; }
                if (IgnoreWhiteSpaces && i2 < stop2 && char.IsWhiteSpace(s2[i2])) { ++i2; continue; }

                //Numbers
                if (i1 < stop1 && char.IsDigit(s1[i1]))
                {
                    start_no1 = i1;
                    while (i1 < stop1 && char.IsDigit(s1[i1])) ++i1;
                    end_no1 = i1 - 1;
                }
                if (i2 < stop2 && char.IsDigit(s2[i2]))
                {
                    start_no2 = i2;
                    while (i2 < stop2 && char.IsDigit(s2[i2])) ++i2;
                    end_no2 = i2 - 1;
                }

                //Other characters or the end of one the strings
                if (start_no1 != -1) //if we just finished "parsing" a number in s1
                    if (start_no2 != -1) //if we just finished "parsing" a number in s2
                    {
                        int ret = CompareNumbers(s1, start_no1, end_no1, s2, start_no2, end_no2);
                        if (ret != 0)
                            return ret;
                        //the numbers were equal, so we reset the pointers
                        start_no1 = start_no2 = -1;
                    }
                    else
                        return i2 < stop2 ? -1 : 1; //We consider a number smaller than any other character, but bigger than the empty string
                else
                    if (start_no2 != -1)
                        return i1 < stop1 ? 1 : -1;
                    else
                    {
                        if (i1 < stop1 && i2 < stop2)
                        {
                            if (s1[i1] == s2[i2]) { ++i1; ++i2; continue; }
                            return s1[i1] < s2[i2] ? -1 : 1;
                        }
                        return i1 == stop1 ? -1 : 1; //we reached the end of exactly one of the strings
                    }
            }

            return 0;
        }

        private int CompareNumbers(string s1, int start_no1, int end_no1, string s2, int start_no2, int end_no2)
        {
            Debug.Assert(start_no1 >= 0 && start_no1 <= end_no1 &&
                         start_no2 >= 0 && start_no2 <= end_no2 &&
                         end_no1 < s1.Length && end_no2 < s2.Length);
            //Ignore leading zeroes
            while (start_no1 < end_no1 && s1[start_no1] == '0') ++start_no1;
            while (start_no2 < end_no2 && s2[start_no2] == '0') ++start_no2;
            //Compare lengths
            if (end_no1 - start_no1 < end_no2 - start_no2)
                return -1;
            if (end_no1 - start_no1 > end_no2 - start_no2)
                return 1;
            //Now the lengths are equal
            while (start_no1 <= end_no1 && s1[start_no1] == s2[start_no2])
            {
                ++start_no1;
                ++start_no2;
            }
            if (start_no1 <= end_no1) //if we found a mismatch
                return s1[start_no1] < s2[start_no2] ? -1 : 1;
            return 0;
        }
    }
}
