// Componente de navegacion principal del flujo del joven.
// Muestra los enlaces segun si hay sesion activa o no.

import { Link, useNavigate } from 'react-router-dom';
import { useContextoJoven } from '../../contexto/ContextoJoven';

export default function Navegacion() {
  const { jovenActual, cerrarSesion } = useContextoJoven();
  const navegar = useNavigate();

  function manejarCerrarSesion() {
    cerrarSesion();
    navegar('/registro');
  }

  return (
    <nav className="navegacion">
      <div className="navegacion-marca">
        <Link to="/">Bolsa de Empleos</Link>
      </div>

      {jovenActual ? (
        <ul className="navegacion-enlaces">
          <li><Link to="/ofertas">Ofertas de Trabajo</Link></li>
          <li><Link to="/cursos">Cursos</Link></li>
          <li><Link to="/curriculum">Mi Curriculum</Link></li>
          <li><Link to="/mis-postulaciones">Mis Postulaciones</Link></li>
          <li>
            <button className="boton-cerrar-sesion" onClick={manejarCerrarSesion}>
              Cerrar Sesion
            </button>
          </li>
        </ul>
      ) : (
        <ul className="navegacion-enlaces">
          <li><Link to="/registro">Registrarse</Link></li>
        </ul>
      )}
    </nav>
  );
}
