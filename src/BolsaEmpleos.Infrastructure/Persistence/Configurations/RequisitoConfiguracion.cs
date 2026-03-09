using BolsaEmpleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BolsaEmpleos.Infrastructure.Persistence.Configurations;

// Configuracion de EF Core para la entidad Requisito.
public class RequisitoConfiguracion : IEntityTypeConfiguration<Requisito>
{
    public void Configure(EntityTypeBuilder<Requisito> constructor)
    {
        constructor.ToTable("requisitos");

        constructor.HasKey(r => r.Id);
        constructor.Property(r => r.Id).HasColumnName("id").ValueGeneratedOnAdd();

        constructor.Property(r => r.OfertaTrabajoId)
            .HasColumnName("oferta_trabajo_id")
            .IsRequired();

        constructor.Property(r => r.HabilidadId)
            .HasColumnName("habilidad_id")
            .IsRequired();

        constructor.Property(r => r.TipoRequisito)
            .HasColumnName("tipo_requisito")
            .IsRequired();

        constructor.Property(r => r.Descripcion)
            .HasColumnName("descripcion")
            .HasMaxLength(500);

        constructor.Property(r => r.FechaCreacion)
            .HasColumnName("fecha_creacion")
            .IsRequired();

        constructor.Property(r => r.FechaModificacion)
            .HasColumnName("fecha_modificacion");

        constructor.Property(r => r.Activo)
            .HasColumnName("activo")
            .IsRequired()
            .HasDefaultValue(true);

        // Relacion con OfertaTrabajo
        constructor.HasOne(r => r.OfertaTrabajo)
            .WithMany(o => o.Requisitos)
            .HasForeignKey(r => r.OfertaTrabajoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacion con Habilidad (no se elimina en cascada para preservar la habilidad)
        constructor.HasOne(r => r.Habilidad)
            .WithMany(h => h.Requisitos)
            .HasForeignKey(r => r.HabilidadId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
