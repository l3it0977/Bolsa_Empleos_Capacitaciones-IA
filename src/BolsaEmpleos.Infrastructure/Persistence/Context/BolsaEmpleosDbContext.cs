using BolsaEmpleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BolsaEmpleos.Infrastructure.Persistence.Context;

// Contexto principal de base de datos para la aplicacion.
// Utiliza Entity Framework Core con el proveedor de PostgreSQL.
// Cada DbSet representa una tabla en la base de datos.
public class BolsaEmpleosDbContext : DbContext
{
    public BolsaEmpleosDbContext(DbContextOptions<BolsaEmpleosDbContext> opciones)
        : base(opciones)
    {
    }

    // Tabla de jovenes registrados en la plataforma
    public DbSet<Joven> Jovenes { get; set; }

    // Tabla de curricula vitae de los jovenes
    public DbSet<Curriculum> Curricula { get; set; }

    // Tabla intermedia entre Curriculum y Habilidad
    public DbSet<CurriculumHabilidad> CurriculumHabilidades { get; set; }

    // Tabla de empresas registradas en la plataforma
    public DbSet<Empresa> Empresas { get; set; }

    // Tabla de ofertas de trabajo publicadas por empresas
    public DbSet<OfertaTrabajo> OfertasTrabajo { get; set; }

    // Tabla de requisitos de cada oferta de trabajo
    public DbSet<Requisito> Requisitos { get; set; }

    // Tabla de habilidades disponibles en el sistema
    public DbSet<Habilidad> Habilidades { get; set; }

    // Tabla de cursos de capacitacion disponibles
    public DbSet<Curso> Cursos { get; set; }

    // Tabla de evaluaciones de jovenes sobre cursos
    public DbSet<Evaluacion> Evaluaciones { get; set; }

    // Tabla de postulaciones de jovenes a ofertas de trabajo
    public DbSet<Postulacion> Postulaciones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar configuraciones de entidad desde el ensamblado actual
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BolsaEmpleosDbContext).Assembly);
    }
}
