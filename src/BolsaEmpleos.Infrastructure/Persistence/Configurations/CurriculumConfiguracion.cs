using BolsaEmpleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BolsaEmpleos.Infrastructure.Persistence.Configurations;

// Configuracion de EF Core para la entidad Curriculum.
public class CurriculumConfiguracion : IEntityTypeConfiguration<Curriculum>
{
    public void Configure(EntityTypeBuilder<Curriculum> constructor)
    {
        constructor.ToTable("curricula");

        constructor.HasKey(c => c.Id);
        constructor.Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();

        constructor.Property(c => c.JovenId)
            .HasColumnName("joven_id")
            .IsRequired();

        constructor.Property(c => c.ResumenProfesional)
            .HasColumnName("resumen_profesional")
            .HasMaxLength(1000);

        constructor.Property(c => c.TituloProfesional)
            .HasColumnName("titulo_profesional")
            .HasMaxLength(200);

        constructor.Property(c => c.UrlPortfolio)
            .HasColumnName("url_portfolio")
            .HasMaxLength(500);

        constructor.Property(c => c.FechaCreacion)
            .HasColumnName("fecha_creacion")
            .IsRequired();

        constructor.Property(c => c.FechaModificacion)
            .HasColumnName("fecha_modificacion");

        constructor.Property(c => c.Activo)
            .HasColumnName("activo")
            .IsRequired()
            .HasDefaultValue(true);
    }
}
