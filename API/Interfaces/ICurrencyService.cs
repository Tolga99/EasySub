namespace API.Interfaces
{
    public interface ICurrencyService
    {
        decimal ConvertFromUSD(decimal usdAmount);
        decimal ConvertFromUSD(decimal amountUSD, string targetCurrencyCode);
        string Format(decimal? usdAmountNullable, bool showSymbol = true, bool showCode = false);
    }
}
