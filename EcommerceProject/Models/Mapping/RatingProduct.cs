using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EcommerceProject.Models.Mapping
{
    [Table("Rating")]
    public class RatingProduct
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Star { get; set; }
    }
}
