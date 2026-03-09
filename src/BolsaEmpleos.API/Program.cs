using BolsaEmpleos.Application;
using BolsaEmpleos.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Registrar los servicios de la capa de aplicacion (servicios de negocio y AutoMapper)
builder.Services.AgregarAplicacion();

// Registrar los servicios de la capa de infraestructura (DbContext y repositorios)
builder.Services.AgregarInfraestructura(builder.Configuration);

// Agregar controladores de la API REST
builder.Services.AddControllers();

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

// Habilitar Swagger en todos los entornos para facilitar el desarrollo y las pruebas
app.UseSwagger();
app.UseSwaggerUI(opciones =>
{
    opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "BolsaEmpleos API v1");
    opciones.RoutePrefix = string.Empty; // Swagger disponible en la raiz
});

app.UseHttpsRedirection();

// Mapear los controladores de la API
app.MapControllers();

app.Run();
