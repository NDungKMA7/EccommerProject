using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EcommerceProject.Models.Mapping
{
    [Table("Orders")]
    public class ItemOrder
    {
        [Key]
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public DateTime Create { get; set; }
        public int Status { get; set; }
        public double Price { get; set; }
    }
}
