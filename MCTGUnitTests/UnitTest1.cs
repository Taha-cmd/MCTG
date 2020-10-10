using NUnit.Framework;

namespace MCTGUnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void test()
        {
            Assert.AreEqual(1, 2);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}