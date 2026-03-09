using BolsaEmpleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BolsaEmpleos.Infrastructure.Persistence.Configurations;

// Configuracion de Entity Framework Core para la entidad Joven.
// Define la estructura de la tabla, restricciones y relaciones en la base de datos.
public class JovenConfiguracion : IEntityTypeConfiguration<Joven>
{
    public void Configure(EntityTypeBuilder<Joven> constructor)
    {
        constructor.ToTable("jovenes");

        constructor.HasKey(j => j.Id);
        constructor.Property(j => j.Id).HasColumnName("id").ValueGeneratedOnAdd();

        constructor.Property(j => j.Nombre)
            .HasColumnName("nombre")
            .IsRequired()
            .HasMaxLength(100);

        constructor.Property(j => j.Apellido)
            .HasColumnName("apellido")
            .IsRequired()
            .HasMaxLength(100);

        constructor.Property(j => j.CorreoElectronico)
            .HasColumnName("correo_electronico")
            .IsRequired()
            .HasMaxLength(200);

        // El correo electronico debe ser unico en la tabla
        constructor.HasIndex(j => j.CorreoElectronico).IsUnique();

        constructor.Property(j => j.ContrasenaHash)
            .HasColumnName("contrasena_hash")
            .IsRequired();

        constructor.Property(j => j.Telefono)
            .HasColumnName("telefono")
            .HasMaxLength(20);

        constructor.Property(j => j.FechaNacimiento)
            .HasColumnName("fecha_nacimiento")
            .IsRequired();

        constructor.Property(j => j.NivelEducativo)
            .HasColumnName("nivel_educativo")
            .IsRequired();

        constructor.Property(j => j.FechaCreacion)
            .HasColumnName("fecha_creacion")
            .IsRequired();

        constructor.Property(j => j.FechaModificacion)
            .HasColumnName("fecha_modificacion");

        constructor.Property(j => j.Activo)
            .HasColumnName("activo")
            .IsRequired()
            .HasDefaultValue(true);

        // Un joven puede tener un solo curriculum (relacion uno a uno)
        constructor.HasOne(j => j.Curriculum)
            .WithOne(c => c.Joven)
            .HasForeignKey<Curriculum>(c => c.JovenId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
