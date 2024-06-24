using Microsoft.EntityFrameworkCore;
using U3Api.Repositories;
using U3API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("Img", client =>
{
    client.BaseAddress = new Uri("https://sga.api.labsystec.net/");
});

string? cadena = builder.Configuration.GetConnectionString("ActividadesConnectionStrings");
builder.Services.AddDbContext<LabsysteDoubledContext>(optionsBuilder =>
optionsBuilder.UseMySql(cadena, ServerVersion.AutoDetect(cadena)), ServiceLifetime.Transient);

builder.Services.AddTransient<DepartamentoRepository>();
builder.Services.AddTransient<ActividadRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
