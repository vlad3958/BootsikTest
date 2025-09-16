using TemplateDAL;

namespace Templates.BLL.Services;

public class TemplateService
{
    private readonly ITemplateRepository _repository;

    public TemplateService(ITemplateRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Template>> GetAllAsync() => _repository.GetAllAsync();
    public Task<Template?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

    public async Task AddAsync(Template template)
    {
        await _repository.AddAsync(template);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateAsync(Template template)
    {
        await _repository.UpdateAsync(template);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }
}
