namespace TemplateDAL
{
    public class Template
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string HtmlContent { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
