using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceProject.Models.Mapping
{
    [Table("Adv")]
    public class ItemAdv
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public int Position { get; set; }
    }
}
