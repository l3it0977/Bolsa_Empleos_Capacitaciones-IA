using BolsaEmpleos.Domain.Interfaces;
using BolsaEmpleos.Infrastructure.Persistence.Context;
using BolsaEmpleos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BolsaEmpleos.Infrastructure;

// Extension de IServiceCollection para registrar todos los servicios
// de la capa de infraestructura en el contenedor de inyeccion de dependencias.
public static class DependencyInjection
{
    // Agrega todos los servicios de la capa de infraestructura al contenedor DI
    public static IServiceCollection AgregarInfraestructura(
        this IServiceCollection services,
        IConfiguration configuracion)
    {
        // Configurar el contexto de base de datos con PostgreSQL
        services.AddDbContext<BolsaEmpleosDbContext>(opciones =>
            opciones.UseNpgsql(
                configuracion.GetConnectionString("BolsaEmpleosDb"),
                npgsqlOpciones => npgsqlOpciones.MigrationsAssembly(
                    typeof(BolsaEmpleosDbContext).Assembly.FullName)));

        // Registrar repositorios especificos
        services.AddScoped<IRepositorioJoven, RepositorioJoven>();
        services.AddScoped<IRepositorioEmpresa, RepositorioEmpresa>();
        services.AddScoped<IRepositorioCurriculum, RepositorioCurriculum>();
        services.AddScoped<IRepositorioOfertaTrabajo, RepositorioOfertaTrabajo>();
        services.AddScoped<IRepositorioHabilidad, RepositorioHabilidad>();
        services.AddScoped<IRepositorioCurso, RepositorioCurso>();
        services.AddScoped<IRepositorioEvaluacion, RepositorioEvaluacion>();
        services.AddScoped<IRepositorioPostulacion, RepositorioPostulacion>();

        return services;
    }
}
