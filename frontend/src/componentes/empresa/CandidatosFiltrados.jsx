// Componente para visualizar candidatos filtrados de una oferta.
// Muestra los postulantes con su informacion y permite acceder al modulo de feedback.

import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { obtenerPostulantesOferta, obtenerOferta } from '../../api/clienteApi';
import { useContextoEmpresa } from '../../contexto/ContextoEmpresa';
import CargandoSpinner from '../comunes/CargandoSpinner';
import MensajeError from '../comunes/MensajeError';

// Etiquetas legibles para los estados de postulacion
const etiquetasEstado = {
  0: 'Pendiente',
  1: 'Aceptada',
  2: 'Rechazada',
};

export default function CandidatosFiltrados() {
  const { ofertaId } = useParams();
  const navegar = useNavigate();
  const { empresaActual } = useContextoEmpresa();

  const [postulantes, setPostulantes] = useState([]);
  const [oferta, setOferta] = useState(null);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState('');

  // Filtro de estado activo
  const [filtroEstado, setFiltroEstado] = useState('todos');

  useEffect(() => {
    if (!empresaActual) {
      navegar('/empresa/registro');
      return;
    }
    cargarDatos();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [empresaActual, ofertaId]);

  async function cargarDatos() {
    setCargando(true);
    setError('');
    try {
      const [listaPostulantes, datosOferta] = await Promise.all([
        obtenerPostulantesOferta(ofertaId),
        obtenerOferta(ofertaId),
      ]);
      setPostulantes(listaPostulantes);
      setOferta(datosOferta);
    } catch {
      setError('No se pudieron cargar los candidatos de esta oferta.');
    } finally {
      setCargando(false);
    }
  }

  // Aplica el filtro de estado sobre la lista de postulantes
  const candidatosFiltrados =
    filtroEstado === 'todos'
      ? postulantes
      : postulantes.filter(p => String(p.estado) === filtroEstado);

  if (cargando) return <CargandoSpinner />;

  return (
    <div className="contenedor-pagina">
      <button className="boton-volver" onClick={() => navegar('/empresa/ofertas')}>
        ← Volver a mis ofertas
      </button>

      <div className="encabezado-panel">
        <div>
          <h1>Candidatos Postulados</h1>
          {oferta && (
            <p className="descripcion-seccion">
              Oferta: <strong>{oferta.titulo}</strong> — {oferta.ubicacion}
            </p>
          )}
        </div>
        <span className="contador-candidatos">
          {postulantes.length} candidato{postulantes.length !== 1 ? 's' : ''} en total
        </span>
      </div>

      <MensajeError mensaje={error} />

      {/* Filtro por estado */}
      <div className="filtros-candidatos">
        <label htmlFor="filtroEstado">Filtrar por estado:</label>
        <select
          id="filtroEstado"
          value={filtroEstado}
          onChange={e => setFiltroEstado(e.target.value)}
          className="selector-filtro"
        >
          <option value="todos">Todos ({postulantes.length})</option>
          <option value="0">
            Pendientes ({postulantes.filter(p => p.estado === 0).length})
          </option>
          <option value="1">
            Aceptados ({postulantes.filter(p => p.estado === 1).length})
          </option>
          <option value="2">
            Rechazados ({postulantes.filter(p => p.estado === 2).length})
          </option>
        </select>
      </div>

      {candidatosFiltrados.length === 0 ? (
        <div className="sin-resultados">
          <p>
            {postulantes.length === 0
              ? 'Aun no hay candidatos postulados a esta oferta.'
              : 'No hay candidatos con el estado seleccionado.'}
          </p>
        </div>
      ) : (
        <div className="grilla-tarjetas">
          {candidatosFiltrados.map(postulante => (
            <article key={postulante.id} className="tarjeta-candidato">
              <h2 className="nombre-candidato">{postulante.nombreJoven}</h2>

              <p className="fecha-postulacion">
                Postulado el:{' '}
                {new Date(postulante.fechaPostulacion).toLocaleDateString('es-AR')}
              </p>

              <p className={`estado-postulacion estado-${postulante.estado}`}>
                Estado: <strong>{etiquetasEstado[postulante.estado] || 'Desconocido'}</strong>
              </p>

              {/* Accion para enviar feedback solo si la postulacion esta pendiente */}
              <div className="acciones-tarjeta">
                <button
                  className="boton-primario"
                  onClick={() =>
                    navegar(
                      `/empresa/ofertas/${ofertaId}/candidatos/${postulante.id}/feedback`
                    )
                  }
                >
                  {postulante.estado === 0 ? 'Enviar Feedback' : 'Ver Detalle'}
                </button>
              </div>
            </article>
          ))}
        </div>
      )}
    </div>
  );
}
