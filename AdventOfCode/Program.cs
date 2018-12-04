using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdventOfCode._2018;

namespace AdventOfCode
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new Day1();
            new Day2();

            Console.WriteLine("\n\n\n...press any key to exit");
            Console.ReadKey();
        }
    }
}
