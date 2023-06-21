using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models.Mapping
{
    [Table("Categories")]
    public class ItemCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public int DisplayHomePage { get; set; }
    }
}
