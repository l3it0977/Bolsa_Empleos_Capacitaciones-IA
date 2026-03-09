using BolsaEmpleos.Application.Interfaces;
using BolsaEmpleos.Application.Mappings;
using BolsaEmpleos.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BolsaEmpleos.Application;

// Extension de IServiceCollection para registrar todos los servicios
// de la capa de aplicacion en el contenedor de inyeccion de dependencias.
public static class DependencyInjection
{
    // Agrega todos los servicios de la capa de aplicacion al contenedor DI
    public static IServiceCollection AgregarAplicacion(this IServiceCollection services)
    {
        // Registrar AutoMapper con el perfil de mapeo del proyecto
        services.AddAutoMapper(typeof(PerfilMapeo));

        // Registrar servicios de negocio
        services.AddScoped<IServicioJoven, ServicioJoven>();
        services.AddScoped<IServicioEmpresa, ServicioEmpresa>();
        services.AddScoped<IServicioCurriculum, ServicioCurriculum>();
        services.AddScoped<IServicioOfertaTrabajo, ServicioOfertaTrabajo>();
        services.AddScoped<IServicioHabilidad, ServicioHabilidad>();
        services.AddScoped<IServicioCurso, ServicioCurso>();
        services.AddScoped<IServicioEvaluacion, ServicioEvaluacion>();
        services.AddScoped<IServicioPostulacion, ServicioPostulacion>();

        return services;
    }
}
