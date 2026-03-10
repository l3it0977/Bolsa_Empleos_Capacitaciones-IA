// Contexto global para el flujo del joven.
// Almacena el id del joven autenticado y expone funciones de autenticacion basica.

import { createContext, useContext, useState } from 'react';

const ContextoJoven = createContext(null);

/**
 * Proveedor del contexto del joven.
 * Envuelve toda la aplicacion para compartir el estado de sesion.
 */
export function ProveedorContextoJoven({ children }) {
  // Estado con los datos basicos del joven en sesion
  const [jovenActual, setJovenActual] = useState(() => {
    const guardado = localStorage.getItem('jovenActual');
    return guardado ? JSON.parse(guardado) : null;
  });

  /** Guarda el joven en el estado y en localStorage al registrarse o iniciar sesion */
  function iniciarSesion(datosJoven) {
    setJovenActual(datosJoven);
    localStorage.setItem('jovenActual', JSON.stringify(datosJoven));
  }

  /** Limpia la sesion del joven */
  function cerrarSesion() {
    setJovenActual(null);
    localStorage.removeItem('jovenActual');
  }

  return (
    <ContextoJoven.Provider value={{ jovenActual, iniciarSesion, cerrarSesion }}>
      {children}
    </ContextoJoven.Provider>
  );
}

/** Hook para consumir el contexto del joven en cualquier componente */
// eslint-disable-next-line react-refresh/only-export-components
export function useContextoJoven() {
  return useContext(ContextoJoven);
}
