// Componente para listar las ofertas de trabajo publicadas.
// Muestra las ofertas disponibles con sus datos principales.

import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { obtenerOfertas } from '../api/clienteApi';
import { useContextoJoven } from '../contexto/ContextoJoven';
import CargandoSpinner from './comunes/CargandoSpinner';
import MensajeError from './comunes/MensajeError';

export default function ListaOfertas() {
  const { jovenActual } = useContextoJoven();
  const navegar = useNavigate();

  const [ofertas, setOfertas] = useState([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!jovenActual) {
      navegar('/registro');
      return;
    }
    cargarOfertas();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [jovenActual]);

  async function cargarOfertas() {
    setCargando(true);
    setError('');
    try {
      const listaOfertas = await obtenerOfertas();
      setOfertas(listaOfertas);
    } catch {
      setError('No se pudieron cargar las ofertas de trabajo.');
    } finally {
      setCargando(false);
    }
  }

  /** Navega al detalle de la oferta seleccionada */
  function verDetalle(ofertaId) {
    navegar(`/ofertas/${ofertaId}`);
  }

  if (cargando) return <CargandoSpinner />;

  return (
    <div className="contenedor-pagina">
      <h1>Ofertas de Trabajo</h1>
      <p className="descripcion-seccion">
        Explore las oportunidades laborales disponibles y postulese a las que se ajusten a su perfil.
      </p>

      <MensajeError mensaje={error} />

      {ofertas.length === 0 ? (
        <div className="sin-resultados">
          <p>No hay ofertas de trabajo disponibles en este momento.</p>
        </div>
      ) : (
        <div className="grilla-tarjetas">
          {ofertas.map(oferta => (
            <article key={oferta.id} className="tarjeta-oferta">
              <h2 className="titulo-oferta">{oferta.titulo}</h2>
              <p className="empresa-oferta">{oferta.empresaNombre || oferta.empresa}</p>
              <p className="ubicacion-oferta">{oferta.ubicacion}</p>

              {oferta.salario && (
                <p className="salario-oferta">
                  Salario: ${Number(oferta.salario).toLocaleString('es-AR')}
                </p>
              )}

              {oferta.fechaCierre && (
                <p className="fecha-cierre-oferta">
                  Cierre: {new Date(oferta.fechaCierre).toLocaleDateString('es-AR')}
                </p>
              )}

              <p className="descripcion-oferta">{oferta.descripcion}</p>

              <div className="acciones-tarjeta">
                <button
                  className="boton-primario"
                  onClick={() => verDetalle(oferta.id)}
                >
                  Ver Detalle y Postularme
                </button>
              </div>
            </article>
          ))}
        </div>
      )}
    </div>
  );
}
