using BolsaEmpleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BolsaEmpleos.Infrastructure.Persistence.Configurations;

// Configuracion de EF Core para la entidad Curso.
public class CursoConfiguracion : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> constructor)
    {
        constructor.ToTable("cursos");

        constructor.HasKey(c => c.Id);
        constructor.Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();

        constructor.Property(c => c.Titulo)
            .HasColumnName("titulo")
            .IsRequired()
            .HasMaxLength(200);

        constructor.Property(c => c.Descripcion)
            .HasColumnName("descripcion")
            .IsRequired();

        constructor.Property(c => c.HabilidadId)
            .HasColumnName("habilidad_id")
            .IsRequired();

        constructor.Property(c => c.DuracionHoras)
            .HasColumnName("duracion_horas")
            .IsRequired()
            .HasPrecision(4, 1);

        constructor.Property(c => c.UrlMaterial)
            .HasColumnName("url_material")
            .HasMaxLength(500);

        constructor.Property(c => c.PuntajeMinimoAprobacion)
            .HasColumnName("puntaje_minimo_aprobacion")
            .IsRequired()
            .HasDefaultValue(70);

        constructor.Property(c => c.FechaCreacion)
            .HasColumnName("fecha_creacion")
            .IsRequired();

        constructor.Property(c => c.FechaModificacion)
            .HasColumnName("fecha_modificacion");

        constructor.Property(c => c.Activo)
            .HasColumnName("activo")
            .IsRequired()
            .HasDefaultValue(true);

        // Un curso esta asociado a una sola habilidad
        constructor.HasOne(c => c.Habilidad)
            .WithMany(h => h.Cursos)
            .HasForeignKey(c => c.HabilidadId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
