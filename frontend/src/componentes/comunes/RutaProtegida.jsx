import { Navigate } from 'react-router-dom';
import { useContextoJoven } from '../../contexto/ContextoJoven';
import { useContextoEmpresa } from '../../contexto/ContextoEmpresa';

export default function RutaProtegida({ children, rolPermitido }) {
  const { jovenActual } = useContextoJoven();
  const { empresaActual } = useContextoEmpresa();
  const token = localStorage.getItem('tokenSesion');
  const rolSesion = localStorage.getItem('rolSesion');

  const haySesion = Boolean(token && (jovenActual || empresaActual));
  if (!haySesion) {
    return <Navigate to="/login" replace />;
  }

  if (rolPermitido && rolSesion !== rolPermitido) {
    return <Navigate to={rolSesion === 'Empresa' ? '/empresa/ofertas' : '/ofertas'} replace />;
  }

  return children;
}
