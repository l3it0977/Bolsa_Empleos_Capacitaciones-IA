using BolsaEmpleos.Infrastructure.Persistence.Context;
using BolsaEmpleos.Application;
using BolsaEmpleos.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Registrar los servicios de la capa de aplicacion (servicios de negocio y AutoMapper)
builder.Services.AgregarAplicacion();

// Registrar los servicios de la capa de infraestructura (DbContext y repositorios)
builder.Services.AgregarInfraestructura(builder.Configuration);

// Agregar controladores de la API REST
builder.Services.AddControllers();

// Configurar CORS para desarrollo local del frontend React
builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy("FrontendLocal", politica =>
    {
        politica.WithOrigins(builder.Configuration["Frontend:Url"] ?? "http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Configurar autenticacion JWT para proteger endpoints sensibles
var claveJwt = builder.Configuration["Jwt:ClaveSecreta"];
if (string.IsNullOrWhiteSpace(claveJwt))
{
    throw new InvalidOperationException("Falta configurar Jwt:ClaveSecreta.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones =>
    {
        opciones.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveJwt)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Configurar Swagger para documentacion de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Bolsa de Empleos con Capacitaciones - API",
        Version = "v1",
        Description = "API REST para la gestion de jovenes, empresas, ofertas de trabajo y cursos de capacitacion."
    });
});

var app = builder.Build();

// Crear esquema local automaticamente cuando no hay migraciones disponibles
using (var alcance = app.Services.CreateScope())
{
    var contexto = alcance.ServiceProvider.GetRequiredService<BolsaEmpleosDbContext>();
    contexto.Database.EnsureCreated();
}

// Habilitar Swagger en todos los entornos para facilitar el desarrollo y las pruebas
app.UseSwagger();
app.UseSwaggerUI(opciones =>
{
    opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "BolsaEmpleos API v1");
    opciones.RoutePrefix = string.Empty; // Swagger disponible en la raiz
});

app.UseHttpsRedirection();
app.UseCors("FrontendLocal");
app.UseAuthentication();
app.UseAuthorization();

// Mapear los controladores de la API
app.MapControllers();

app.Run();
