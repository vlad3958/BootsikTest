using System.ComponentModel.DataAnnotations;

namespace Templates.PL.DTOs;

public class UpdateTemplateRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MinLength(5)]
    public string HtmlContent { get; set; } = string.Empty;
}