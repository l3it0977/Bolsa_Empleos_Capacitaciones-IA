// Componente de rendicion de examen de un curso.
// Obtiene preguntas generadas por IA, permite responderlas y envia las respuestas
// para evaluacion. Si se aprueba, la habilidad se agrega automaticamente al CV.

import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  obtenerCurso,
  iniciarEvaluacion,
  obtenerPreguntasIA,
  evaluarRespuestasIA,
} from '../api/clienteApi';
import { useContextoJoven } from '../contexto/ContextoJoven';
import CargandoSpinner from './comunes/CargandoSpinner';
import MensajeError from './comunes/MensajeError';

export default function ExamenCurso() {
  const { cursoId } = useParams();
  const { jovenActual } = useContextoJoven();
  const navegar = useNavigate();

  const [curso, setCurso] = useState(null);
  const [evaluacion, setEvaluacion] = useState(null);

  // Preguntas generadas por la IA para el examen
  const [preguntas, setPreguntas] = useState([]);

  // Respuestas del joven indexadas por posicion de pregunta
  const [respuestas, setRespuestas] = useState({});

  // Resultado final del examen luego de enviarlo
  const [resultado, setResultado] = useState(null);

  const [cargando, setCargando] = useState(true);
  const [enviando, setEnviando] = useState(false);
  const [error, setError] = useState('');

  // Etapa actual: 'preparacion' | 'examen' | 'resultado'
  const [etapa, setEtapa] = useState('preparacion');

  useEffect(() => {
    if (!jovenActual) {
      navegar('/registro');
      return;
    }
    cargarCurso();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [cursoId, jovenActual]);

  async function cargarCurso() {
    setCargando(true);
    setError('');
    try {
      const datosCurso = await obtenerCurso(cursoId);
      setCurso(datosCurso);
    } catch {
      setError('No se pudo cargar el curso.');
    } finally {
      setCargando(false);
    }
  }

  /** Inicia la evaluacion y carga las preguntas generadas por IA */
  async function manejarIniciarExamen() {
    setCargando(true);
    setError('');
    try {
      // Registra el inicio de la evaluacion en el backend
      const datosEvaluacion = await iniciarEvaluacion(jovenActual.id, cursoId);
      setEvaluacion(datosEvaluacion);

      // Solicita 5 preguntas al modulo de IA
      const listadoPreguntas = await obtenerPreguntasIA(cursoId, 5);
      setPreguntas(listadoPreguntas);
      setRespuestas({});
      setEtapa('examen');
    } catch {
      setError('No se pudo iniciar el examen. Intente nuevamente.');
    } finally {
      setCargando(false);
    }
  }

  /** Registra la respuesta del joven para una pregunta */
  function manejarCambioRespuesta(indicePregunta, valorRespuesta) {
    setRespuestas(anterior => ({ ...anterior, [indicePregunta]: valorRespuesta }));
  }

  /** Verifica que todas las preguntas hayan sido respondidas */
  function todasRespondidas() {
    return preguntas.every((_, indice) => respuestas[indice] !== undefined && respuestas[indice] !== '');
  }

  /** Envia las respuestas al motor de IA para evaluacion */
  async function manejarEnviarExamen() {
    setEnviando(true);
    setError('');
    try {
      // Construye la estructura requerida por el endpoint de evaluacion IA
      const solicitud = {
        jovenId: jovenActual.id,
        cursoId: Number(cursoId),
        evaluacionId: evaluacion?.id,
        respuestas: preguntas.map((pregunta, indice) => ({
          pregunta: pregunta.textoPregunta || pregunta.pregunta,
          respuestaJoven: respuestas[indice],
          respuestaCorrecta: pregunta.respuestaEsperada || pregunta.respuestaCorrecta || '',
        })),
      };

      const datosResultado = await evaluarRespuestasIA(solicitud);
      setResultado(datosResultado);
      setEtapa('resultado');
    } catch {
      setError('No se pudo evaluar el examen. Intente nuevamente.');
    } finally {
      setEnviando(false);
    }
  }

  if (cargando) return <CargandoSpinner />;

  if (etapa === 'preparacion') {
    return (
      <div className="contenedor-formulario">
        <button className="boton-volver" onClick={() => navegar('/cursos')}>
          Volver a Cursos
        </button>

        {curso && (
          <div>
            <h1>Examen: {curso.titulo}</h1>
            <p className="descripcion-seccion">{curso.descripcion}</p>

            <div className="informacion-examen">
              <p><strong>Habilidad que obtendra:</strong> {curso.habilidadNombre || 'No especificada'}</p>
              <p><strong>Duracion del curso:</strong> {curso.duracionHoras} hora(s)</p>
              <p><strong>Puntaje minimo para aprobar:</strong> {curso.puntajeMinimoAprobacion}%</p>
              <p><strong>Cantidad de preguntas:</strong> 5 preguntas generadas por IA</p>
            </div>

            {curso.urlMaterial && (
              <div className="aviso-material">
                <p>Se recomienda revisar el material del curso antes de rendir el examen.</p>
                <a
                  href={curso.urlMaterial}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="enlace-material"
                >
                  Acceder al Material del Curso
                </a>
              </div>
            )}

            <MensajeError mensaje={error} />

            <button className="boton-primario" onClick={manejarIniciarExamen}>
              Comenzar Examen
            </button>
          </div>
        )}
      </div>
    );
  }

  if (etapa === 'examen') {
    return (
      <div className="contenedor-formulario">
        <h1>Examen: {curso?.titulo}</h1>
        <p className="descripcion-seccion">
          Responda todas las preguntas y envie el examen para obtener su puntaje.
        </p>

        <MensajeError mensaje={error} />

        <form onSubmit={e => { e.preventDefault(); manejarEnviarExamen(); }}>
          <ol className="lista-preguntas">
            {preguntas.map((pregunta, indice) => (
              <li key={indice} className="item-pregunta">
                <p className="texto-pregunta">
                  {pregunta.textoPregunta || pregunta.pregunta}
                </p>

                {/* Si la pregunta tiene opciones multiples, muestra radio buttons */}
                {pregunta.opciones && pregunta.opciones.length > 0 ? (
                  <ul className="lista-opciones">
                    {pregunta.opciones.map((opcion, indiceOpcion) => (
                      <li key={indiceOpcion} className="opcion-respuesta">
                        <label>
                          <input
                            type="radio"
                            name={`pregunta-${indice}`}
                            value={opcion}
                            checked={respuestas[indice] === opcion}
                            onChange={() => manejarCambioRespuesta(indice, opcion)}
                          />
                          {opcion}
                        </label>
                      </li>
                    ))}
                  </ul>
                ) : (
                  // Si no hay opciones, muestra un campo de texto libre
                  <textarea
                    className="respuesta-abierta"
                    rows={3}
                    value={respuestas[indice] || ''}
                    onChange={e => manejarCambioRespuesta(indice, e.target.value)}
                    placeholder="Escriba su respuesta aqui"
                    required
                  />
                )}
              </li>
            ))}
          </ol>

          <div className="acciones-examen">
            <button
              type="submit"
              className="boton-primario"
              disabled={enviando || !todasRespondidas()}
            >
              {enviando ? 'Enviando examen...' : 'Enviar Examen'}
            </button>
          </div>
        </form>
      </div>
    );
  }

  if (etapa === 'resultado') {
    const aprobado = resultado?.aprobado || resultado?.puntajeObtenido >= (curso?.puntajeMinimoAprobacion || 70);

    return (
      <div className="contenedor-formulario">
        <h1>Resultado del Examen</h1>
        <p className="titulo-curso-resultado">Curso: {curso?.titulo}</p>

        <div className={`resultado-final ${aprobado ? 'resultado-aprobado' : 'resultado-desaprobado'}`}>
          <p className="puntaje-resultado">
            Puntaje obtenido: <strong>{resultado?.puntajeObtenido ?? '--'}%</strong>
          </p>

          <p className="estado-resultado">
            {aprobado
              ? 'Aprobado - La habilidad ha sido agregada a su curriculum.'
              : 'Desaprobado - Puede intentarlo nuevamente cuando lo desee.'}
          </p>

          {resultado?.retroalimentacion && (
            <div className="retroalimentacion">
              <h3>Retroalimentacion</h3>
              <p>{resultado.retroalimentacion}</p>
            </div>
          )}

          {resultado?.feedbackPorPregunta && resultado.feedbackPorPregunta.length > 0 && (
            <div className="feedback-preguntas">
              <h3>Detalle por Pregunta</h3>
              <ol>
                {resultado.feedbackPorPregunta.map((fb, indice) => (
                  <li key={indice} className={`item-feedback ${fb.esCorrecta ? 'correcto' : 'incorrecto'}`}>
                    <p><strong>Pregunta:</strong> {fb.pregunta}</p>
                    <p><strong>Su respuesta:</strong> {fb.respuestaJoven}</p>
                    {!fb.esCorrecta && fb.respuestaCorrecta && (
                      <p><strong>Respuesta correcta:</strong> {fb.respuestaCorrecta}</p>
                    )}
                    {fb.comentario && <p><em>{fb.comentario}</em></p>}
                  </li>
                ))}
              </ol>
            </div>
          )}
        </div>

        <div className="acciones-resultado">
          {!aprobado && (
            <button className="boton-secundario" onClick={() => setEtapa('preparacion')}>
              Intentar Nuevamente
            </button>
          )}
          <button className="boton-primario" onClick={() => navegar('/cursos')}>
            Volver a Cursos
          </button>
          {aprobado && (
            <button className="boton-primario" onClick={() => navegar('/ofertas')}>
              Ver Ofertas de Trabajo
            </button>
          )}
        </div>
      </div>
    );
  }

  return null;
}
