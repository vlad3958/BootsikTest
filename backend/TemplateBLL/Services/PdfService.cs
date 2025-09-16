using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace TemplateBLL;

public class PdfService
{
    public byte[] Generate(string html, Dictionary<string, string> data)
    {
        foreach (var item in data)
            html = html.Replace("{{" + item.Key + "}}", item.Value);

        var pdf = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Content().Element(container =>
                {
                    container.Text(html); // Replacing the non-existent Html() method with Text() to render the content
                });
            });
        });

        return pdf.GeneratePdf();
    }
}
