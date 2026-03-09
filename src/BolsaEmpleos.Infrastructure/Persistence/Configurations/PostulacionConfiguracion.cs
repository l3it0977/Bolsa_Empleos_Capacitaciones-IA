using BolsaEmpleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BolsaEmpleos.Infrastructure.Persistence.Configurations;

// Configuracion de EF Core para la entidad Postulacion.
public class PostulacionConfiguracion : IEntityTypeConfiguration<Postulacion>
{
    public void Configure(EntityTypeBuilder<Postulacion> constructor)
    {
        constructor.ToTable("postulaciones");

        constructor.HasKey(p => p.Id);
        constructor.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();

        constructor.Property(p => p.JovenId)
            .HasColumnName("joven_id")
            .IsRequired();

        constructor.Property(p => p.OfertaTrabajoId)
            .HasColumnName("oferta_trabajo_id")
            .IsRequired();

        constructor.Property(p => p.Estado)
            .HasColumnName("estado")
            .IsRequired();

        constructor.Property(p => p.FechaPostulacion)
            .HasColumnName("fecha_postulacion")
            .IsRequired();

        constructor.Property(p => p.FechaCreacion)
            .HasColumnName("fecha_creacion")
            .IsRequired();

        constructor.Property(p => p.FechaModificacion)
            .HasColumnName("fecha_modificacion");

        constructor.Property(p => p.Activo)
            .HasColumnName("activo")
            .IsRequired()
            .HasDefaultValue(true);

        // Indice unico para evitar postulaciones duplicadas por joven y oferta
        constructor.HasIndex(p => new { p.JovenId, p.OfertaTrabajoId })
            .IsUnique();

        // Relacion con Joven
        constructor.HasOne(p => p.Joven)
            .WithMany(j => j.Postulaciones)
            .HasForeignKey(p => p.JovenId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacion con OfertaTrabajo
        constructor.HasOne(p => p.OfertaTrabajo)
            .WithMany(o => o.Postulaciones)
            .HasForeignKey(p => p.OfertaTrabajoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
