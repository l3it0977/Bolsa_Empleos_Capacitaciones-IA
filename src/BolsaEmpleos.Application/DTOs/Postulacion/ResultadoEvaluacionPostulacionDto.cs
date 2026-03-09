namespace BolsaEmpleos.Application.DTOs.Postulacion;

// DTO de respuesta que informa si un joven puede postularse a una oferta.
// Incluye el detalle de las brechas de habilidades y los cursos sugeridos
// para que el frontend presente una experiencia clara al usuario.
public class ResultadoEvaluacionPostulacionDto
{
    // Identificador del joven evaluado
    public int JovenId { get; set; }

    // Identificador de la oferta de trabajo evaluada
    public int OfertaTrabajoId { get; set; }

    // Titulo de la oferta de trabajo
    public string TituloOferta { get; set; } = string.Empty;

    // Indica si el joven puede postularse de forma directa
    public bool PuedePostular { get; set; }

    // Motivo por el cual no puede postularse (nulo si puede postular)
    public string? MotivoBloqueo { get; set; }

    // Habilidades relevantes que el joven no posee y que bloquean la postulacion
    public IEnumerable<HabilidadFaltanteDto> HabilidadesRelevantesFaltantes { get; set; }
        = new List<HabilidadFaltanteDto>();

    // Cursos sugeridos para cubrir habilidades no relevantes faltantes
    public IEnumerable<CursoSugeridoDto> CursosSugeridos { get; set; }
        = new List<CursoSugeridoDto>();
}

// Detalle de una habilidad relevante que el joven no posee en su curriculum
public class HabilidadFaltanteDto
{
    // Identificador de la habilidad requerida
    public int HabilidadId { get; set; }

    // Nombre descriptivo de la habilidad
    public string NombreHabilidad { get; set; } = string.Empty;

    // Descripcion adicional del requisito segun la oferta
    public string? DescripcionRequisito { get; set; }
}

// Curso sugerido para cubrir una habilidad no relevante que falta en el curriculum
public class CursoSugeridoDto
{
    // Identificador del curso sugerido
    public int CursoId { get; set; }

    // Titulo del curso
    public string TituloCurso { get; set; } = string.Empty;

    // Identificador de la habilidad que otorga este curso
    public int HabilidadId { get; set; }

    // Nombre de la habilidad que otorga este curso
    public string NombreHabilidad { get; set; } = string.Empty;

    // Duracion estimada del curso en horas
    public decimal DuracionHoras { get; set; }

    // URL al material del curso (puede ser nulo)
    public string? UrlMaterial { get; set; }

    // Descripcion del requisito que cubre este curso segun la oferta
    public string? DescripcionRequisito { get; set; }
}
