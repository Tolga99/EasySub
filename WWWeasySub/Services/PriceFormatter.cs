using System.Globalization;

namespace WWWeasySub.Services
{
    public static class PriceFormatter
    {
        public static string Format(decimal amount, string currencyCode)
        {
            var culture = currencyCode switch
            {
                "EUR" => "fr-FR",
                "USD" => "en-US",
                "GBP" => "en-GB",
                "CAD" => "en-CA",
                "AUD" => "en-AU",
                _ => "en-US"
            };

            var cultureInfo = new CultureInfo(culture);

            // Force la devise même si la culture n'est pas parfaitement alignée
            var formatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
            formatInfo.CurrencySymbol = new RegionInfo(culture).CurrencySymbol;

            return string.Format(formatInfo, "{0:C}", amount);
        }
    }

}
