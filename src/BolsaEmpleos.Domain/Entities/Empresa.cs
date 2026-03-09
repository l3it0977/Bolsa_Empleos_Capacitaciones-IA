using BolsaEmpleos.Domain.Common;

namespace BolsaEmpleos.Domain.Entities;

// Representa una empresa que publica ofertas de trabajo en la plataforma.
// Las empresas pueden crear multiples ofertas con requisitos especificos
// y el sistema de IA sugiere candidatos que mejor se ajusten al perfil.
public class Empresa : EntidadBase
{
    // Razon social o nombre comercial de la empresa
    public string RazonSocial { get; set; } = string.Empty;

    // Numero de identificacion tributaria (RUC, CUIT, NIT, etc.)
    public string NumeroIdentificacion { get; set; } = string.Empty;

    // Correo electronico corporativo de la empresa
    public string CorreoElectronico { get; set; } = string.Empty;

    // Contrasena cifrada de la cuenta de la empresa
    public string ContrasenaHash { get; set; } = string.Empty;

    // Numero de telefono de contacto de la empresa
    public string? Telefono { get; set; }

    // Descripcion general de la empresa y su rubro
    public string? Descripcion { get; set; }

    // Sitio web oficial de la empresa
    public string? SitioWeb { get; set; }

    // Ofertas de trabajo publicadas por esta empresa (relacion uno a muchos)
    public ICollection<OfertaTrabajo> OfertasTrabajo { get; set; } = new List<OfertaTrabajo>();
}
