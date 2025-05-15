using System.Globalization;

namespace WWWeasySub.Interfaces
{
    public interface ILocalizationService
    {
        string CurrencySymbol { get; set; }
        CultureInfo Culture { get; set; }

        void SetCulture(string languageCode);
    }
}
