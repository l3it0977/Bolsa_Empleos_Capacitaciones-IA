using AutoMapper;
using BolsaEmpleos.Application.DTOs.Postulacion;
using BolsaEmpleos.Application.DTOs.Seguimiento;
using BolsaEmpleos.Application.Interfaces;
using BolsaEmpleos.Domain.Enums;
using BolsaEmpleos.Domain.Interfaces;

namespace BolsaEmpleos.Application.Services;

// Servicio de aplicacion encargado del seguimiento laboral del joven y de la
// recomendacion de nuevas ofertas de trabajo cuando no fue contratado.
// Implementa el algoritmo de parentesco que compara las habilidades del CV
// con los requisitos de cada oferta y prioriza las coincidencias mas altas.
public class ServicioSeguimiento : IServicioSeguimiento
{
    private readonly IRepositorioJoven _repositorioJoven;
    private readonly IRepositorioPostulacion _repositorioPostulacion;
    private readonly IRepositorioOfertaTrabajo _repositorioOfertaTrabajo;
    private readonly IMapper _mapper;

    public ServicioSeguimiento(
        IRepositorioJoven repositorioJoven,
        IRepositorioPostulacion repositorioPostulacion,
        IRepositorioOfertaTrabajo repositorioOfertaTrabajo,
        IMapper mapper)
    {
        _repositorioJoven = repositorioJoven;
        _repositorioPostulacion = repositorioPostulacion;
        _repositorioOfertaTrabajo = repositorioOfertaTrabajo;
        _mapper = mapper;
    }

    // Obtiene el seguimiento completo del joven: estado de empleo, historial de postulaciones
    // y, si no consiguio empleo activo, las ofertas recomendadas por compatibilidad de CV.
    public async Task<ResultadoSeguimientoDto> ObtenerSeguimientoAsync(int jovenId)
    {
        // Verificar que el joven exista y este activo
        var joven = await _repositorioJoven.ObtenerPorIdAsync(jovenId);
        if (joven is null || !joven.Activo)
        {
            throw new InvalidOperationException(
                $"No se encontro un joven activo con el identificador {jovenId}.");
        }

        // Obtener el historial completo de postulaciones del joven
        var postulaciones = (await _repositorioPostulacion.ObtenerPorJovenAsync(jovenId)).ToList();

        // Contar postulaciones por estado
        var aceptadas = postulaciones.Count(p => p.Estado == EstadoPostulacion.Aceptada);
        var rechazadas = postulaciones.Count(p => p.Estado == EstadoPostulacion.Rechazada);
        var pendientes = postulaciones.Count(p => p.Estado == EstadoPostulacion.Pendiente);

        // El joven tiene empleo si al menos una postulacion fue aceptada
        var tieneEmpleo = aceptadas > 0;

        // Solo recomendar ofertas si el joven no tiene empleo activo
        IEnumerable<RecomendacionOfertaDto> recomendaciones = new List<RecomendacionOfertaDto>();
        if (!tieneEmpleo)
        {
            recomendaciones = await RecomendarOfertasAsync(jovenId);
        }

        return new ResultadoSeguimientoDto
        {
            JovenId = jovenId,
            NombreJoven = $"{joven.Nombre} {joven.Apellido}",
            TieneEmpleoActual = tieneEmpleo,
            TotalPostulaciones = postulaciones.Count,
            PostulacionesAceptadas = aceptadas,
            PostulacionesRechazadas = rechazadas,
            PostulacionesPendientes = pendientes,
            OfertasRecomendadas = recomendaciones
        };
    }

    // Algoritmo de parentesco: analiza las habilidades del CV del joven y las compara
    // con los requisitos de cada oferta publicada para calcular el porcentaje de compatibilidad.
    // Las ofertas se ordenan de mayor a menor compatibilidad y se excluyen
    // aquellas a las que el joven ya se postulo.
    public async Task<IEnumerable<RecomendacionOfertaDto>> RecomendarOfertasAsync(int jovenId)
    {
        // Obtener el joven con su curriculum y habilidades para el analisis
        var joven = await _repositorioJoven.ObtenerConCurriculumAsync(jovenId);
        if (joven is null || !joven.Activo)
        {
            throw new InvalidOperationException(
                $"No se encontro un joven activo con el identificador {jovenId}.");
        }

        // Extraer el conjunto de identificadores de habilidades del CV
        var habilidadesJoven = ObtenerHabilidadesDelCurriculum(joven);

        // Obtener las ofertas publicadas a las que el joven ya se postulo
        var postulacionesExistentes = await _repositorioPostulacion.ObtenerPorJovenAsync(jovenId);
        var ofertasPostuladas = postulacionesExistentes
            .Select(p => p.OfertaTrabajoId)
            .ToHashSet();

        // Obtener todas las ofertas publicadas con sus requisitos y habilidades
        var ofertasPublicadas = await _repositorioOfertaTrabajo
            .ObtenerPorEstadoAsync(EstadoOferta.Publicada);

        var recomendaciones = new List<RecomendacionOfertaDto>();

        foreach (var oferta in ofertasPublicadas)
        {
            // Excluir ofertas a las que el joven ya se postulo
            if (ofertasPostuladas.Contains(oferta.Id))
            {
                continue;
            }

            // Obtener los requisitos activos de la oferta
            var requisitos = oferta.Requisitos.Where(r => r.Activo).ToList();

            // Si la oferta no tiene requisitos, omitirla del ranking
            if (requisitos.Count == 0)
            {
                continue;
            }

            // Separar las habilidades que el joven posee de las que le faltan
            var idsRequisitos = requisitos.Select(r => r.HabilidadId).ToHashSet();
            var coincidentes = idsRequisitos.Intersect(habilidadesJoven).ToHashSet();
            var faltantes = idsRequisitos.Except(habilidadesJoven).ToHashSet();

            // Calcular el porcentaje de compatibilidad redondeado a dos decimales
            var porcentaje = Math.Round(
                (decimal)coincidentes.Count / requisitos.Count * 100, 2);

            // Construir los nombres de habilidades coincidentes y faltantes para el DTO
            var nombresCoincidentes = requisitos
                .Where(r => coincidentes.Contains(r.HabilidadId))
                .Select(r => r.Habilidad.Nombre)
                .Distinct()
                .ToList();

            var nombresFaltantes = requisitos
                .Where(r => faltantes.Contains(r.HabilidadId))
                .Select(r => r.Habilidad.Nombre)
                .Distinct()
                .ToList();

            recomendaciones.Add(new RecomendacionOfertaDto
            {
                OfertaTrabajoId = oferta.Id,
                TituloOferta = oferta.Titulo,
                NombreEmpresa = oferta.Empresa.RazonSocial,
                Ubicacion = oferta.Ubicacion,
                Salario = oferta.Salario,
                PorcentajeCompatibilidad = porcentaje,
                TotalCoincidencias = coincidentes.Count,
                TotalRequisitos = requisitos.Count,
                HabilidadesCoincidentes = nombresCoincidentes,
                HabilidadesFaltantes = nombresFaltantes
            });
        }

        // Ordenar por porcentaje de compatibilidad de mayor a menor (mayor parentesco primero)
        return recomendaciones
            .OrderByDescending(r => r.PorcentajeCompatibilidad)
            .ThenByDescending(r => r.TotalCoincidencias)
            .ToList();
    }

    // Registra si el joven consiguio empleo actualizando el estado de la postulacion.
    // Establece Aceptada cuando consiguioEmpleo es verdadero, Rechazada en caso contrario.
    public async Task<PostulacionDto?> RegistrarResultadoEmpleoAsync(
        int postulacionId, bool consiguioEmpleo)
    {
        var postulacion = await _repositorioPostulacion.ObtenerPorIdAsync(postulacionId);
        if (postulacion is null)
        {
            return null;
        }

        // Actualizar el estado segun el resultado de la contratacion
        postulacion.Estado = consiguioEmpleo
            ? EstadoPostulacion.Aceptada
            : EstadoPostulacion.Rechazada;

        await _repositorioPostulacion.ActualizarAsync(postulacion);

        // Recargar con relaciones para el mapeo correcto
        var postulacionActualizada = await _repositorioPostulacion.ObtenerPorIdAsync(postulacionId);
        return _mapper.Map<PostulacionDto>(postulacionActualizada!);
    }

    // Extrae el conjunto de identificadores de habilidades registradas en el CV del joven.
    // Retorna un conjunto vacio si el joven no tiene curriculum o no tiene habilidades.
    private static HashSet<int> ObtenerHabilidadesDelCurriculum(
        Domain.Entities.Joven joven)
    {
        if (joven.Curriculum?.CurriculumHabilidades is null)
        {
            return new HashSet<int>();
        }

        return joven.Curriculum.CurriculumHabilidades
            .Select(ch => ch.HabilidadId)
            .ToHashSet();
    }
}
