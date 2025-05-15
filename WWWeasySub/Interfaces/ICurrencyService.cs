using WWWeasySub.Models;

namespace WWWeasySub.Interfaces
{
    public interface ICurrencyService
    {
        List<Currency> AvailableCurrencies { get; }

        Currency SelectedCurrency { get; set; }

        event Action? OnCurrencyChanged;

        decimal ConvertFromUSD(decimal usdAmount);

        string Format(decimal? usdAmountNullable, bool showSymbol = true, bool showCode = false);
    }
}
