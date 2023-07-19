using EcommerceProject.Models.Mapping;

namespace EcommerceProject.DTO
{
    public class ItemOrderAdmin
    {
        public int Id { get; set; }
        public string NameUser { get; set; }
        public string AddressUser { get; set; }
        public int Phone { get; set; }
        public DateTime Create { get; set; }
        public int Status { get; set; }
        public double Price { get; set; }
    }
}
