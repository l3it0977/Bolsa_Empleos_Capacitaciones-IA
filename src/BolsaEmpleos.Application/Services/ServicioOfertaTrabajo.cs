using AutoMapper;
using BolsaEmpleos.Application.DTOs.OfertaTrabajo;
using BolsaEmpleos.Application.Interfaces;
using BolsaEmpleos.Domain.Entities;
using BolsaEmpleos.Domain.Enums;
using BolsaEmpleos.Domain.Interfaces;

namespace BolsaEmpleos.Application.Services;

// Servicio que implementa la logica de negocio para la gestion de ofertas de trabajo.
// Gestiona el ciclo de vida de las ofertas y sus requisitos asociados.
public class ServicioOfertaTrabajo : IServicioOfertaTrabajo
{
    private readonly IRepositorioOfertaTrabajo _repositorioOferta;
    private readonly IRepositorioEmpresa _repositorioEmpresa;
    private readonly IMapper _mapper;

    public ServicioOfertaTrabajo(
        IRepositorioOfertaTrabajo repositorioOferta,
        IRepositorioEmpresa repositorioEmpresa,
        IMapper mapper)
    {
        _repositorioOferta = repositorioOferta;
        _repositorioEmpresa = repositorioEmpresa;
        _mapper = mapper;
    }

    // Obtiene todas las ofertas con estado Publicada
    public async Task<IEnumerable<OfertaTrabajoDto>> ObtenerPublicadasAsync()
    {
        var ofertas = await _repositorioOferta.ObtenerPorEstadoAsync(EstadoOferta.Publicada);
        return _mapper.Map<IEnumerable<OfertaTrabajoDto>>(ofertas);
    }

    // Obtiene todas las ofertas de una empresa especifica
    public async Task<IEnumerable<OfertaTrabajoDto>> ObtenerPorEmpresaAsync(int empresaId)
    {
        var ofertas = await _repositorioOferta.ObtenerPorEmpresaAsync(empresaId);
        return _mapper.Map<IEnumerable<OfertaTrabajoDto>>(ofertas);
    }

    // Obtiene una oferta por su identificador con todos sus requisitos
    public async Task<OfertaTrabajoDto?> ObtenerPorIdAsync(int id)
    {
        var oferta = await _repositorioOferta.ObtenerConRequisitosAsync(id);
        return oferta is null ? null : _mapper.Map<OfertaTrabajoDto>(oferta);
    }

    // Crea una nueva oferta de trabajo para una empresa
    public async Task<OfertaTrabajoDto> CrearAsync(int empresaId, CrearOfertaTrabajoDto dto)
    {
        // Verificar que la empresa existe y esta activa
        var empresa = await _repositorioEmpresa.ObtenerPorIdAsync(empresaId);
        if (empresa is null || !empresa.Activo)
        {
            throw new InvalidOperationException(
                $"No se encontro una empresa activa con el identificador {empresaId}.");
        }

        // Crear la oferta con sus requisitos
        var oferta = new OfertaTrabajo
        {
            EmpresaId = empresaId,
            Titulo = dto.Titulo,
            Descripcion = dto.Descripcion,
            Ubicacion = dto.Ubicacion,
            Salario = dto.Salario,
            Estado = EstadoOferta.Borrador,
            FechaCierre = dto.FechaCierre,
            Requisitos = dto.Requisitos.Select(r => new Requisito
            {
                HabilidadId = r.HabilidadId,
                TipoRequisito = r.TipoRequisito,
                Descripcion = r.Descripcion
            }).ToList()
        };

        var ofertaCreada = await _repositorioOferta.AgregarAsync(oferta);
        return _mapper.Map<OfertaTrabajoDto>(ofertaCreada);
    }

    // Cambia el estado de una oferta (por ejemplo, de Borrador a Publicada)
    public async Task<bool> CambiarEstadoAsync(int ofertaId, EstadoOferta nuevoEstado)
    {
        var oferta = await _repositorioOferta.ObtenerPorIdAsync(ofertaId);
        if (oferta is null) return false;

        oferta.Estado = nuevoEstado;
        oferta.FechaModificacion = DateTime.UtcNow;

        await _repositorioOferta.ActualizarAsync(oferta);
        return true;
    }

    // Elimina logicamente una oferta de trabajo
    public async Task<bool> EliminarAsync(int id)
    {
        var existe = await _repositorioOferta.ExisteAsync(o => o.Id == id && o.Activo);
        if (!existe) return false;

        await _repositorioOferta.EliminarAsync(id);
        return true;
    }
}
