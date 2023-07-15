namespace EcommerceProject.DTO
{
    public class ProductHomeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Discount { get; set; }
        public double Price { get; set; }
        public int Hot { get; set; }
        public string Photo { get; set; } = string.Empty;
    }
}
