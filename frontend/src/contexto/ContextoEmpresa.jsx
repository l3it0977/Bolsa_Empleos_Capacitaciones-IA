// Contexto global para el flujo de empresa.
// Almacena los datos de la empresa autenticada y expone funciones de sesion basica.

import { createContext, useContext, useState } from 'react';

const ContextoEmpresa = createContext(null);

/**
 * Proveedor del contexto de empresa.
 * Envuelve las rutas del modulo empresa para compartir el estado de sesion.
 */
export function ProveedorContextoEmpresa({ children }) {
  // Estado con los datos basicos de la empresa en sesion
  const [empresaActual, setEmpresaActual] = useState(() => {
    const guardado = localStorage.getItem('empresaActual');
    return guardado ? JSON.parse(guardado) : null;
  });

  /** Guarda la empresa en el estado y en localStorage al registrarse o iniciar sesion */
  function iniciarSesionEmpresa(datosEmpresa) {
    setEmpresaActual(datosEmpresa);
    localStorage.setItem('empresaActual', JSON.stringify(datosEmpresa));
  }

  /** Limpia la sesion de empresa */
  function cerrarSesionEmpresa() {
    setEmpresaActual(null);
    localStorage.removeItem('empresaActual');
  }

  return (
    <ContextoEmpresa.Provider value={{ empresaActual, iniciarSesionEmpresa, cerrarSesionEmpresa }}>
      {children}
    </ContextoEmpresa.Provider>
  );
}

/** Hook para consumir el contexto de empresa en cualquier componente */
// eslint-disable-next-line react-refresh/only-export-components
export function useContextoEmpresa() {
  return useContext(ContextoEmpresa);
}
