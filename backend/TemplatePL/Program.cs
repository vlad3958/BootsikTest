using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using TemplateBLL;
using TemplateDAL;
using Templates.BLL.Services;


var builder = WebApplication.CreateBuilder(args);
QuestPDF.Settings.License = LicenseType.Community;


builder.Services.AddDbContext<TemplatesDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TemplatesDb;Trusted_Connection=True;"));


builder.Services.AddScoped<ITemplateRepository, TemplateRepository>();


builder.Services.AddScoped<TemplateService>();
builder.Services.AddScoped<PdfService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
