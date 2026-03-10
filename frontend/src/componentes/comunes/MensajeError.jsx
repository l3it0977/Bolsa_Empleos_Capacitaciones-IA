// Muestra un mensaje de error con estilos destacados.

export default function MensajeError({ mensaje }) {
  if (!mensaje) return null;
  return (
    <div className="mensaje-error" role="alert">
      <strong>Error:</strong> {mensaje}
    </div>
  );
}
