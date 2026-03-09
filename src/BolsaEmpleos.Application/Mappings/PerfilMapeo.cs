using AutoMapper;
using BolsaEmpleos.Application.DTOs.Joven;
using BolsaEmpleos.Application.DTOs.Curriculum;
using BolsaEmpleos.Application.DTOs.Empresa;
using BolsaEmpleos.Application.DTOs.OfertaTrabajo;
using BolsaEmpleos.Application.DTOs.Habilidad;
using BolsaEmpleos.Application.DTOs.Curso;
using BolsaEmpleos.Application.DTOs.Evaluacion;
using BolsaEmpleos.Domain.Entities;

namespace BolsaEmpleos.Application.Mappings;

// Perfil de AutoMapper que define las reglas de mapeo entre entidades del dominio
// y los DTOs de la capa de aplicacion.
// Centralizar los mapeos facilita el mantenimiento y reduce errores de conversion.
public class PerfilMapeo : Profile
{
    public PerfilMapeo()
    {
        // Mapeos de Joven
        CreateMap<Joven, JovenDto>();
        CreateMap<CrearJovenDto, Joven>()
            .ForMember(dest => dest.ContrasenaHash, opt => opt.Ignore()); // El hash se gestiona en el servicio

        // Mapeos de Curriculum
        CreateMap<Curriculum, CurriculumDto>()
            .ForMember(dest => dest.NombreJoven, opt => opt.MapFrom(src => src.Joven.Nombre))
            .ForMember(dest => dest.ApellidoJoven, opt => opt.MapFrom(src => src.Joven.Apellido))
            .ForMember(dest => dest.CorreoElectronicoJoven, opt => opt.MapFrom(src => src.Joven.CorreoElectronico))
            .ForMember(dest => dest.Habilidades, opt => opt.MapFrom(src => src.CurriculumHabilidades));

        CreateMap<CurriculumHabilidad, HabilidadCurriculumDto>()
            .ForMember(dest => dest.NombreHabilidad, opt => opt.MapFrom(src => src.Habilidad.Nombre))
            .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Habilidad.Categoria));

        CreateMap<GuardarCurriculumDto, Curriculum>();

        // Mapeos de Empresa
        CreateMap<Empresa, EmpresaDto>();
        CreateMap<CrearEmpresaDto, Empresa>()
            .ForMember(dest => dest.ContrasenaHash, opt => opt.Ignore()); // El hash se gestiona en el servicio

        // Mapeos de OfertaTrabajo
        CreateMap<OfertaTrabajo, OfertaTrabajoDto>()
            .ForMember(dest => dest.RazonSocialEmpresa, opt => opt.MapFrom(src => src.Empresa.RazonSocial));

        CreateMap<Requisito, RequisitoDto>()
            .ForMember(dest => dest.NombreHabilidad, opt => opt.MapFrom(src => src.Habilidad.Nombre));

        // Mapeos de Habilidad
        CreateMap<Habilidad, HabilidadDto>();
        CreateMap<GuardarHabilidadDto, Habilidad>();

        // Mapeos de Curso
        CreateMap<Curso, CursoDto>()
            .ForMember(dest => dest.NombreHabilidad, opt => opt.MapFrom(src => src.Habilidad.Nombre));

        CreateMap<GuardarCursoDto, Curso>();

        // Mapeos de Evaluacion
        CreateMap<Evaluacion, EvaluacionDto>()
            .ForMember(dest => dest.NombreJoven,
                opt => opt.MapFrom(src => src.Joven.Nombre + " " + src.Joven.Apellido))
            .ForMember(dest => dest.TituloCurso, opt => opt.MapFrom(src => src.Curso.Titulo));
    }
}
