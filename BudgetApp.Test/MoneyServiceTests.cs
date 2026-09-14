using Moq;
using BudgetApp.Data.Services;

namespace BudgetAPP.Test
{
    [TestClass]
    public sealed class MoneyServiceTests
    {
        
        [TestMethod]
        public void TestConvertToCents()
        {
            MoneyService moneyService = new MoneyService();
            int expected = 250;
            int actual = moneyService.ConvertToCents(2.50m);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestConvertToDollars()
        {
            var moneyService = new MoneyService();
            decimal expected = 2.50m;
            decimal actual = moneyService.ConvertToDollars(250);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestFormatCurrency()
        {
            var moneyService = new MoneyService();
            string expected = "$2.50";
            string actual = moneyService.FormatCurrency(2.50m);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestFormatCurrency_WithSymbol()
        {
            var moneyService = new MoneyService();
            string expected = "€2.50";
            string symbol = "€";
            string actual = moneyService.FormatCurrency(2.50m, symbol);
            Assert.AreEqual(expected, actual);
        }
    }
}
