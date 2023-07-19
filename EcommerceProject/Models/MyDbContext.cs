using EcommerceProject.Models.Mapping;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Templating;

namespace EcommerceProject.Models
{
    public class MyDbContext: IdentityDbContext<ApplicationUser>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
      
        public DbSet<ItemCategory> Categories { get; set; }
        public DbSet<ItemProduct> Products { get; set; }
        public DbSet<ItemCategoriesProducts> CategoriesProducts { get; set; }
        public DbSet<ItemTag> Tags { get; set; }
        public DbSet<ItemTagsProducts> TagsProducts { get; set; }
        public DbSet<ItemNew> News { get; set; }
        public DbSet<ItemAdv> Adv { get; set; }
        public DbSet<ItemSlide> Slides { get; set; }
        public DbSet<ItemListArticle> ListArticle { get; set; }
        public DbSet<ItemOrder> Orders { get; set; }
        public DbSet<ItemOrderDetail> OrdersDetail { get; set; }
    }
}
