namespace EcommerceProject.DTO
{
    public class InfoNews
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Hot { get; set; }
        public string Photo { get; set; } = string.Empty;

        public string Categories { get; set; }
    }
}
