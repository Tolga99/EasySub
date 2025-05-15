using API.Interfaces;
using API.Models;

namespace API.Services
{
    public class CurrencyService : ICurrencyService
    {

        public CurrencyService()
        {
            _selectedCurrency = AvailableCurrencies.Find(a => a.Code == "EUR");
        }
        public List<Currency> AvailableCurrencies { get; } = new()
        {
            new Currency { Code = "USD", Symbol = "$", ConversionRateFromUSD = 1m },
            new Currency { Code = "EUR", Symbol = "€", ConversionRateFromUSD = 0.89m },
            new Currency { Code = "GBP", Symbol = "£", ConversionRateFromUSD = 0.77m },
            new Currency { Code = "CAD", Symbol = "$", ConversionRateFromUSD = 1.40m },
            new Currency { Code = "AUD", Symbol = "$", ConversionRateFromUSD = 1.58m }
        };
        public decimal ConvertFromUSD(decimal amountUSD, string targetCurrencyCode)
        {
            var currency = AvailableCurrencies
                .FirstOrDefault(c => c.Code.Equals(targetCurrencyCode, StringComparison.OrdinalIgnoreCase));

            if (currency == null)
                throw new ArgumentException($"Unsupported currency: {targetCurrencyCode}");

            return amountUSD * currency.ConversionRateFromUSD;
        }
        private Currency _selectedCurrency;

        public Currency SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                if (_selectedCurrency != value)
                {
                    _selectedCurrency = value;
                    OnCurrencyChanged?.Invoke(); // Notifie les composants
                }
            }
        }

        // 🔔 Événement à utiliser dans tes composants pour se mettre à jour automatiquement
        public event Action? OnCurrencyChanged;

        public decimal ConvertFromUSD(decimal usdAmount)
        {
            return usdAmount * SelectedCurrency.ConversionRateFromUSD;
        }

        public string Format(decimal? usdAmountNullable, bool showSymbol = true, bool showCode = false)
        {
            try
            {
                if (usdAmountNullable.HasValue == false)
                {
                    Console.WriteLine("Format : usdAmount is NULL");
                    return "n/a";
                }
                decimal usdAmount = usdAmountNullable.Value;
                var converted = ConvertFromUSD(usdAmount);
                var formatted = converted.ToString("0.00");

                return (showSymbol, showCode) switch
                {
                    (true, true) => $"{formatted} {SelectedCurrency.Symbol} ({SelectedCurrency.Code})",
                    (true, false) => $"{formatted} {SelectedCurrency.Symbol}",
                    (false, true) => $"{formatted} {SelectedCurrency.Code}",
                    _ => formatted
                };
            }
            catch
            {
                Console.WriteLine("CurrencyService-Format : error");
                return "n/a";
            }

        }
    }
}