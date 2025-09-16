
namespace TemplateDAL;

public interface ITemplateRepository
{
    Task<IEnumerable<Template>> GetAllAsync();
    Task<Template?> GetByIdAsync(int id);
    Task AddAsync(Template template);
    Task UpdateAsync(Template template);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
