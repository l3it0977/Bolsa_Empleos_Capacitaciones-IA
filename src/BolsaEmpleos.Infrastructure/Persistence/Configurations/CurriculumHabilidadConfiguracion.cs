using BolsaEmpleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BolsaEmpleos.Infrastructure.Persistence.Configurations;

// Configuracion de EF Core para la tabla intermedia CurriculumHabilidad.
public class CurriculumHabilidadConfiguracion : IEntityTypeConfiguration<CurriculumHabilidad>
{
    public void Configure(EntityTypeBuilder<CurriculumHabilidad> constructor)
    {
        constructor.ToTable("curriculum_habilidades");

        constructor.HasKey(ch => ch.Id);
        constructor.Property(ch => ch.Id).HasColumnName("id").ValueGeneratedOnAdd();

        constructor.Property(ch => ch.CurriculumId)
            .HasColumnName("curriculum_id")
            .IsRequired();

        constructor.Property(ch => ch.HabilidadId)
            .HasColumnName("habilidad_id")
            .IsRequired();

        // No se permite agregar la misma habilidad dos veces al mismo curriculum
        constructor.HasIndex(ch => new { ch.CurriculumId, ch.HabilidadId }).IsUnique();

        constructor.Property(ch => ch.ObtenidaPorCurso)
            .HasColumnName("obtenida_por_curso")
            .IsRequired()
            .HasDefaultValue(false);

        constructor.Property(ch => ch.FechaAgregado)
            .HasColumnName("fecha_agregado")
            .IsRequired();

        constructor.Property(ch => ch.FechaCreacion)
            .HasColumnName("fecha_creacion")
            .IsRequired();

        constructor.Property(ch => ch.FechaModificacion)
            .HasColumnName("fecha_modificacion");

        constructor.Property(ch => ch.Activo)
            .HasColumnName("activo")
            .IsRequired()
            .HasDefaultValue(true);

        // Relacion con Curriculum
        constructor.HasOne(ch => ch.Curriculum)
            .WithMany(c => c.CurriculumHabilidades)
            .HasForeignKey(ch => ch.CurriculumId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacion con Habilidad
        constructor.HasOne(ch => ch.Habilidad)
            .WithMany(h => h.CurriculumHabilidades)
            .HasForeignKey(ch => ch.HabilidadId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
