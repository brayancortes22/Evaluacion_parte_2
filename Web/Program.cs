using Microsoft.EntityFrameworkCore;
using Entity.Contexts;
using Business;
using Data;
using Business.Interfaces;
using Business.Implementations;
using Data.Interfaces;
using Data.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API Master-Detail",
        Version = "v1",
        Description = "API para gestión de Categorías y Productos (Master-Detail)"
    });
});

// Configuración CORS y servicios
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", 
        builder => builder
            .WithOrigins("http://127.0.0.1:5500", "https://localhost:3000", "https://miotrofrontend.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Configuración predeterminada para SQL Server con migraciones en Entity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Entity")));

Console.WriteLine("Usando SQL Server como proveedor de base de datos");

// Inyección de dependencias - Repositorios
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

// Inyección de dependencias - Servicios
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();



try
{
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("AllowAll"); // Habilitar CORS - IMPORTANTE: debe ir antes de app.UseAuthorization()

    app.UseHttpsRedirection();

    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Error al iniciar la aplicación: {ex.Message}");
    throw;
}