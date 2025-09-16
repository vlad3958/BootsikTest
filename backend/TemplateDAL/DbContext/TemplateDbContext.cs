using Microsoft.EntityFrameworkCore;

namespace TemplateDAL;

public class TemplatesDbContext : DbContext
{
    public TemplatesDbContext(DbContextOptions<TemplatesDbContext> options) : base(options) { }

    public DbSet<Template> Templates { get; set; }
}
