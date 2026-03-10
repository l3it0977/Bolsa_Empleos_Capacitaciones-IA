// Indicador visual de carga. Se muestra mientras se espera una respuesta de la API.

export default function CargandoSpinner() {
  return (
    <div className="cargando-contenedor" role="status" aria-live="polite">
      <div className="cargando-spinner"></div>
      <p>Cargando...</p>
    </div>
  );
}
