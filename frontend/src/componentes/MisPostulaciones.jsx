// Componente para ver el historial de postulaciones del joven.

import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { obtenerPostulacionesJoven } from '../api/clienteApi';
import { useContextoJoven } from '../contexto/ContextoJoven';
import CargandoSpinner from './comunes/CargandoSpinner';
import MensajeError from './comunes/MensajeError';

// Etiquetas legibles para los estados de postulacion del backend
const etiquetasEstadoPostulacion = {
  0: 'Pendiente',
  1: 'En Revision',
  2: 'Aceptado',
  3: 'Rechazado',
};

export default function MisPostulaciones() {
  const { jovenActual } = useContextoJoven();
  const navegar = useNavigate();

  const [postulaciones, setPostulaciones] = useState([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!jovenActual) {
      navegar('/registro');
      return;
    }
    cargarPostulaciones();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [jovenActual]);

  async function cargarPostulaciones() {
    setCargando(true);
    setError('');
    try {
      const listaPostulaciones = await obtenerPostulacionesJoven(jovenActual.id);
      setPostulaciones(listaPostulaciones);
    } catch {
      setError('No se pudieron cargar las postulaciones.');
    } finally {
      setCargando(false);
    }
  }

  if (cargando) return <CargandoSpinner />;

  return (
    <div className="contenedor-pagina">
      <h1>Mis Postulaciones</h1>
      <p className="descripcion-seccion">
        Seguimiento de todas las ofertas a las que se ha postulado.
      </p>

      <MensajeError mensaje={error} />

      {postulaciones.length === 0 ? (
        <div className="sin-resultados">
          <p>No tiene postulaciones registradas.</p>
          <button className="boton-primario" onClick={() => navegar('/ofertas')}>
            Explorar Ofertas
          </button>
        </div>
      ) : (
        <div className="grilla-tarjetas">
          {postulaciones.map(postulacion => (
            <article key={postulacion.id} className="tarjeta-postulacion">
              <h2>{postulacion.tituloOferta || postulacion.ofertaTrabajo?.titulo}</h2>
              <p className="empresa-postulacion">
                {postulacion.empresaNombre || postulacion.ofertaTrabajo?.empresa}
              </p>

              <p className="fecha-postulacion">
                Postulado el:{' '}
                {new Date(postulacion.fechaPostulacion).toLocaleDateString('es-AR')}
              </p>

              <p className={`estado-postulacion estado-${postulacion.estado}`}>
                Estado: {etiquetasEstadoPostulacion[postulacion.estado] || 'Desconocido'}
              </p>

              <button
                className="boton-secundario"
                onClick={() => navegar(`/ofertas/${postulacion.ofertaTrabajoId || postulacion.ofertaTrabajo?.id}`)}
              >
                Ver Oferta
              </button>
            </article>
          ))}
        </div>
      )}
    </div>
  );
}
