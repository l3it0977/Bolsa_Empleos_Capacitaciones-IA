namespace BolsaEmpleos.Application.DTOs.Seguimiento;

// DTO que representa una oferta de trabajo recomendada para un joven,
// junto con el grado de compatibilidad calculado por el algoritmo de parentesco.
public class RecomendacionOfertaDto
{
    // Identificador de la oferta de trabajo recomendada
    public int OfertaTrabajoId { get; set; }

    // Titulo del puesto de trabajo
    public string TituloOferta { get; set; } = string.Empty;

    // Razon social de la empresa que publico la oferta
    public string NombreEmpresa { get; set; } = string.Empty;

    // Ubicacion o modalidad del trabajo
    public string Ubicacion { get; set; } = string.Empty;

    // Salario ofrecido (puede ser nulo si no se divulga)
    public decimal? Salario { get; set; }

    // Porcentaje de compatibilidad entre las habilidades del CV y los requisitos de la oferta.
    // Calculado como: (habilidades coincidentes / total de requisitos) * 100
    public decimal PorcentajeCompatibilidad { get; set; }

    // Cantidad de requisitos de la oferta que el joven ya posee en su curriculum
    public int TotalCoincidencias { get; set; }

    // Cantidad total de requisitos de la oferta
    public int TotalRequisitos { get; set; }

    // Nombres de las habilidades que el joven posee y que coinciden con los requisitos
    public IEnumerable<string> HabilidadesCoincidentes { get; set; } = new List<string>();

    // Nombres de las habilidades requeridas que el joven aun no posee en su curriculum
    public IEnumerable<string> HabilidadesFaltantes { get; set; } = new List<string>();
}
