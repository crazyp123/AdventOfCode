using AoC.y2019;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Day1()
        {
            var x = new Day1();
        }

        [Test]
        public void Day2_1()
        {
            var x = new Day2();
            x.Part1();
        }

        [Test]
        public void Day2_2()
        {
            var x = new Day2();
            x.Part2();
        }
    }
}