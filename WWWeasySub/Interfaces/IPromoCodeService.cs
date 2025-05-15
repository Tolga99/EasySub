using WWWeasySub.Services;

namespace WWWeasySub.Interfaces
{
    public interface IPromoCodeService
    {
        Task<PromoCodeResult?> ValidateCodeAsync(string code);
    }
}
