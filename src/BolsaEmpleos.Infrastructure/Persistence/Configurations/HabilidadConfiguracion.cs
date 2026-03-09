using BolsaEmpleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BolsaEmpleos.Infrastructure.Persistence.Configurations;

// Configuracion de EF Core para la entidad Habilidad.
public class HabilidadConfiguracion : IEntityTypeConfiguration<Habilidad>
{
    public void Configure(EntityTypeBuilder<Habilidad> constructor)
    {
        constructor.ToTable("habilidades");

        constructor.HasKey(h => h.Id);
        constructor.Property(h => h.Id).HasColumnName("id").ValueGeneratedOnAdd();

        constructor.Property(h => h.Nombre)
            .HasColumnName("nombre")
            .IsRequired()
            .HasMaxLength(100);

        // El nombre de la habilidad debe ser unico en el sistema
        constructor.HasIndex(h => h.Nombre).IsUnique();

        constructor.Property(h => h.Descripcion)
            .HasColumnName("descripcion")
            .HasMaxLength(500);

        constructor.Property(h => h.Categoria)
            .HasColumnName("categoria")
            .HasMaxLength(100);

        constructor.Property(h => h.FechaCreacion)
            .HasColumnName("fecha_creacion")
            .IsRequired();

        constructor.Property(h => h.FechaModificacion)
            .HasColumnName("fecha_modificacion");

        constructor.Property(h => h.Activo)
            .HasColumnName("activo")
            .IsRequired()
            .HasDefaultValue(true);
    }
}
