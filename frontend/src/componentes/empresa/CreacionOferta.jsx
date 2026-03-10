// Componente para crear una nueva oferta laboral.
// Permite definir titulo, descripcion, ubicacion, salario y requisitos (relevantes y no relevantes).

import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { crearOferta, obtenerHabilidades } from '../../api/clienteApi';
import { useContextoEmpresa } from '../../contexto/ContextoEmpresa';
import CargandoSpinner from '../comunes/CargandoSpinner';
import MensajeError from '../comunes/MensajeError';

// Tipos de requisito segun el dominio del backend
const tiposRequisito = [
  { valor: 0, etiqueta: 'Relevante', descripcion: 'El candidato debe cumplirlo obligatoriamente' },
  { valor: 1, etiqueta: 'No Relevante', descripcion: 'Deseable; se sugiere un curso si no lo tiene' },
];

export default function CreacionOferta() {
  const navegar = useNavigate();
  const { empresaActual } = useContextoEmpresa();

  // Estado del formulario principal
  const [formulario, setFormulario] = useState({
    titulo: '',
    descripcion: '',
    ubicacion: '',
    salario: '',
    fechaCierre: '',
  });

  // Lista de requisitos agregados a la oferta
  const [requisitos, setRequisitos] = useState([]);

  // Estado para agregar un nuevo requisito
  const [nuevoRequisito, setNuevoRequisito] = useState({
    habilidadId: '',
    tipoRequisito: 0,
    descripcion: '',
  });

  // Catalogo de habilidades disponibles
  const [habilidades, setHabilidades] = useState([]);
  const [cargandoHabilidades, setCargandoHabilidades] = useState(true);
  const [cargando, setCargando] = useState(false);
  const [error, setError] = useState('');
  const [errorRequisito, setErrorRequisito] = useState('');

  // Redirige si no hay empresa en sesion
  useEffect(() => {
    if (!empresaActual) {
      navegar('/empresa/registro');
      return;
    }
    cargarHabilidades();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [empresaActual]);

  async function cargarHabilidades() {
    try {
      const lista = await obtenerHabilidades();
      setHabilidades(lista);
    } catch {
      setError('No se pudo cargar el catalogo de habilidades.');
    } finally {
      setCargandoHabilidades(false);
    }
  }

  /** Actualiza los campos del formulario principal */
  function manejarCambio(evento) {
    const { name, value } = evento.target;
    setFormulario(anterior => ({ ...anterior, [name]: value }));
  }

  /** Actualiza los campos del nuevo requisito */
  function manejarCambioRequisito(evento) {
    const { name, value } = evento.target;
    setNuevoRequisito(anterior => ({ ...anterior, [name]: value }));
  }

  /** Agrega el requisito en construccion a la lista */
  function agregarRequisito() {
    setErrorRequisito('');

    if (!nuevoRequisito.habilidadId) {
      setErrorRequisito('Debe seleccionar una habilidad para el requisito.');
      return;
    }

    // Evita habilidades duplicadas
    const yaExiste = requisitos.some(r => String(r.habilidadId) === String(nuevoRequisito.habilidadId));
    if (yaExiste) {
      setErrorRequisito('Esta habilidad ya fue agregada como requisito.');
      return;
    }

    const habilidad = habilidades.find(h => String(h.id) === String(nuevoRequisito.habilidadId));

    setRequisitos(anteriores => [
      ...anteriores,
      {
        habilidadId: Number(nuevoRequisito.habilidadId),
        tipoRequisito: Number(nuevoRequisito.tipoRequisito),
        descripcion: nuevoRequisito.descripcion || null,
        nombreHabilidad: habilidad?.nombre || '',
      },
    ]);

    // Limpia el formulario de nuevo requisito
    setNuevoRequisito({ habilidadId: '', tipoRequisito: 0, descripcion: '' });
  }

  /** Elimina un requisito de la lista por posicion */
  function eliminarRequisito(indice) {
    setRequisitos(anteriores => anteriores.filter((_, i) => i !== indice));
  }

  /** Envia la oferta a la API */
  async function manejarEnvio(evento) {
    evento.preventDefault();
    setError('');
    setCargando(true);

    try {
      const datosOferta = {
        titulo: formulario.titulo,
        descripcion: formulario.descripcion,
        ubicacion: formulario.ubicacion,
        salario: formulario.salario ? Number(formulario.salario) : null,
        fechaCierre: formulario.fechaCierre || null,
        requisitos: requisitos.map(r => ({
          habilidadId: r.habilidadId,
          tipoRequisito: r.tipoRequisito,
          descripcion: r.descripcion,
        })),
      };

      await crearOferta(empresaActual.id, datosOferta);
      navegar('/empresa/ofertas');
    } catch (err) {
      const mensajeError =
        err.response?.data?.mensaje ||
        err.response?.data?.message ||
        err.response?.data ||
        'Ocurrio un error al crear la oferta. Verifique los datos e intente nuevamente.';
      setError(typeof mensajeError === 'string' ? mensajeError : JSON.stringify(mensajeError));
    } finally {
      setCargando(false);
    }
  }

  if (cargandoHabilidades) return <CargandoSpinner />;

  return (
    <div className="contenedor-pagina">
      <button className="boton-volver" onClick={() => navegar('/empresa/ofertas')}>
        ← Volver a mis ofertas
      </button>

      <h1>Nueva Oferta Laboral</h1>
      <p className="descripcion-seccion">
        Complete los datos del puesto y defina los requisitos de habilidades para los candidatos.
      </p>

      <MensajeError mensaje={error} />

      <form onSubmit={manejarEnvio} noValidate>
        <div className="campo">
          <label htmlFor="titulo">Titulo del Puesto</label>
          <input
            id="titulo"
            type="text"
            name="titulo"
            value={formulario.titulo}
            onChange={manejarCambio}
            required
            placeholder="Ej: Desarrollador Full Stack"
          />
        </div>

        <div className="campo">
          <label htmlFor="descripcion">Descripcion del Puesto</label>
          <textarea
            id="descripcion"
            name="descripcion"
            value={formulario.descripcion}
            onChange={manejarCambio}
            required
            rows={4}
            placeholder="Descripcion detallada de las tareas, responsabilidades y condiciones del puesto"
          />
        </div>

        <div className="grupo-campos">
          <div className="campo">
            <label htmlFor="ubicacion">Ubicacion</label>
            <input
              id="ubicacion"
              type="text"
              name="ubicacion"
              value={formulario.ubicacion}
              onChange={manejarCambio}
              required
              placeholder="Ciudad, Provincia o Remoto"
            />
          </div>

          <div className="campo">
            <label htmlFor="salario">Salario Mensual (opcional)</label>
            <input
              id="salario"
              type="number"
              name="salario"
              value={formulario.salario}
              onChange={manejarCambio}
              min={0}
              placeholder="Monto en pesos"
            />
          </div>
        </div>

        <div className="campo">
          <label htmlFor="fechaCierre">Fecha de Cierre (opcional)</label>
          <input
            id="fechaCierre"
            type="date"
            name="fechaCierre"
            value={formulario.fechaCierre}
            onChange={manejarCambio}
          />
        </div>

        {/* Seccion de requisitos */}
        <section className="seccion-requisitos">
          <h2>Requisitos de Habilidades</h2>
          <p className="descripcion-seccion">
            Defina las habilidades requeridas. Los requisitos <strong>Relevantes</strong> son
            obligatorios para postular; los <strong>No Relevantes</strong> son deseables y
            activan sugerencias de cursos para los candidatos que no los posean.
          </p>

          {errorRequisito && (
            <div className="mensaje-error" role="alert">
              <strong>Error:</strong> {errorRequisito}
            </div>
          )}

          {/* Formulario para agregar un requisito */}
          <div className="panel-agregar-requisito">
            <div className="grupo-campos">
              <div className="campo">
                <label htmlFor="habilidadId">Habilidad</label>
                <select
                  id="habilidadId"
                  name="habilidadId"
                  value={nuevoRequisito.habilidadId}
                  onChange={manejarCambioRequisito}
                >
                  <option value="">-- Seleccione una habilidad --</option>
                  {habilidades.map(h => (
                    <option key={h.id} value={h.id}>
                      {h.nombre}
                    </option>
                  ))}
                </select>
              </div>

              <div className="campo">
                <label htmlFor="tipoRequisito">Tipo de Requisito</label>
                <select
                  id="tipoRequisito"
                  name="tipoRequisito"
                  value={nuevoRequisito.tipoRequisito}
                  onChange={manejarCambioRequisito}
                >
                  {tiposRequisito.map(t => (
                    <option key={t.valor} value={t.valor}>
                      {t.etiqueta}
                    </option>
                  ))}
                </select>
              </div>
            </div>

            <div className="campo">
              <label htmlFor="descripcionRequisito">Descripcion del Requisito (opcional)</label>
              <input
                id="descripcionRequisito"
                type="text"
                name="descripcion"
                value={nuevoRequisito.descripcion}
                onChange={manejarCambioRequisito}
                placeholder="Aclaracion adicional sobre este requisito"
              />
            </div>

            <button
              type="button"
              className="boton-secundario"
              onClick={agregarRequisito}
            >
              + Agregar Requisito
            </button>
          </div>

          {/* Lista de requisitos agregados */}
          {requisitos.length > 0 && (
            <div className="lista-requisitos-oferta">
              <h3>Requisitos agregados ({requisitos.length})</h3>
              <ul className="lista-requisitos">
                {requisitos.map((req, indice) => (
                  <li key={indice} className="item-requisito-empresa">
                    <div className="info-requisito">
                      <strong>{req.nombreHabilidad}</strong>
                      <span className={`etiqueta-tipo-requisito tipo-${req.tipoRequisito}`}>
                        {tiposRequisito.find(t => t.valor === req.tipoRequisito)?.etiqueta}
                      </span>
                      {req.descripcion && (
                        <span className="descripcion-requisito-item">{req.descripcion}</span>
                      )}
                    </div>
                    <button
                      type="button"
                      className="boton-eliminar-requisito"
                      onClick={() => eliminarRequisito(indice)}
                      title="Eliminar requisito"
                    >
                      ✕
                    </button>
                  </li>
                ))}
              </ul>
            </div>
          )}
        </section>

        <div className="acciones-formulario">
          <button type="submit" className="boton-primario" disabled={cargando}>
            {cargando ? 'Publicando...' : 'Publicar Oferta'}
          </button>
          <button
            type="button"
            className="boton-secundario"
            onClick={() => navegar('/empresa/ofertas')}
          >
            Cancelar
          </button>
        </div>
      </form>
    </div>
  );
}
