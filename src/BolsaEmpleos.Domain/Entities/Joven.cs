using BolsaEmpleos.Domain.Common;
using BolsaEmpleos.Domain.Enums;

namespace BolsaEmpleos.Domain.Entities;

// Representa a un joven que busca empleo en la plataforma.
// Es el usuario principal del sistema y puede tener un curriculum,
// postularse a ofertas laborales y tomar cursos de capacitacion.
public class Joven : EntidadBase
{
    // Nombre de pila del joven
    public string Nombre { get; set; } = string.Empty;

    // Apellido del joven
    public string Apellido { get; set; } = string.Empty;

    // Correo electronico utilizado para autenticacion y comunicacion
    public string CorreoElectronico { get; set; } = string.Empty;

    // Contrasena cifrada del joven (nunca se almacena en texto plano)
    public string ContrasenaHash { get; set; } = string.Empty;

    // Numero de telefono de contacto
    public string? Telefono { get; set; }

    // Fecha de nacimiento del joven
    public DateOnly FechaNacimiento { get; set; }

    // Nivel educativo alcanzado por el joven
    public NivelEducativo NivelEducativo { get; set; } = NivelEducativo.Secundaria;

    // Curriculum vitae asociado al joven (relacion uno a uno)
    public Curriculum? Curriculum { get; set; }

    // Evaluaciones de cursos que ha tomado el joven (relacion uno a muchos)
    public ICollection<Evaluacion> Evaluaciones { get; set; } = new List<Evaluacion>();

    // Postulaciones a ofertas de trabajo realizadas por el joven (relacion uno a muchos)
    public ICollection<Postulacion> Postulaciones { get; set; } = new List<Postulacion>();
}
