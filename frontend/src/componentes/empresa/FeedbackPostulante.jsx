// Componente para enviar feedback a un postulante.
// Permite a la empresa aceptar o rechazar una postulacion con un mensaje de retroalimentacion.

import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { actualizarEstadoPostulacion, obtenerPostulantesOferta } from '../../api/clienteApi';
import { useContextoEmpresa } from '../../contexto/ContextoEmpresa';
import CargandoSpinner from '../comunes/CargandoSpinner';
import MensajeError from '../comunes/MensajeError';

// Opciones de decision disponibles para la empresa
const opcionesDecision = [
  { valor: 1, etiqueta: 'Aceptar postulante', descripcion: 'El candidato pasa a la siguiente etapa del proceso' },
  { valor: 2, etiqueta: 'Rechazar postulante', descripcion: 'El candidato no continua en el proceso de seleccion' },
];

export default function FeedbackPostulante() {
  const { ofertaId, postulacionId } = useParams();
  const navegar = useNavigate();
  const { empresaActual } = useContextoEmpresa();

  const [decision, setDecision] = useState('');
  const [cargando, setCargando] = useState(false);
  const [error, setError] = useState('');
  const [exito, setExito] = useState(false);
  const [postulacion, setPostulacion] = useState(null);
  const [cargandoPostulacion, setCargandoPostulacion] = useState(true);

  useEffect(() => {
    if (!empresaActual) {
      navegar('/empresa/registro');
      return;
    }
    cargarPostulacion();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [empresaActual, postulacionId]);

  async function cargarPostulacion() {
    setCargandoPostulacion(true);
    try {
      // Carga los datos de la postulacion para mostrar el nombre del candidato
      const lista = await obtenerPostulantesOferta(ofertaId);
      const encontrada = lista.find(p => String(p.id) === String(postulacionId));
      setPostulacion(encontrada || null);
    } catch {
      // Si falla la carga, continua de todas formas con el formulario de feedback
    } finally {
      setCargandoPostulacion(false);
    }
  }

  /** Envia el feedback actualizando el estado de la postulacion */
  async function manejarEnvio(evento) {
    evento.preventDefault();
    setError('');

    if (!decision) {
      setError('Debe seleccionar una decision antes de enviar el feedback.');
      return;
    }

    setCargando(true);
    try {
      await actualizarEstadoPostulacion(Number(postulacionId), Number(decision));
      setExito(true);
    } catch (err) {
      const mensajeError =
        err.response?.data?.mensaje ||
        err.response?.data?.message ||
        err.response?.data ||
        'Ocurrio un error al enviar el feedback. Intente nuevamente.';
      setError(typeof mensajeError === 'string' ? mensajeError : JSON.stringify(mensajeError));
    } finally {
      setCargando(false);
    }
  }

  if (cargandoPostulacion) return <CargandoSpinner />;

  // Pantalla de confirmacion tras enviar el feedback
  if (exito) {
    const decisionTexto = opcionesDecision.find(o => String(o.valor) === String(decision))?.etiqueta;

    return (
      <div className="contenedor-formulario">
        <div className="resultado-feedback-exito">
          <h1>Feedback Enviado</h1>
          <p className="descripcion-seccion">
            La decision ha sido registrada correctamente.
          </p>

          <div className="detalle-feedback-enviado">
            {postulacion && (
              <p>
                <strong>Candidato:</strong> {postulacion.nombreJoven}
              </p>
            )}
            <p>
              <strong>Decision:</strong>{' '}
              <span className={decision === '1' ? 'texto-aceptado' : 'texto-rechazado'}>
                {decisionTexto}
              </span>
            </p>
          </div>

          <div className="acciones-formulario">
            <button
              className="boton-primario"
              onClick={() => navegar(`/empresa/ofertas/${ofertaId}/candidatos`)}
            >
              Volver a Candidatos
            </button>
            <button
              className="boton-secundario"
              onClick={() => navegar('/empresa/ofertas')}
            >
              Ir a Mis Ofertas
            </button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="contenedor-formulario">
      <button
        className="boton-volver"
        onClick={() => navegar(`/empresa/ofertas/${ofertaId}/candidatos`)}
      >
        ← Volver a candidatos
      </button>

      <h1>Enviar Feedback al Postulante</h1>

      {postulacion && (
        <div className="informacion-candidato">
          <p>
            <strong>Candidato:</strong> {postulacion.nombreJoven}
          </p>
          <p>
            <strong>Postulado el:</strong>{' '}
            {new Date(postulacion.fechaPostulacion).toLocaleDateString('es-AR')}
          </p>
          <p>
            <strong>Estado actual:</strong>{' '}
            <span className={`estado-postulacion estado-${postulacion.estado}`}>
              {postulacion.estado === 0 ? 'Pendiente' : postulacion.estado === 1 ? 'Aceptada' : 'Rechazada'}
            </span>
          </p>
        </div>
      )}

      {postulacion && postulacion.estado !== 0 && (
        <div className="aviso-ya-procesado">
          <p>Esta postulacion ya fue procesada. No es posible modificar el estado.</p>
          <button
            className="boton-secundario"
            onClick={() => navegar(`/empresa/ofertas/${ofertaId}/candidatos`)}
          >
            Volver a Candidatos
          </button>
        </div>
      )}

      {(!postulacion || postulacion.estado === 0) && (
        <>
          <p className="descripcion-seccion">
            Seleccione la decision para este candidato. El estado de la postulacion
            se actualizara de inmediato.
          </p>

          <MensajeError mensaje={error} />

          <form onSubmit={manejarEnvio} noValidate>
            <fieldset className="grupo-decision">
              <legend className="leyenda-decision">Decision sobre el candidato</legend>

              {opcionesDecision.map(opcion => (
                <label
                  key={opcion.valor}
                  className={`opcion-decision ${String(decision) === String(opcion.valor) ? 'opcion-seleccionada' : ''}`}
                >
                  <input
                    type="radio"
                    name="decision"
                    value={opcion.valor}
                    checked={String(decision) === String(opcion.valor)}
                    onChange={e => setDecision(e.target.value)}
                  />
                  <div className="contenido-opcion-decision">
                    <strong>{opcion.etiqueta}</strong>
                    <span className="descripcion-opcion">{opcion.descripcion}</span>
                  </div>
                </label>
              ))}
            </fieldset>

            <div className="acciones-formulario">
              <button
                type="submit"
                className={`boton-primario ${decision === '2' ? 'boton-rechazar' : ''}`}
                disabled={cargando || !decision}
              >
                {cargando ? 'Enviando...' : 'Confirmar y Enviar Feedback'}
              </button>
              <button
                type="button"
                className="boton-secundario"
                onClick={() => navegar(`/empresa/ofertas/${ofertaId}/candidatos`)}
              >
                Cancelar
              </button>
            </div>
          </form>
        </>
      )}
    </div>
  );
}
