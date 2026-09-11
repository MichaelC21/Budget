namespace BudgetApp.Business.Interfaces
{
    public interface IMoneyService
    {
        int ConvertToCents(decimal amount);
        decimal ConvertToDollars(int cents);
        string FormatCurrency(decimal amount, string currencySymbol = "$");
    }
}
