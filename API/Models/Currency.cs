namespace API.Models
{
    public class Currency
    {
        public string Code { get; set; } = "";
        public string Symbol { get; set; } = "";
        public decimal ConversionRateFromUSD { get; set; }

        public override string ToString() => $"{Code} - {Symbol}";
    }
}
