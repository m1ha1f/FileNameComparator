using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileNameComparator;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FileNameComparator.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var comp = new Comparator();
            Assert.IsTrue(comp.Compare("ana", "maria") < 0); 
        }

        [TestMethod]
        public void TestMethod2()
        {
            var comp = new Comparator();
            Assert.IsTrue(comp.Compare("jshajkfhaksf", "jshajkfhaksf") == 0);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var comp = new Comparator();
            Assert.IsTrue(comp.Compare("", "") == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestMethod4()
        {
            var comp = new Comparator();
            Assert.IsTrue(comp.Compare("null", null) == 0);
        }

        [TestMethod]
        public void TestMethod5()
        {
            var comp = new Comparator();
            Assert.IsTrue(comp.Compare("abcd", "abc") > 0);
        }

        [TestMethod]
        public void TestMethod6()
        {
            var comp = new Comparator();
            Assert.IsTrue(comp.Compare("abc", "abcd") < 0);
        }

        [TestMethod]
        public void TestMethod7()
        {
            var comp = new Comparator();
            Assert.IsTrue(comp.Compare("    abc defgh ", "abc " + Environment.NewLine + "defgh        ") == 0);
        }

        [TestMethod]
        public void TestMethod8()
        {
            var comp = new Comparator();
            Assert.IsTrue(comp.Compare("abcdedgh", "abciktf") < 0);
        }

        [TestMethod]
        public void TestMethod9()
        {
            var comp = new Comparator { IgnoreWhiteSpaces = false};
            //space space tab vs space tab
            Assert.IsTrue(comp.Compare("  \tabc defgh ", " \tabc ") > 0);
        }

        [TestMethod]
        public void TestMethod10()
        {
            var comp = new Comparator { IgnoreWhiteSpaces = false };
            
            Assert.IsTrue(comp.Compare("  \tabc\tdefgh ", "  \tabc x") < 0);
        }

        [TestMethod]
        public void TestMethod11()
        {
            var comp = new Comparator { IgnoreWhiteSpaces = false };
            Assert.IsTrue(comp.Compare("", "nsada") < 0);
        }

        [TestMethod]
        public void TestMethod12()
        {
            var comp = new Comparator();
            Assert.IsTrue(comp.Compare("test1.txt", "test10.txt") < 0);
        }

        [TestMethod]
        public void TestMethod13()
        {
            var comp = new Comparator();
            Assert.IsTrue(comp.Compare("test777.txt", "test83.txt") > 0);
        }

        [TestMethod]
        public void TestMethod14()
        {
            var comp = new Comparator();
            Assert.IsTrue(comp.Compare("test1x", "test2") < 0);
        }

        [TestMethod]
        public void TestMethod15()
        {
            //testing sorting of filenames without extension
            var comp = new Comparator();
            var list = new[] { "test", "test1", "testa", "test2", "test1x", "testa1", "testb"};
            Array.Sort(list, (x, y) => comp.Compare(x, y));
            foreach (var s in list)
                Debug.WriteLine(s);
            
            var correct = new[] { "test", "test1", "test1x", "test2", "testa", "testa1", "testb"};
            for (int i = 0; i < list.Length; ++i)
                Assert.AreEqual(list[i], correct[i]);
        }

        [TestMethod]
        public void TestMethod16()
        {
            //testing sorting of filenames with extension
            var comp = new Comparator();
            var list = new[] { "test.txt", "test1.txt", "testa.txt", "test2.txt", "test1x.txt", "testa1.txt", "testb.txt" };
            Array.Sort(list, (x, y) => comp.Compare(x, y));
            foreach (var s in list)
                Debug.WriteLine(s);
            
            var correct = new[] { "test.txt", "test1.txt", "test1x.txt", "test2.txt", "testa.txt", "testa1.txt", "testb.txt" };
            for (int i = 0; i < list.Length; ++i)
                Assert.AreEqual(list[i], correct[i]);
        }


        static Random random = new Random();

        public char GetRandomLetter()
        {
            int num = random.Next(0, 26);
            char letter = (char)('a' + num);
            return letter;
        }

        public char GetRandomDigit()
        {
            int num = random.Next(0, 9);
            char digit = (char)('0' + num);
            return digit;
        }

        private string BuildRandomLettersString(int length)
        {
            var s = new StringBuilder();
            for (int i = 0; i < length; ++i)
                s.Append(GetRandomLetter());
            return s.ToString();
        }

        private string BuildRandomDigitsString(int length)
        {
            var s = new StringBuilder();
            for (int i = 0; i < length; ++i)
                s.Append(GetRandomDigit());
            return s.ToString();
        }

        [TestMethod]
        public void TestMethod17()
        {
            //Maximum test with letters
            var comp = new Comparator();
            var s = BuildRandomLettersString(255);
            Debug.WriteLine(s);
            Assert.IsTrue(comp.Compare(s, s) == 0);
        }

        [TestMethod]
        public void TestMethod18()
        {
            var comp = new Comparator();
            Assert.IsTrue(comp.Compare("1 2 10", "1 10 2") < 0);
        }

        [TestMethod]
        public void TestMethod19()
        {
            var comp = new Comparator();
            Assert.IsTrue(comp.Compare("1/5", "1/20") < 0);
        }

        [TestMethod]
        public void TestMethod20()
        {
            //Maximum test with digits ( to test overflow of all primitive data types)
            var comp = new Comparator();
            var s = BuildRandomDigitsString(255);
            Debug.WriteLine(s);
            Assert.IsTrue(comp.Compare(s, s) == 0);
        }

        [TestMethod]
        public void TestMethod21()
        {
            //testing sorting of extensions
            var comp = new Comparator();
            var list = new[] { "test.8", "test.2", "test.03", "test.4", "test.001", "test.199", "test.000" };
            Array.Sort(list, (x, y) => comp.Compare(x, y));
            foreach (var s in list)
                Debug.WriteLine(s);

            var correct = new[] { "test.000", "test.001", "test.2", "test.03", "test.4", "test.8", "test.199" };
            for (int i = 0; i < list.Length; ++i)
                Assert.AreEqual(list[i], correct[i]);
        }

        [TestMethod]
        public void TestMethod22()
        {
            var comp = new Comparator();
            Assert.IsTrue(comp.Compare("abc22.001", "ab3.002") > 0);
        }

        [TestMethod]
        public void TestMethod23()
        {
            //random test that check lexicographic ordering if case of only letters
            var comp = new Comparator();
            for (int i = 0; i < 100000; ++i)
            {
                var s1 = BuildRandomLettersString(random.Next(255));
                var s2 = BuildRandomLettersString(random.Next(255));

                var ret1 = comp.Compare(s1, s2);

                var ret2 = s1.CompareTo(s2);

                if (ret2 > 0) ret2 = 1;
                if (ret2 < 0) ret2 = -1;

                if (ret1 != ret2)
                {
                    Debug.WriteLine("");
                    Debug.WriteLine(s1); 
                    Debug.WriteLine(s2); 
                    Debug.WriteLine("");
                    Assert.Fail();
                }
            }
        }
        
    }
}
