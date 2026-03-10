// Componente de detalle de oferta y postulacion.
// Muestra la informacion completa de la oferta, evalua la elegibilidad del joven
// y permite postularse o ver los cursos sugeridos para completar los requisitos.

import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  obtenerOferta,
  evaluarPostulacion,
  postularAOferta,
} from '../api/clienteApi';
import { useContextoJoven } from '../contexto/ContextoJoven';
import CargandoSpinner from './comunes/CargandoSpinner';
import MensajeError from './comunes/MensajeError';

export default function DetalleOferta() {
  const { ofertaId } = useParams();
  const { jovenActual } = useContextoJoven();
  const navegar = useNavigate();

  const [oferta, setOferta] = useState(null);

  // Resultado de la evaluacion de elegibilidad del joven para la oferta
  const [evaluacion, setEvaluacion] = useState(null);

  const [cargando, setCargando] = useState(true);
  const [evaluando, setEvaluando] = useState(false);
  const [postulando, setPostulando] = useState(false);
  const [error, setError] = useState('');
  const [mensajeExito, setMensajeExito] = useState('');

  useEffect(() => {
    if (!jovenActual) {
      navegar('/registro');
      return;
    }
    cargarOferta();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [ofertaId, jovenActual]);

  async function cargarOferta() {
    setCargando(true);
    setError('');
    try {
      const datosOferta = await obtenerOferta(ofertaId);
      setOferta(datosOferta);
    } catch {
      setError('No se pudo cargar el detalle de la oferta.');
    } finally {
      setCargando(false);
    }
  }

  /** Consulta a la API si el joven cumple los requisitos de la oferta */
  async function manejarEvaluacion() {
    setEvaluando(true);
    setError('');
    setEvaluacion(null);
    try {
      const resultado = await evaluarPostulacion(jovenActual.id, ofertaId);
      setEvaluacion(resultado);
    } catch {
      setError('No se pudo evaluar su postulacion. Intente nuevamente.');
    } finally {
      setEvaluando(false);
    }
  }

  /** Envia la postulacion a la oferta */
  async function manejarPostulacion() {
    setPostulando(true);
    setError('');
    setMensajeExito('');
    try {
      await postularAOferta(jovenActual.id, ofertaId);
      setMensajeExito('Postulacion enviada correctamente. La empresa revisara su perfil.');
    } catch (err) {
      const mensajeError =
        err.response?.data?.message ||
        err.response?.data ||
        'No se pudo enviar la postulacion.';
      setError(typeof mensajeError === 'string' ? mensajeError : JSON.stringify(mensajeError));
    } finally {
      setPostulando(false);
    }
  }

  if (cargando) return <CargandoSpinner />;
  if (!oferta) return <MensajeError mensaje={error || 'Oferta no encontrada.'} />;

  return (
    <div className="contenedor-pagina">
      <button className="boton-volver" onClick={() => navegar('/ofertas')}>
        Volver a Ofertas
      </button>

      <article className="detalle-oferta">
        <h1>{oferta.titulo}</h1>
        <p className="empresa-oferta">{oferta.empresaNombre || oferta.empresa}</p>
        <p className="ubicacion-oferta">{oferta.ubicacion}</p>

        {oferta.salario && (
          <p className="salario-oferta">
            Salario: ${Number(oferta.salario).toLocaleString('es-AR')}
          </p>
        )}

        {oferta.fechaCierre && (
          <p className="fecha-cierre-oferta">
            Fecha de cierre: {new Date(oferta.fechaCierre).toLocaleDateString('es-AR')}
          </p>
        )}

        <section>
          <h2>Descripcion del Puesto</h2>
          <p>{oferta.descripcion}</p>
        </section>

        {oferta.requisitos && oferta.requisitos.length > 0 && (
          <section>
            <h2>Requisitos</h2>
            <ul className="lista-requisitos">
              {oferta.requisitos.map(requisito => (
                <li key={requisito.id} className="item-requisito">
                  <strong>{requisito.habilidadNombre || requisito.habilidad}</strong>
                  {requisito.tipoRequisito === 0 && (
                    <span className="etiqueta-obligatorio"> (Obligatorio)</span>
                  )}
                  {requisito.descripcion && <p>{requisito.descripcion}</p>}
                </li>
              ))}
            </ul>
          </section>
        )}
      </article>

      <MensajeError mensaje={error} />

      {mensajeExito && (
        <div className="mensaje-exito" role="status">
          {mensajeExito}
        </div>
      )}

      {/* Seccion de evaluacion de elegibilidad */}
      {!mensajeExito && (
        <section className="seccion-postulacion">
          <h2>Verificar Elegibilidad</h2>
          <p>
            Verifique si cumple con los requisitos de esta oferta antes de postularse.
          </p>

          {!evaluacion && (
            <button
              className="boton-secundario"
              onClick={manejarEvaluacion}
              disabled={evaluando}
            >
              {evaluando ? 'Evaluando...' : 'Verificar mis Habilidades'}
            </button>
          )}

          {evaluacion && (
            <div className="resultado-evaluacion">
              {evaluacion.puedePostularse ? (
                <div>
                  <p className="evaluacion-aprobada">
                    Cumple con los requisitos de esta oferta.
                  </p>
                  <button
                    className="boton-primario"
                    onClick={manejarPostulacion}
                    disabled={postulando}
                  >
                    {postulando ? 'Enviando postulacion...' : 'Postularme Ahora'}
                  </button>
                </div>
              ) : (
                <div>
                  <p className="evaluacion-rechazada">
                    Aun no cumple con todos los requisitos obligatorios para esta oferta.
                  </p>

                  {evaluacion.habilidadesFaltantes && evaluacion.habilidadesFaltantes.length > 0 && (
                    <div>
                      <h3>Habilidades que le faltan</h3>
                      <ul className="lista-brechas">
                        {evaluacion.habilidadesFaltantes.map((habilidad, indice) => (
                          <li key={indice}>{habilidad.nombre || habilidad}</li>
                        ))}
                      </ul>
                    </div>
                  )}

                  {evaluacion.cursosSugeridos && evaluacion.cursosSugeridos.length > 0 && (
                    <div>
                      <h3>Cursos Sugeridos para Completar su Perfil</h3>
                      <ul className="lista-cursos-sugeridos">
                        {evaluacion.cursosSugeridos.map(curso => (
                          <li key={curso.id} className="item-curso-sugerido">
                            <strong>{curso.titulo}</strong>
                            <p>{curso.descripcion}</p>
                            <button
                              className="boton-secundario"
                              onClick={() => navegar(`/cursos/${curso.id}`)}
                            >
                              Ver Curso
                            </button>
                          </li>
                        ))}
                      </ul>
                    </div>
                  )}
                </div>
              )}
            </div>
          )}
        </section>
      )}
    </div>
  );
}
