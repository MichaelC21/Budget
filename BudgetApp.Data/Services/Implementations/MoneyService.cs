using BudgetApp.Business.Interfaces;

namespace BudgetApp.Data.Services
{
    public class MoneyService : IMoneyService
    {
        // Ignoring currencies that dont have cents (for now) and assuming that all currencies have 100 cents in a dollar
        public int ConvertToCents(decimal amount)
        {
            return (int)(amount * 100);
        }

        public decimal ConvertToDollars(int cents)
        {
            return Math.Round(cents / 100m, 2);
        }

        public string FormatCurrency(decimal amount, string currencySymbol = "$")
        {
            return $"{currencySymbol}{amount:N2}";
        }
    }
}
