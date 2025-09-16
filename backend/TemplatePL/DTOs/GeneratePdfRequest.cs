using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Templates.PL.DTOs;

public class GeneratePdfRequest
{
    [Required]
    public Dictionary<string, string> Data { get; set; } = new();
    public static bool IsValidJson(string json)
    {
        try
        {
            JsonDocument.Parse(json);
            return true;
        }
        catch { return false; }
    }
}