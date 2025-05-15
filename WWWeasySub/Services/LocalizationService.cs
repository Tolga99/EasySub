using System.Globalization;
using WWWeasySub.Interfaces;

namespace WWWeasySub.Services
{
    public class LocalizationService : ILocalizationService
    {
        public CultureInfo Culture { get; set; } = new CultureInfo("en-US");
        public string CurrencySymbol { get; set; } = "$";

        public void SetCulture(string languageCode)
        {
            Culture = new CultureInfo(languageCode);
            CultureInfo.DefaultThreadCurrentCulture = Culture;
            CultureInfo.DefaultThreadCurrentUICulture = Culture;
        }
    }
}
