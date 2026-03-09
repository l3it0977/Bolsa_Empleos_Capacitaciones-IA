using BolsaEmpleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BolsaEmpleos.Infrastructure.Persistence.Configurations;

// Configuracion de EF Core para la entidad Evaluacion.
public class EvaluacionConfiguracion : IEntityTypeConfiguration<Evaluacion>
{
    public void Configure(EntityTypeBuilder<Evaluacion> constructor)
    {
        constructor.ToTable("evaluaciones");

        constructor.HasKey(e => e.Id);
        constructor.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

        constructor.Property(e => e.JovenId)
            .HasColumnName("joven_id")
            .IsRequired();

        constructor.Property(e => e.CursoId)
            .HasColumnName("curso_id")
            .IsRequired();

        constructor.Property(e => e.PuntajeObtenido)
            .HasColumnName("puntaje_obtenido");

        constructor.Property(e => e.Estado)
            .HasColumnName("estado")
            .IsRequired();

        constructor.Property(e => e.FechaInicio)
            .HasColumnName("fecha_inicio");

        constructor.Property(e => e.FechaFin)
            .HasColumnName("fecha_fin");

        constructor.Property(e => e.Intentos)
            .HasColumnName("intentos")
            .IsRequired()
            .HasDefaultValue(0);

        constructor.Property(e => e.FechaCreacion)
            .HasColumnName("fecha_creacion")
            .IsRequired();

        constructor.Property(e => e.FechaModificacion)
            .HasColumnName("fecha_modificacion");

        constructor.Property(e => e.Activo)
            .HasColumnName("activo")
            .IsRequired()
            .HasDefaultValue(true);

        // Relacion con Joven
        constructor.HasOne(e => e.Joven)
            .WithMany(j => j.Evaluaciones)
            .HasForeignKey(e => e.JovenId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacion con Curso
        constructor.HasOne(e => e.Curso)
            .WithMany(c => c.Evaluaciones)
            .HasForeignKey(e => e.CursoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
