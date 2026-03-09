namespace BolsaEmpleos.Domain.Common;

// Clase base para todas las entidades del dominio.
// Centraliza la clave primaria y los campos de auditoria
// para evitar repeticion en cada entidad.
public abstract class EntidadBase
{
    // Identificador unico de la entidad (clave primaria)
    public int Id { get; set; }

    // Fecha y hora en que se creo el registro
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    // Fecha y hora de la ultima modificacion del registro
    public DateTime? FechaModificacion { get; set; }

    // Indica si el registro esta activo (borrado logico)
    public bool Activo { get; set; } = true;
}
