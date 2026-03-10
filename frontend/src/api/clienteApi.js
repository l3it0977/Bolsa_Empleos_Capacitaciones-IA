// Modulo central de llamadas a la API REST del backend.
// Todas las funciones retornan la data directamente o lanzan un error con mensaje.

import axios from 'axios';

// Instancia de axios apuntando al backend .NET
const instanciaApi = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5000',
  headers: { 'Content-Type': 'application/json' },
});

// ---------------------------------------------------------------
// JOVENES
// ---------------------------------------------------------------

/** Registra un nuevo joven en el sistema */
export async function registrarJoven(datosJoven) {
  const respuesta = await instanciaApi.post('/api/jovenes', datosJoven);
  return respuesta.data;
}

/** Obtiene el perfil de un joven por su id */
export async function obtenerJoven(jovenId) {
  const respuesta = await instanciaApi.get(`/api/jovenes/${jovenId}`);
  return respuesta.data;
}

/** Actualiza los datos de un joven */
export async function actualizarJoven(jovenId, datosJoven) {
  const respuesta = await instanciaApi.put(`/api/jovenes/${jovenId}`, datosJoven);
  return respuesta.data;
}

// ---------------------------------------------------------------
// CURRICULUM
// ---------------------------------------------------------------

/** Obtiene el curriculum de un joven */
export async function obtenerCurriculum(jovenId) {
  const respuesta = await instanciaApi.get(`/api/jovenes/${jovenId}/curriculum`);
  return respuesta.data;
}

/** Crea o actualiza el curriculum de un joven */
export async function guardarCurriculum(jovenId, datosCurriculum) {
  const respuesta = await instanciaApi.put(`/api/jovenes/${jovenId}/curriculum`, datosCurriculum);
  return respuesta.data;
}

/** Agrega una habilidad manualmente al curriculum */
export async function agregarHabilidadCurriculum(jovenId, habilidadId) {
  const respuesta = await instanciaApi.post(
    `/api/jovenes/${jovenId}/curriculum/habilidades/${habilidadId}`
  );
  return respuesta.data;
}

// ---------------------------------------------------------------
// HABILIDADES
// ---------------------------------------------------------------

/** Obtiene todas las habilidades activas */
export async function obtenerHabilidades() {
  const respuesta = await instanciaApi.get('/api/habilidades');
  return respuesta.data;
}

/** Busca habilidades por nombre */
export async function buscarHabilidades(nombre) {
  const respuesta = await instanciaApi.get(`/api/habilidades/buscar?nombre=${encodeURIComponent(nombre)}`);
  return respuesta.data;
}

// ---------------------------------------------------------------
// OFERTAS DE TRABAJO
// ---------------------------------------------------------------

/** Obtiene todas las ofertas publicadas */
export async function obtenerOfertas() {
  const respuesta = await instanciaApi.get('/api/ofertas-trabajo');
  return respuesta.data;
}

/** Obtiene el detalle de una oferta por su id */
export async function obtenerOferta(ofertaId) {
  const respuesta = await instanciaApi.get(`/api/ofertas-trabajo/${ofertaId}`);
  return respuesta.data;
}

// ---------------------------------------------------------------
// POSTULACIONES
// ---------------------------------------------------------------

/** Evalua si un joven cumple los requisitos para una oferta y muestra brechas */
export async function evaluarPostulacion(jovenId, ofertaId) {
  const respuesta = await instanciaApi.get(
    `/api/postulaciones/evaluar/joven/${jovenId}/oferta/${ofertaId}`
  );
  return respuesta.data;
}

/** Envia la postulacion de un joven a una oferta */
export async function postularAOferta(jovenId, ofertaId) {
  const respuesta = await instanciaApi.post(
    `/api/postulaciones/joven/${jovenId}/oferta/${ofertaId}`
  );
  return respuesta.data;
}

/** Obtiene todas las postulaciones de un joven */
export async function obtenerPostulacionesJoven(jovenId) {
  const respuesta = await instanciaApi.get(`/api/postulaciones/joven/${jovenId}`);
  return respuesta.data;
}

// ---------------------------------------------------------------
// CURSOS
// ---------------------------------------------------------------

/** Obtiene todos los cursos activos */
export async function obtenerCursos() {
  const respuesta = await instanciaApi.get('/api/cursos');
  return respuesta.data;
}

/** Obtiene el detalle de un curso por su id */
export async function obtenerCurso(cursoId) {
  const respuesta = await instanciaApi.get(`/api/cursos/${cursoId}`);
  return respuesta.data;
}

/** Obtiene los cursos asociados a una habilidad */
export async function obtenerCursosPorHabilidad(habilidadId) {
  const respuesta = await instanciaApi.get(`/api/cursos/habilidad/${habilidadId}`);
  return respuesta.data;
}

// ---------------------------------------------------------------
// EVALUACIONES
// ---------------------------------------------------------------

/** Obtiene todas las evaluaciones de un joven */
export async function obtenerEvaluacionesJoven(jovenId) {
  const respuesta = await instanciaApi.get(`/api/evaluaciones/joven/${jovenId}`);
  return respuesta.data;
}

/** Inicia una evaluacion para un joven en un curso */
export async function iniciarEvaluacion(jovenId, cursoId) {
  const respuesta = await instanciaApi.post(`/api/evaluaciones/joven/${jovenId}/curso/${cursoId}`);
  return respuesta.data;
}

// ---------------------------------------------------------------
// EVALUACION IA
// ---------------------------------------------------------------

/** Genera preguntas de evaluacion para un curso usando IA */
export async function obtenerPreguntasIA(cursoId, cantidad = 5) {
  const respuesta = await instanciaApi.get(
    `/api/evaluacion-ia/cursos/${cursoId}/preguntas?cantidad=${cantidad}`
  );
  return respuesta.data;
}

/** Envia las respuestas al motor de IA para evaluacion y obtencion del puntaje */
export async function evaluarRespuestasIA(solicitud) {
  const respuesta = await instanciaApi.post('/api/evaluacion-ia/evaluar', solicitud);
  return respuesta.data;
}

// ---------------------------------------------------------------
// EMPRESAS
// ---------------------------------------------------------------

/** Registra una nueva empresa en el sistema */
export async function registrarEmpresa(datosEmpresa) {
  const respuesta = await instanciaApi.post('/api/empresas', datosEmpresa);
  return respuesta.data;
}

/** Obtiene el perfil de una empresa por su id */
export async function obtenerEmpresa(empresaId) {
  const respuesta = await instanciaApi.get(`/api/empresas/${empresaId}`);
  return respuesta.data;
}

/** Actualiza los datos de una empresa */
export async function actualizarEmpresa(empresaId, datosEmpresa) {
  const respuesta = await instanciaApi.put(`/api/empresas/${empresaId}`, datosEmpresa);
  return respuesta.data;
}

/** Obtiene todas las ofertas de una empresa */
export async function obtenerOfertasPorEmpresa(empresaId) {
  const respuesta = await instanciaApi.get(`/api/ofertas-trabajo/empresa/${empresaId}`);
  return respuesta.data;
}

/** Crea una nueva oferta para una empresa */
export async function crearOferta(empresaId, datosOferta) {
  const respuesta = await instanciaApi.post(`/api/ofertas-trabajo/empresa/${empresaId}`, datosOferta);
  return respuesta.data;
}

/** Cambia el estado de una oferta */
export async function cambiarEstadoOferta(ofertaId, nuevoEstado) {
  const respuesta = await instanciaApi.patch(`/api/ofertas-trabajo/${ofertaId}/estado`, nuevoEstado);
  return respuesta.data;
}

/** Elimina una oferta */
export async function eliminarOferta(ofertaId) {
  await instanciaApi.delete(`/api/ofertas-trabajo/${ofertaId}`);
}

/** Obtiene los postulantes de una oferta */
export async function obtenerPostulantesOferta(ofertaId) {
  const respuesta = await instanciaApi.get(`/api/postulaciones/oferta/${ofertaId}`);
  return respuesta.data;
}

/** Actualiza el estado de una postulacion (envio de feedback) */
export async function actualizarEstadoPostulacion(postulacionId, nuevoEstado) {
  const respuesta = await instanciaApi.patch(`/api/postulaciones/${postulacionId}/estado`, nuevoEstado);
  return respuesta.data;
}
