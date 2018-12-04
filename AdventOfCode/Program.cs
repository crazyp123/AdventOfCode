using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdventOfCode
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var input = Utils.GetInput(2018, 1);

            // new Day1(input.AsListOf<int>());

            new Day2(Utils.GetInput(2018, 2).AsListOf<string>());

            Console.WriteLine("\n\n\n...press any key to exit");
            Console.ReadKey();
        }
    }
}
