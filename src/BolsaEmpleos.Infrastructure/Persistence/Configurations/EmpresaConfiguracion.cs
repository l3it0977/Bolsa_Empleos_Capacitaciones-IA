using BolsaEmpleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BolsaEmpleos.Infrastructure.Persistence.Configurations;

// Configuracion de EF Core para la entidad Empresa.
public class EmpresaConfiguracion : IEntityTypeConfiguration<Empresa>
{
    public void Configure(EntityTypeBuilder<Empresa> constructor)
    {
        constructor.ToTable("empresas");

        constructor.HasKey(e => e.Id);
        constructor.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

        constructor.Property(e => e.RazonSocial)
            .HasColumnName("razon_social")
            .IsRequired()
            .HasMaxLength(200);

        constructor.Property(e => e.NumeroIdentificacion)
            .HasColumnName("numero_identificacion")
            .IsRequired()
            .HasMaxLength(20);

        constructor.HasIndex(e => e.NumeroIdentificacion).IsUnique();

        constructor.Property(e => e.CorreoElectronico)
            .HasColumnName("correo_electronico")
            .IsRequired()
            .HasMaxLength(200);

        constructor.HasIndex(e => e.CorreoElectronico).IsUnique();

        constructor.Property(e => e.ContrasenaHash)
            .HasColumnName("contrasena_hash")
            .IsRequired();

        constructor.Property(e => e.Telefono)
            .HasColumnName("telefono")
            .HasMaxLength(20);

        constructor.Property(e => e.Descripcion)
            .HasColumnName("descripcion")
            .HasMaxLength(500);

        constructor.Property(e => e.SitioWeb)
            .HasColumnName("sitio_web")
            .HasMaxLength(500);

        constructor.Property(e => e.FechaCreacion)
            .HasColumnName("fecha_creacion")
            .IsRequired();

        constructor.Property(e => e.FechaModificacion)
            .HasColumnName("fecha_modificacion");

        constructor.Property(e => e.Activo)
            .HasColumnName("activo")
            .IsRequired()
            .HasDefaultValue(true);
    }
}
