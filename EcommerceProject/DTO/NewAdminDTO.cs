namespace EcommerceProject.DTO
{
    public class NewAdminDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Hot { get; set; }
        public string Photo { get; set; } = string.Empty;

        public string Categories { get; set; }
    }
}
