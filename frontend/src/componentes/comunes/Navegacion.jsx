// Componente de navegacion principal.
// Muestra los enlaces segun si hay sesion activa de joven, empresa o ninguna.

import { Link, useNavigate } from 'react-router-dom';
import { useContextoJoven } from '../../contexto/ContextoJoven';
import { useContextoEmpresa } from '../../contexto/ContextoEmpresa';

export default function Navegacion() {
  const { jovenActual, cerrarSesion } = useContextoJoven();
  const { empresaActual, cerrarSesionEmpresa } = useContextoEmpresa();
  const navegar = useNavigate();

  function manejarCerrarSesionJoven() {
    cerrarSesion();
    navegar('/registro');
  }

  function manejarCerrarSesionEmpresa() {
    cerrarSesionEmpresa();
    navegar('/empresa/registro');
  }

  return (
    <nav className="navegacion">
      <div className="navegacion-marca">
        <Link to="/">Bolsa de Empleos</Link>
      </div>

      {/* Navegacion para joven autenticado */}
      {jovenActual && (
        <ul className="navegacion-enlaces">
          <li><Link to="/ofertas">Ofertas de Trabajo</Link></li>
          <li><Link to="/cursos">Cursos</Link></li>
          <li><Link to="/curriculum">Mi Curriculum</Link></li>
          <li><Link to="/mis-postulaciones">Mis Postulaciones</Link></li>
          <li>
            <button className="boton-cerrar-sesion" onClick={manejarCerrarSesionJoven}>
              Cerrar Sesion
            </button>
          </li>
        </ul>
      )}

      {/* Navegacion para empresa autenticada */}
      {empresaActual && !jovenActual && (
        <ul className="navegacion-enlaces">
          <li><Link to="/empresa/ofertas">Mis Ofertas</Link></li>
          <li><Link to="/empresa/ofertas/nueva">Nueva Oferta</Link></li>
          <li className="nombre-empresa-nav">{empresaActual.razonSocial}</li>
          <li>
            <button className="boton-cerrar-sesion" onClick={manejarCerrarSesionEmpresa}>
              Cerrar Sesion
            </button>
          </li>
        </ul>
      )}

      {/* Navegacion sin sesion activa */}
      {!jovenActual && !empresaActual && (
        <ul className="navegacion-enlaces">
          <li><Link to="/registro">Soy Candidato</Link></li>
          <li><Link to="/empresa/registro">Soy Empresa</Link></li>
        </ul>
      )}
    </nav>
  );
}
