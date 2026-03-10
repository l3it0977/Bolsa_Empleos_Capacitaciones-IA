// Componente de gestion de ofertas laborales de la empresa.
// Muestra las ofertas creadas y permite crear nuevas o ver candidatos.

import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { obtenerOfertasPorEmpresa, cambiarEstadoOferta, eliminarOferta } from '../../api/clienteApi';
import { useContextoEmpresa } from '../../contexto/ContextoEmpresa';
import CargandoSpinner from '../comunes/CargandoSpinner';
import MensajeError from '../comunes/MensajeError';

// Etiquetas legibles para los estados de oferta del backend
const etiquetasEstadoOferta = {
  0: 'Borrador',
  1: 'Publicada',
  2: 'En Revision',
  3: 'Cerrada',
  4: 'Cancelada',
};

export default function GestionOfertas() {
  const navegar = useNavigate();
  const { empresaActual } = useContextoEmpresa();

  const [ofertas, setOfertas] = useState([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState('');
  const [mensajeExito, setMensajeExito] = useState('');

  useEffect(() => {
    if (!empresaActual) {
      navegar('/empresa/registro');
      return;
    }
    cargarOfertas();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [empresaActual]);

  async function cargarOfertas() {
    setCargando(true);
    setError('');
    try {
      const lista = await obtenerOfertasPorEmpresa(empresaActual.id);
      setOfertas(lista);
    } catch {
      setError('No se pudieron cargar las ofertas de la empresa.');
    } finally {
      setCargando(false);
    }
  }

  /** Cambia el estado de una oferta (publicar, cerrar, etc.) */
  async function manejarCambioEstado(ofertaId, nuevoEstado) {
    setError('');
    setMensajeExito('');
    try {
      await cambiarEstadoOferta(ofertaId, nuevoEstado);
      setMensajeExito('Estado de la oferta actualizado correctamente.');
      await cargarOfertas();
    } catch {
      setError('No se pudo actualizar el estado de la oferta.');
    }
  }

  /** Elimina logicamente una oferta */
  async function manejarEliminar(ofertaId) {
    if (!window.confirm('¿Confirma que desea eliminar esta oferta?')) return;
    setError('');
    setMensajeExito('');
    try {
      await eliminarOferta(ofertaId);
      setMensajeExito('Oferta eliminada correctamente.');
      setOfertas(anteriores => anteriores.filter(o => o.id !== ofertaId));
    } catch {
      setError('No se pudo eliminar la oferta.');
    }
  }

  if (cargando) return <CargandoSpinner />;

  return (
    <div className="contenedor-pagina">
      <div className="encabezado-panel">
        <div>
          <h1>Mis Ofertas Laborales</h1>
          <p className="descripcion-seccion">
            Gestione las ofertas publicadas y el proceso de seleccion de candidatos.
          </p>
          {empresaActual && (
            <p className="nombre-empresa-panel">{empresaActual.razonSocial}</p>
          )}
        </div>
        <button
          className="boton-primario"
          onClick={() => navegar('/empresa/ofertas/nueva')}
        >
          + Nueva Oferta
        </button>
      </div>

      {error && <MensajeError mensaje={error} />}

      {mensajeExito && (
        <div className="mensaje-exito" role="status">
          {mensajeExito}
        </div>
      )}

      {ofertas.length === 0 ? (
        <div className="sin-resultados">
          <p>No tiene ofertas publicadas aun.</p>
          <button
            className="boton-primario"
            onClick={() => navegar('/empresa/ofertas/nueva')}
          >
            Crear Primera Oferta
          </button>
        </div>
      ) : (
        <div className="grilla-tarjetas">
          {ofertas.map(oferta => (
            <article key={oferta.id} className="tarjeta-oferta-empresa">
              <div className="cabecera-tarjeta-oferta">
                <h2 className="titulo-oferta">{oferta.titulo}</h2>
                <span className={`etiqueta-estado-oferta estado-oferta-${oferta.estado}`}>
                  {etiquetasEstadoOferta[oferta.estado] || 'Desconocido'}
                </span>
              </div>

              <p className="ubicacion-oferta">{oferta.ubicacion}</p>

              {oferta.salario && (
                <p className="salario-oferta">
                  ${Number(oferta.salario).toLocaleString('es-AR')} / mes
                </p>
              )}

              {oferta.fechaCierre && (
                <p className="fecha-cierre-oferta">
                  Cierre: {new Date(oferta.fechaCierre).toLocaleDateString('es-AR')}
                </p>
              )}

              <p className="descripcion-oferta">{oferta.descripcion}</p>

              {oferta.requisitos && oferta.requisitos.length > 0 && (
                <p className="cantidad-requisitos">
                  {oferta.requisitos.length} requisito{oferta.requisitos.length !== 1 ? 's' : ''} definido{oferta.requisitos.length !== 1 ? 's' : ''}
                </p>
              )}

              <div className="acciones-tarjeta">
                {/* Ver candidatos */}
                <button
                  className="boton-primario"
                  onClick={() => navegar(`/empresa/ofertas/${oferta.id}/candidatos`)}
                >
                  Ver Candidatos
                </button>

                {/* Publicar si esta en borrador */}
                {oferta.estado === 0 && (
                  <button
                    className="boton-secundario"
                    onClick={() => manejarCambioEstado(oferta.id, 1)}
                  >
                    Publicar
                  </button>
                )}

                {/* Cerrar si esta publicada */}
                {oferta.estado === 1 && (
                  <button
                    className="boton-secundario"
                    onClick={() => manejarCambioEstado(oferta.id, 3)}
                  >
                    Cerrar Convocatoria
                  </button>
                )}

                {/* Eliminar */}
                <button
                  className="boton-eliminar"
                  onClick={() => manejarEliminar(oferta.id)}
                >
                  Eliminar
                </button>
              </div>
            </article>
          ))}
        </div>
      )}
    </div>
  );
}
