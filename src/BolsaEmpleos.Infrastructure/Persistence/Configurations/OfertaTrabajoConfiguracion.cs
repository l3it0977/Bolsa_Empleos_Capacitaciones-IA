using BolsaEmpleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BolsaEmpleos.Infrastructure.Persistence.Configurations;

// Configuracion de EF Core para la entidad OfertaTrabajo.
public class OfertaTrabajoConfiguracion : IEntityTypeConfiguration<OfertaTrabajo>
{
    public void Configure(EntityTypeBuilder<OfertaTrabajo> constructor)
    {
        constructor.ToTable("ofertas_trabajo");

        constructor.HasKey(o => o.Id);
        constructor.Property(o => o.Id).HasColumnName("id").ValueGeneratedOnAdd();

        constructor.Property(o => o.EmpresaId)
            .HasColumnName("empresa_id")
            .IsRequired();

        constructor.Property(o => o.Titulo)
            .HasColumnName("titulo")
            .IsRequired()
            .HasMaxLength(200);

        constructor.Property(o => o.Descripcion)
            .HasColumnName("descripcion")
            .IsRequired();

        constructor.Property(o => o.Ubicacion)
            .HasColumnName("ubicacion")
            .IsRequired()
            .HasMaxLength(200);

        constructor.Property(o => o.Salario)
            .HasColumnName("salario")
            .HasPrecision(12, 2);

        constructor.Property(o => o.Estado)
            .HasColumnName("estado")
            .IsRequired();

        constructor.Property(o => o.FechaCierre)
            .HasColumnName("fecha_cierre");

        constructor.Property(o => o.FechaCreacion)
            .HasColumnName("fecha_creacion")
            .IsRequired();

        constructor.Property(o => o.FechaModificacion)
            .HasColumnName("fecha_modificacion");

        constructor.Property(o => o.Activo)
            .HasColumnName("activo")
            .IsRequired()
            .HasDefaultValue(true);

        // Una empresa puede tener muchas ofertas (relacion muchos a uno con Empresa)
        constructor.HasOne(o => o.Empresa)
            .WithMany(e => e.OfertasTrabajo)
            .HasForeignKey(o => o.EmpresaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
