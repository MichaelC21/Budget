namespace BudgetAPP.Test
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestAdd()
        {
            int expected = 6;
            int actual = 2 + 3;
            Assert.AreEqual(expected, actual);
        }
    }
}
