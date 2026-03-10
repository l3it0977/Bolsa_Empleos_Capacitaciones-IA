// Componente de registro del joven.
// Permite crear una cuenta nueva y redirige al flujo principal.

import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { registrarJoven } from '../api/clienteApi';
import { useContextoJoven } from '../contexto/ContextoJoven';
import MensajeError from './comunes/MensajeError';

// Niveles educativos disponibles segun el dominio del backend
const nivelesEducativos = [
  { valor: 0, etiqueta: 'Primaria' },
  { valor: 1, etiqueta: 'Secundaria' },
  { valor: 2, etiqueta: 'Tecnico' },
  { valor: 3, etiqueta: 'Universitario' },
  { valor: 4, etiqueta: 'Posgrado' },
];

export default function Registro() {
  const navegar = useNavigate();
  const { iniciarSesion } = useContextoJoven();

  // Estado del formulario
  const [formulario, setFormulario] = useState({
    nombre: '',
    apellido: '',
    correoElectronico: '',
    contrasena: '',
    telefono: '',
    fechaNacimiento: '',
    nivelEducativo: 1,
  });

  const [cargando, setCargando] = useState(false);
  const [error, setError] = useState('');

  /** Actualiza el campo correspondiente en el estado del formulario */
  function manejarCambio(evento) {
    const { name, value } = evento.target;
    setFormulario(anterior => ({ ...anterior, [name]: value }));
  }

  /** Envia el formulario a la API y guarda la sesion del joven */
  async function manejarEnvio(evento) {
    evento.preventDefault();
    setError('');
    setCargando(true);

    try {
      const jovenRegistrado = await registrarJoven({
        nombre: formulario.nombre,
        apellido: formulario.apellido,
        correoElectronico: formulario.correoElectronico,
        contrasena: formulario.contrasena,
        telefono: formulario.telefono || null,
        fechaNacimiento: formulario.fechaNacimiento,
        nivelEducativo: Number(formulario.nivelEducativo),
      });

      // Guarda el joven en el contexto y redirige
      iniciarSesion(jovenRegistrado);
      navegar('/curriculum');
    } catch (err) {
      const mensajeError =
        err.response?.data?.message ||
        err.response?.data ||
        'Ocurrio un error al registrarse. Verifique los datos e intente nuevamente.';
      setError(typeof mensajeError === 'string' ? mensajeError : JSON.stringify(mensajeError));
    } finally {
      setCargando(false);
    }
  }

  return (
    <div className="contenedor-formulario">
      <h1>Crear Cuenta</h1>
      <p className="descripcion-seccion">
        Complete los datos para registrarse en la Bolsa de Empleos.
      </p>

      <MensajeError mensaje={error} />

      <form onSubmit={manejarEnvio} noValidate>
        <div className="grupo-campos">
          <div className="campo">
            <label htmlFor="nombre">Nombre</label>
            <input
              id="nombre"
              type="text"
              name="nombre"
              value={formulario.nombre}
              onChange={manejarCambio}
              required
              placeholder="Ingrese su nombre"
            />
          </div>

          <div className="campo">
            <label htmlFor="apellido">Apellido</label>
            <input
              id="apellido"
              type="text"
              name="apellido"
              value={formulario.apellido}
              onChange={manejarCambio}
              required
              placeholder="Ingrese su apellido"
            />
          </div>
        </div>

        <div className="campo">
          <label htmlFor="correoElectronico">Correo Electronico</label>
          <input
            id="correoElectronico"
            type="email"
            name="correoElectronico"
            value={formulario.correoElectronico}
            onChange={manejarCambio}
            required
            placeholder="ejemplo@correo.com"
          />
        </div>

        <div className="campo">
          <label htmlFor="contrasena">Contrasena</label>
          <input
            id="contrasena"
            type="password"
            name="contrasena"
            value={formulario.contrasena}
            onChange={manejarCambio}
            required
            placeholder="Minimo 6 caracteres"
            minLength={6}
          />
        </div>

        <div className="campo">
          <label htmlFor="telefono">Telefono (opcional)</label>
          <input
            id="telefono"
            type="tel"
            name="telefono"
            value={formulario.telefono}
            onChange={manejarCambio}
            placeholder="+54 11 1234-5678"
          />
        </div>

        <div className="campo">
          <label htmlFor="fechaNacimiento">Fecha de Nacimiento</label>
          <input
            id="fechaNacimiento"
            type="date"
            name="fechaNacimiento"
            value={formulario.fechaNacimiento}
            onChange={manejarCambio}
            required
          />
        </div>

        <div className="campo">
          <label htmlFor="nivelEducativo">Nivel Educativo</label>
          <select
            id="nivelEducativo"
            name="nivelEducativo"
            value={formulario.nivelEducativo}
            onChange={manejarCambio}
          >
            {nivelesEducativos.map(nivel => (
              <option key={nivel.valor} value={nivel.valor}>
                {nivel.etiqueta}
              </option>
            ))}
          </select>
        </div>

        <button type="submit" className="boton-primario" disabled={cargando}>
          {cargando ? 'Registrando...' : 'Crear Cuenta'}
        </button>
      </form>
    </div>
  );
}
