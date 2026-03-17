import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { loginEmpresa, loginJoven } from '../api/clienteApi';
import { useContextoEmpresa } from '../contexto/ContextoEmpresa';
import { useContextoJoven } from '../contexto/ContextoJoven';
import MensajeError from './comunes/MensajeError';

export default function Login() {
  const navegar = useNavigate();
  const { iniciarSesion } = useContextoJoven();
  const { iniciarSesionEmpresa } = useContextoEmpresa();
  const [tipoCuenta, setTipoCuenta] = useState('Joven');
  const [correoElectronico, setCorreoElectronico] = useState('');
  const [contrasena, setContrasena] = useState('');
  const [error, setError] = useState('');
  const [cargando, setCargando] = useState(false);

  async function manejarEnvio(evento) {
    evento.preventDefault();
    setError('');
    setCargando(true);

    try {
      if (tipoCuenta === 'Empresa') {
        const respuesta = await loginEmpresa(correoElectronico, contrasena);
        iniciarSesionEmpresa(
          {
            id: respuesta.usuario.id,
            razonSocial: respuesta.usuario.nombre,
            correoElectronico: respuesta.usuario.correoElectronico,
          },
          respuesta.token
        );
        navegar('/empresa/ofertas');
      } else {
        const respuesta = await loginJoven(correoElectronico, contrasena);
        iniciarSesion(
          {
            id: respuesta.usuario.id,
            nombre: respuesta.usuario.nombre,
            apellido: respuesta.usuario.nombreSecundario ?? '',
            correoElectronico: respuesta.usuario.correoElectronico,
          },
          respuesta.token
        );
        navegar('/ofertas');
      }
    } catch (err) {
      setError(err.response?.data?.message || 'Credenciales inválidas.');
    } finally {
      setCargando(false);
    }
  }

  return (
    <div className="contenedor-formulario">
      <h1>Iniciar Sesion</h1>
      <p className="descripcion-seccion">Ingrese con su cuenta de joven o de empresa.</p>
      <MensajeError mensaje={error} />

      <form onSubmit={manejarEnvio}>
        <div className="campo">
          <label htmlFor="tipoCuenta">Tipo de cuenta</label>
          <select id="tipoCuenta" value={tipoCuenta} onChange={evento => setTipoCuenta(evento.target.value)}>
            <option value="Joven">Joven</option>
            <option value="Empresa">Empresa</option>
          </select>
        </div>

        <div className="campo">
          <label htmlFor="correoElectronico">Correo Electronico</label>
          <input
            id="correoElectronico"
            type="email"
            value={correoElectronico}
            onChange={evento => setCorreoElectronico(evento.target.value)}
            required
          />
        </div>

        <div className="campo">
          <label htmlFor="contrasena">Contrasena</label>
          <input
            id="contrasena"
            type="password"
            value={contrasena}
            onChange={evento => setContrasena(evento.target.value)}
            minLength={8}
            required
          />
        </div>

        <button type="submit" className="boton-primario" disabled={cargando}>
          {cargando ? 'Ingresando...' : 'Iniciar Sesion'}
        </button>
      </form>

      <p className="enlace-secundario">
        ¿No tiene cuenta?{' '}
        <Link to={tipoCuenta === 'Empresa' ? '/empresa/registro' : '/registro'}>
          Crear cuenta {tipoCuenta === 'Empresa' ? 'de empresa' : 'de joven'}
        </Link>
      </p>
    </div>
  );
}
