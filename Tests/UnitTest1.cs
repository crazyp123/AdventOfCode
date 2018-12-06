using System;
using AdventOfCode._2018;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var x = new Day5();

            Assert.IsTrue(x.IsOpposite('A', 'a'));
            Assert.IsTrue(x.IsOpposite('a', 'A'));

            Assert.IsFalse(x.IsOpposite('A', 'A'));
            Assert.IsFalse(x.IsOpposite('a', 'a'));
            Assert.IsFalse(x.IsOpposite('c', 'A'));
            Assert.IsFalse(x.IsOpposite('X', 'f'));
        }

        public void Test2()
        {

        }
    }
}
