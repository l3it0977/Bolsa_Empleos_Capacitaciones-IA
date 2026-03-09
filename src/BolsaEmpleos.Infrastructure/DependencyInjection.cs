using BolsaEmpleos.Application.Interfaces;
using BolsaEmpleos.Domain.Interfaces;
using BolsaEmpleos.Infrastructure.IA;
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

        // Configurar el cliente HTTP para NotebookLM y registrar la implementacion de IClienteIA
        services.Configure<ConfiguracionNotebookLM>(opciones =>
            configuracion.GetSection("NotebookLM").Bind(opciones));

        services.AddHttpClient<IClienteIA, ClienteNotebookLM>(cliente =>
        {
            var urlBase = configuracion["NotebookLM:UrlBase"];
            if (!string.IsNullOrWhiteSpace(urlBase))
            {
                cliente.BaseAddress = new Uri(urlBase);
            }

            var tiempoEspera = configuracion.GetValue<int>("NotebookLM:TiempoEsperaSegundos", 30);
            cliente.Timeout = TimeSpan.FromSeconds(tiempoEspera);
        });

        return services;
    }
}

