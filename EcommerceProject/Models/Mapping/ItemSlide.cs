using System.ComponentModel.DataAnnotations;

namespace EcommerceProject.Models.Mapping
{
    public class ItemSlide
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
    }
}
