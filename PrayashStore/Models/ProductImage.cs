namespace PrayashStore.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public int ProductId { get; set; }

    }
}