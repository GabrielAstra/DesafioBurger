using GoodHamburger.API.Middlewares;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Application.Servicos;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Infrastructure.Contexto;
using GoodHamburger.Infrastructure.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("https://localhost:7180", "http://localhost:5136")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? "Data Source=goodhamburger.db";

builder.Services.AddDbContext<GoodHamburgerDbContext>(opt =>
    opt.UseSqlite(connectionString));

builder.Services.AddSingleton<ICardapioRepositorio, CardapioRepositorio>();
builder.Services.AddScoped<IPedidoRepositorio, PedidoRepositorioEf>();

builder.Services.AddScoped<ICardapioServico, CardapioServico>();
builder.Services.AddScoped<IPedidoServico, PedidoServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GoodHamburgerDbContext>();
    db.Database.Migrate();
}

app.UseMiddleware<TratadorDeExcecoesMiddleware>();
app.UseCors("Frontend");

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Good Hamburger API v1"));

app.MapControllers();

app.Run();

public partial class Program { }
