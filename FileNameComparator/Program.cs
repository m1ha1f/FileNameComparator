using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileNameComparator
{
    class Program
    {
        static void Main(string[] args)
        {
            var comp = new Comparator();
            while (true)
            {
                Console.WriteLine("Type the first string: ");
                var s1 = Console.ReadLine();
                Console.WriteLine("Type the second string: ");
                var s2 = Console.ReadLine();
                Console.WriteLine("The result of the comparison is: " + comp.Compare(s1, s2));
            }
        }
    }
}
