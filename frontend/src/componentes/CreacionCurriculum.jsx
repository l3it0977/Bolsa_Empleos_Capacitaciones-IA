// Componente para crear y editar el curriculum del joven.
// Permite ingresar datos profesionales y agregar habilidades al CV.

import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  obtenerCurriculum,
  guardarCurriculum,
  obtenerHabilidades,
  agregarHabilidadCurriculum,
} from '../api/clienteApi';
import { useContextoJoven } from '../contexto/ContextoJoven';
import CargandoSpinner from './comunes/CargandoSpinner';
import MensajeError from './comunes/MensajeError';

export default function CreacionCurriculum() {
  const { jovenActual } = useContextoJoven();
  const navegar = useNavigate();

  // Datos del formulario de curriculum
  const [formulario, setFormulario] = useState({
    tituloProfesional: '',
    resumenProfesional: '',
    urlPortfolio: '',
  });

  // Lista de todas las habilidades disponibles en el sistema
  const [habilidades, setHabilidades] = useState([]);

  // Habilidades ya presentes en el curriculum del joven
  const [habilidadesEnCV, setHabilidadesEnCV] = useState([]);

  const [cargando, setCargando] = useState(true);
  const [guardando, setGuardando] = useState(false);
  const [error, setError] = useState('');
  const [mensajeExito, setMensajeExito] = useState('');

  // Carga inicial: curriculum actual y habilidades del catalogo
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
      const [datosCV, listaHabilidades] = await Promise.all([
        obtenerCurriculum(jovenActual.id).catch(() => null),
        obtenerHabilidades(),
      ]);

      if (datosCV) {
        setFormulario({
          tituloProfesional: datosCV.tituloProfesional || '',
          resumenProfesional: datosCV.resumenProfesional || '',
          urlPortfolio: datosCV.urlPortfolio || '',
        });
        setHabilidadesEnCV(datosCV.habilidades || []);
      }
      setHabilidades(listaHabilidades);
    } catch {
      setError('No se pudo cargar la informacion del curriculum.');
    } finally {
      setCargando(false);
    }
  }

  /** Actualiza el campo correspondiente del formulario */
  function manejarCambio(evento) {
    const { name, value } = evento.target;
    setFormulario(anterior => ({ ...anterior, [name]: value }));
  }

  /** Guarda los datos principales del curriculum */
  async function manejarGuardarCV(evento) {
    evento.preventDefault();
    setError('');
    setMensajeExito('');
    setGuardando(true);
    try {
      await guardarCurriculum(jovenActual.id, formulario);
      setMensajeExito('Curriculum guardado correctamente.');
    } catch {
      setError('No se pudo guardar el curriculum. Intente nuevamente.');
    } finally {
      setGuardando(false);
    }
  }

  /** Agrega una habilidad al curriculum del joven */
  async function manejarAgregarHabilidad(habilidadId) {
    setError('');
    setMensajeExito('');
    try {
      await agregarHabilidadCurriculum(jovenActual.id, habilidadId);
      // Recarga el curriculum para reflejar la habilidad agregada
      await cargarDatos();
      setMensajeExito('Habilidad agregada al curriculum.');
    } catch {
      setError('No se pudo agregar la habilidad. Es posible que ya este en su curriculum.');
    }
  }

  // Ids de las habilidades ya incluidas en el CV para comparacion
  const idsHabilidadesEnCV = habilidadesEnCV.map(h => h.habilidadId || h.id);

  if (cargando) return <CargandoSpinner />;

  return (
    <div className="contenedor-formulario">
      <h1>Mi Curriculum</h1>
      <p className="descripcion-seccion">
        Complete su informacion profesional y agregue las habilidades que posee.
      </p>

      <MensajeError mensaje={error} />

      {mensajeExito && (
        <div className="mensaje-exito" role="status">
          {mensajeExito}
        </div>
      )}

      {/* Formulario de datos del curriculum */}
      <form onSubmit={manejarGuardarCV}>
        <div className="campo">
          <label htmlFor="tituloProfesional">Titulo Profesional</label>
          <input
            id="tituloProfesional"
            type="text"
            name="tituloProfesional"
            value={formulario.tituloProfesional}
            onChange={manejarCambio}
            placeholder="Ejemplo: Desarrollador Web Junior"
          />
        </div>

        <div className="campo">
          <label htmlFor="resumenProfesional">Resumen Profesional</label>
          <textarea
            id="resumenProfesional"
            name="resumenProfesional"
            value={formulario.resumenProfesional}
            onChange={manejarCambio}
            rows={5}
            placeholder="Describa brevemente su perfil y experiencia"
          />
        </div>

        <div className="campo">
          <label htmlFor="urlPortfolio">URL de Portfolio (opcional)</label>
          <input
            id="urlPortfolio"
            type="url"
            name="urlPortfolio"
            value={formulario.urlPortfolio}
            onChange={manejarCambio}
            placeholder="https://miportfolio.com"
          />
        </div>

        <button type="submit" className="boton-primario" disabled={guardando}>
          {guardando ? 'Guardando...' : 'Guardar Curriculum'}
        </button>
      </form>

      {/* Seccion de habilidades */}
      <section className="seccion-habilidades">
        <h2>Habilidades en mi Curriculum</h2>

        {habilidadesEnCV.length === 0 ? (
          <p>Aun no tiene habilidades en su curriculum.</p>
        ) : (
          <ul className="lista-etiquetas">
            {habilidadesEnCV.map(habilidad => (
              <li key={habilidad.habilidadId || habilidad.id} className="etiqueta-habilidad">
                {habilidad.nombreHabilidad || habilidad.nombre}
                {habilidad.obtenidaPorCurso && (
                  <span className="insignia-curso"> (por curso)</span>
                )}
              </li>
            ))}
          </ul>
        )}

        <h2>Agregar Habilidades del Catalogo</h2>
        <p>Seleccione las habilidades que ya posee para agregarlas a su curriculum.</p>

        <div className="grilla-habilidades">
          {habilidades
            .filter(h => !idsHabilidadesEnCV.includes(h.id))
            .map(habilidad => (
              <div key={habilidad.id} className="tarjeta-habilidad">
                <span className="nombre-habilidad">{habilidad.nombre}</span>
                {habilidad.categoria && (
                  <span className="categoria-habilidad">{habilidad.categoria}</span>
                )}
                <button
                  className="boton-secundario"
                  onClick={() => manejarAgregarHabilidad(habilidad.id)}
                >
                  Agregar
                </button>
              </div>
            ))}
        </div>
      </section>

      <div className="acciones-navegacion">
        <button className="boton-primario" onClick={() => navegar('/ofertas')}>
          Ver Ofertas de Trabajo
        </button>
      </div>
    </div>
  );
}
