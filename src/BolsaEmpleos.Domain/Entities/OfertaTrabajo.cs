using BolsaEmpleos.Domain.Common;
using BolsaEmpleos.Domain.Enums;

namespace BolsaEmpleos.Domain.Entities;

// Representa una oferta de trabajo publicada por una empresa.
// Contiene los requisitos necesarios para el puesto y permite que el sistema
// de IA identifique brechas de habilidades en los candidatos y sugiera cursos.
public class OfertaTrabajo : EntidadBase
{
    // Identificador de la empresa que publica la oferta (clave foranea)
    public int EmpresaId { get; set; }

    // Empresa que publica esta oferta de trabajo
    public Empresa Empresa { get; set; } = null!;

    // Titulo o nombre del puesto de trabajo
    public string Titulo { get; set; } = string.Empty;

    // Descripcion detallada del puesto, funciones y beneficios
    public string Descripcion { get; set; } = string.Empty;

    // Ubicacion fisica o modalidad del trabajo (presencial, remoto, hibrido)
    public string Ubicacion { get; set; } = string.Empty;

    // Salario ofrecido (puede ser nulo si no se divulga)
    public decimal? Salario { get; set; }

    // Estado actual de la oferta dentro del ciclo de vida
    public EstadoOferta Estado { get; set; } = EstadoOferta.Borrador;

    // Fecha limite para postularse a la oferta
    public DateTime? FechaCierre { get; set; }

    // Lista de requisitos que deben cumplir los candidatos (relacion uno a muchos)
    public ICollection<Requisito> Requisitos { get; set; } = new List<Requisito>();

    // Postulaciones recibidas para esta oferta (relacion uno a muchos)
    public ICollection<Postulacion> Postulaciones { get; set; } = new List<Postulacion>();
}
