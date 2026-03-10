// Componente para listar los cursos de capacitacion disponibles.
// Muestra los cursos con su habilidad asociada y permite iniciar el examen.

import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { obtenerCursos, obtenerEvaluacionesJoven } from '../api/clienteApi';
import { useContextoJoven } from '../contexto/ContextoJoven';
import CargandoSpinner from './comunes/CargandoSpinner';
import MensajeError from './comunes/MensajeError';

export default function ListaCursos() {
  const { jovenActual } = useContextoJoven();
  const navegar = useNavigate();

  const [cursos, setCursos] = useState([]);
  const [evaluaciones, setEvaluaciones] = useState([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!jovenActual) {
      navegar('/registro');
      return;
    }
    cargarDatos();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [jovenActual]);

  async function cargarDatos() {
    setCargando(true);
    setError('');
    try {
      const [listaCursos, listaEvaluaciones] = await Promise.all([
        obtenerCursos(),
        obtenerEvaluacionesJoven(jovenActual.id).catch(() => []),
      ]);
      setCursos(listaCursos);
      setEvaluaciones(listaEvaluaciones);
    } catch {
      setError('No se pudieron cargar los cursos disponibles.');
    } finally {
      setCargando(false);
    }
  }

  /**
   * Busca el estado de la evaluacion del joven para un curso determinado.
   * Retorna el objeto de evaluacion o null si no existe.
   */
  function obtenerEvaluacionDeCurso(cursoId) {
    return evaluaciones.find(e => e.cursoId === cursoId) || null;
  }

  /** Determina el texto de estado visible para la evaluacion */
  function etiquetaEstadoEvaluacion(evaluacion) {
    if (!evaluacion) return null;
    const estados = {
      0: 'Pendiente',
      1: 'En Progreso',
      2: 'Aprobado',
      3: 'Desaprobado',
    };
    return estados[evaluacion.estado] || 'Desconocido';
  }

  if (cargando) return <CargandoSpinner />;

  return (
    <div className="contenedor-pagina">
      <h1>Cursos de Capacitacion</h1>
      <p className="descripcion-seccion">
        Complete cursos para adquirir nuevas habilidades y mejorar su perfil laboral.
        Al aprobar el examen de cada curso, la habilidad se agrega automaticamente a su curriculum.
      </p>

      <MensajeError mensaje={error} />

      {cursos.length === 0 ? (
        <div className="sin-resultados">
          <p>No hay cursos disponibles en este momento.</p>
        </div>
      ) : (
        <div className="grilla-tarjetas">
          {cursos.map(curso => {
            const evaluacionExistente = obtenerEvaluacionDeCurso(curso.id);
            const estadoEtiqueta = etiquetaEstadoEvaluacion(evaluacionExistente);

            return (
              <article key={curso.id} className="tarjeta-curso">
                <h2 className="titulo-curso">{curso.titulo}</h2>

                {curso.habilidadNombre && (
                  <p className="habilidad-curso">
                    Habilidad: <strong>{curso.habilidadNombre}</strong>
                  </p>
                )}

                <p className="duracion-curso">
                  Duracion: {curso.duracionHoras} hora{curso.duracionHoras !== 1 ? 's' : ''}
                </p>

                <p className="descripcion-curso">{curso.descripcion}</p>

                <p className="puntaje-minimo">
                  Puntaje minimo para aprobar: {curso.puntajeMinimoAprobacion}%
                </p>

                {estadoEtiqueta && (
                  <p className={`estado-evaluacion estado-${evaluacionExistente.estado}`}>
                    Estado: {estadoEtiqueta}
                    {evaluacionExistente.puntajeObtenido !== null &&
                      evaluacionExistente.puntajeObtenido !== undefined && (
                        <span> ({evaluacionExistente.puntajeObtenido}%)</span>
                      )}
                  </p>
                )}

                <div className="acciones-tarjeta">
                  {curso.urlMaterial && (
                    <a
                      href={curso.urlMaterial}
                      target="_blank"
                      rel="noopener noreferrer"
                      className="enlace-material"
                    >
                      Ver Material del Curso
                    </a>
                  )}

                  <button
                    className="boton-primario"
                    onClick={() => navegar(`/cursos/${curso.id}/examen`)}
                  >
                    {evaluacionExistente ? 'Rendir Examen de Nuevo' : 'Rendir Examen'}
                  </button>
                </div>
              </article>
            );
          })}
        </div>
      )}
    </div>
  );
}
