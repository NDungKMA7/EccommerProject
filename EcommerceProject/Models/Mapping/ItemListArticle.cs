using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EcommerceProject.Models.Mapping
{
    [Table("ListArticle")]
    public class ItemListArticle
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
