namespace WWWeasySub.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public byte[]? ImageData { get; set; }
        public bool? Enabled { get; set; }
    }
}
