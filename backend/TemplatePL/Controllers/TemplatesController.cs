using Microsoft.AspNetCore.Mvc;
using TemplateDAL;
using Templates.BLL.Services;
using TemplateBLL;
using Templates.PL.DTOs;


namespace Templates.PL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplatesController : ControllerBase
{
    private readonly TemplateService _templateService;
    private readonly PdfService _pdfService;

    public TemplatesController(TemplateService templateService, PdfService pdfService)
    {
        _templateService = templateService;
        _pdfService = pdfService;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var items = await _templateService.GetAllAsync();
        var response = items.Select(t => new TemplateResponse
        {
            Id = t.Id,
            Name = t.Name,
            HtmlContent = t.HtmlContent,
            CreatedAt = t.CreatedAt
        });
        return Ok(response);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var template = await _templateService.GetByIdAsync(id);
        if (template == null) return NotFound();
        return Ok(new TemplateResponse
        {
            Id = template.Id,
            Name = template.Name,
            HtmlContent = template.HtmlContent,
            CreatedAt = template.CreatedAt
        });
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateTemplateRequest request)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var entity = new Template
        {
            Name = request.Name,
            HtmlContent = request.HtmlContent,
            CreatedAt = DateTime.UtcNow
        };

        await _templateService.AddAsync(entity);

        var response = new TemplateResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            HtmlContent = entity.HtmlContent,
            CreatedAt = entity.CreatedAt
        };
        return CreatedAtAction(nameof(Get), new { id = entity.Id }, response);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTemplateRequest request)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var existing = await _templateService.GetByIdAsync(id);
        if (existing == null) return NotFound();

        existing.Name = request.Name;
        existing.HtmlContent = request.HtmlContent;
        await _templateService.UpdateAsync(existing);

        var response = new TemplateResponse
        {
            Id = existing.Id,
            Name = existing.Name,
            HtmlContent = existing.HtmlContent,
            CreatedAt = existing.CreatedAt
        };
        return Ok(response);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _templateService.GetByIdAsync(id);
        if (existing == null) return NotFound();
        await _templateService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/generate")]
    public async Task<IActionResult> Generate(int id, [FromBody] DTOs.GeneratePdfRequest request)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var template = await _templateService.GetByIdAsync(id);
        if (template == null) return NotFound();

        var pdfBytes = _pdfService.Generate(template.HtmlContent, request.Data);
        return File(pdfBytes, "application/pdf", $"{template.Name}.pdf");
    }
}
