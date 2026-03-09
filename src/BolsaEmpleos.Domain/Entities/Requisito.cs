using BolsaEmpleos.Domain.Common;
using BolsaEmpleos.Domain.Enums;

namespace BolsaEmpleos.Domain.Entities;

// Representa un requisito especifico dentro de una oferta de trabajo.
// Un requisito puede ser relevante (indispensable) o no relevante (deseable).
// Los requisitos no relevantes son candidatos a ser cubiertos con cursos cortos
// sugeridos por la IA cuando el joven postulante no posee esa habilidad.
public class Requisito : EntidadBase
{
    // Identificador de la oferta de trabajo a la que pertenece (clave foranea)
    public int OfertaTrabajoId { get; set; }

    // Oferta de trabajo a la que pertenece este requisito
    public OfertaTrabajo OfertaTrabajo { get; set; } = null!;

    // Identificador de la habilidad requerida por este requisito (clave foranea)
    public int HabilidadId { get; set; }

    // Habilidad que describe el contenido de este requisito
    public Habilidad Habilidad { get; set; } = null!;

    // Clasificacion del requisito como relevante o no relevante para el puesto
    public TipoRequisito TipoRequisito { get; set; } = TipoRequisito.Relevante;

    // Descripcion adicional sobre el nivel o contexto esperado de la habilidad
    public string? Descripcion { get; set; }
}
