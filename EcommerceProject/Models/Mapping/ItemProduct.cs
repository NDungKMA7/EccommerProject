using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models.Mapping
{
    [Table("Products")]
    public class ItemProduct
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty; 
        public double Discount { get; set; }
        public double Price { get; set; }
        public int Hot { get; set; }
        public string Photo { get; set; } = string.Empty;
        public string ImgSub { get; set; } = "";
    }
}
