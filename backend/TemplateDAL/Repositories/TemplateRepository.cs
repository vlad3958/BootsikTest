using Microsoft.EntityFrameworkCore;

namespace TemplateDAL;

public class TemplateRepository : ITemplateRepository
{
    private readonly TemplatesDbContext _context;

    public TemplateRepository(TemplatesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Template>> GetAllAsync() =>
        await _context.Templates.ToListAsync();

    public async Task<Template?> GetByIdAsync(int id) =>
        await _context.Templates.FindAsync(id);

    public async Task AddAsync(Template template) =>
        await _context.Templates.AddAsync(template);

    public Task UpdateAsync(Template template)
    {
        _context.Templates.Update(template);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Templates.FindAsync(id);
        if (entity != null)
            _context.Templates.Remove(entity);
    }

   public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
