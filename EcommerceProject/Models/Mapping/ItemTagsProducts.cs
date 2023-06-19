using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models.Mapping
{
    [Table("TagsProducts")]
    public class ItemTagsProducts
    {
        [Key]
        public int Id { get; set; }
        public int TagId { get; set; }
        public int ProductId { get; set; }
    }
}
