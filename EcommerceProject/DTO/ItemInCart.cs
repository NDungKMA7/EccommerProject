using EcommerceProject.Models.Mapping;

namespace EcommerceProject.DTO
{
    public class ItemInCart
    {
        public ItemProduct ProductRecord { get; set; }
        
        public int Quantity { get; set; }
    }
}
