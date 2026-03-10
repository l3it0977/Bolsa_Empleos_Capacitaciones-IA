// Componente de registro de empresa.
// Permite crear una cuenta de empresa y redirige al panel de gestion.

import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { registrarEmpresa } from '../../api/clienteApi';
import { useContextoEmpresa } from '../../contexto/ContextoEmpresa';
import MensajeError from '../comunes/MensajeError';

export default function RegistroEmpresa() {
  const navegar = useNavigate();
  const { iniciarSesionEmpresa } = useContextoEmpresa();

  // Estado del formulario de registro de empresa
  const [formulario, setFormulario] = useState({
    razonSocial: '',
    numeroIdentificacion: '',
    correoElectronico: '',
    contrasena: '',
    telefono: '',
    descripcion: '',
    sitioWeb: '',
  });

  const [cargando, setCargando] = useState(false);
  const [error, setError] = useState('');

  /** Actualiza el campo correspondiente en el estado del formulario */
  function manejarCambio(evento) {
    const { name, value } = evento.target;
    setFormulario(anterior => ({ ...anterior, [name]: value }));
  }

  /** Envia el formulario a la API y guarda la sesion de empresa */
  async function manejarEnvio(evento) {
    evento.preventDefault();
    setError('');
    setCargando(true);

    try {
      const empresaRegistrada = await registrarEmpresa({
        razonSocial: formulario.razonSocial,
        numeroIdentificacion: formulario.numeroIdentificacion,
        correoElectronico: formulario.correoElectronico,
        contrasena: formulario.contrasena,
        telefono: formulario.telefono || null,
        descripcion: formulario.descripcion || null,
        sitioWeb: formulario.sitioWeb || null,
      });

      // Guarda la empresa en el contexto y redirige al panel
      iniciarSesionEmpresa(empresaRegistrada);
      navegar('/empresa/ofertas');
    } catch (err) {
      const mensajeError =
        err.response?.data?.mensaje ||
        err.response?.data?.message ||
        err.response?.data ||
        'Ocurrio un error al registrar la empresa. Verifique los datos e intente nuevamente.';
      setError(typeof mensajeError === 'string' ? mensajeError : JSON.stringify(mensajeError));
    } finally {
      setCargando(false);
    }
  }

  return (
    <div className="contenedor-formulario">
      <h1>Registrar Empresa</h1>
      <p className="descripcion-seccion">
        Complete los datos para crear la cuenta de su empresa en la Bolsa de Empleos.
      </p>

      <MensajeError mensaje={error} />

      <form onSubmit={manejarEnvio} noValidate>
        <div className="campo">
          <label htmlFor="razonSocial">Razon Social</label>
          <input
            id="razonSocial"
            type="text"
            name="razonSocial"
            value={formulario.razonSocial}
            onChange={manejarCambio}
            required
            placeholder="Nombre legal de la empresa"
          />
        </div>

        <div className="campo">
          <label htmlFor="numeroIdentificacion">Numero de Identificacion (CUIT / RUT)</label>
          <input
            id="numeroIdentificacion"
            type="text"
            name="numeroIdentificacion"
            value={formulario.numeroIdentificacion}
            onChange={manejarCambio}
            required
            placeholder="Ej: 30-12345678-9"
          />
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
            placeholder="contacto@empresa.com"
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
            placeholder="Minimo 8 caracteres"
            minLength={8}
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
          <label htmlFor="descripcion">Descripcion de la Empresa (opcional)</label>
          <textarea
            id="descripcion"
            name="descripcion"
            value={formulario.descripcion}
            onChange={manejarCambio}
            rows={3}
            placeholder="Breve descripcion de la actividad y cultura de la empresa"
          />
        </div>

        <div className="campo">
          <label htmlFor="sitioWeb">Sitio Web (opcional)</label>
          <input
            id="sitioWeb"
            type="url"
            name="sitioWeb"
            value={formulario.sitioWeb}
            onChange={manejarCambio}
            placeholder="https://www.empresa.com"
          />
        </div>

        <div className="acciones-formulario">
          <button type="submit" className="boton-primario" disabled={cargando}>
            {cargando ? 'Registrando...' : 'Crear Cuenta de Empresa'}
          </button>
        </div>
      </form>

      <p className="enlace-secundario">
        ¿Ya tiene cuenta?{' '}
        <Link to="/empresa/ofertas">Ir al panel de empresa</Link>
      </p>
    </div>
  );
}
