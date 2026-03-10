// Componente raiz de la aplicacion. Define el enrutamiento principal del flujo del joven y de empresa.

import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { ProveedorContextoJoven } from './contexto/ContextoJoven';
import { ProveedorContextoEmpresa } from './contexto/ContextoEmpresa';
import Navegacion from './componentes/comunes/Navegacion';
import Registro from './componentes/Registro';
import CreacionCurriculum from './componentes/CreacionCurriculum';
import ListaOfertas from './componentes/ListaOfertas';
import DetalleOferta from './componentes/DetalleOferta';
import ListaCursos from './componentes/ListaCursos';
import ExamenCurso from './componentes/ExamenCurso';
import MisPostulaciones from './componentes/MisPostulaciones';
import RegistroEmpresa from './componentes/empresa/RegistroEmpresa';
import GestionOfertas from './componentes/empresa/GestionOfertas';
import CreacionOferta from './componentes/empresa/CreacionOferta';
import CandidatosFiltrados from './componentes/empresa/CandidatosFiltrados';
import FeedbackPostulante from './componentes/empresa/FeedbackPostulante';
import './App.css';

export default function App() {
  return (
    <ProveedorContextoJoven>
      <ProveedorContextoEmpresa>
        <BrowserRouter>
          <Navegacion />
          <main className="contenido-principal">
            <Routes>
              {/* Ruta inicial redirige al registro */}
              <Route path="/" element={<Navigate to="/registro" replace />} />

              {/* Flujo del joven */}
              <Route path="/registro" element={<Registro />} />
              <Route path="/curriculum" element={<CreacionCurriculum />} />
              <Route path="/ofertas" element={<ListaOfertas />} />
              <Route path="/ofertas/:ofertaId" element={<DetalleOferta />} />
              <Route path="/mis-postulaciones" element={<MisPostulaciones />} />
              <Route path="/cursos" element={<ListaCursos />} />
              <Route path="/cursos/:cursoId/examen" element={<ExamenCurso />} />

              {/* Flujo de empresa */}
              <Route path="/empresa/registro" element={<RegistroEmpresa />} />
              <Route path="/empresa/ofertas" element={<GestionOfertas />} />
              <Route path="/empresa/ofertas/nueva" element={<CreacionOferta />} />
              <Route path="/empresa/ofertas/:ofertaId/candidatos" element={<CandidatosFiltrados />} />
              <Route path="/empresa/ofertas/:ofertaId/candidatos/:postulacionId/feedback" element={<FeedbackPostulante />} />

              {/* Ruta no encontrada */}
              <Route path="*" element={<Navigate to="/registro" replace />} />
            </Routes>
          </main>
        </BrowserRouter>
      </ProveedorContextoEmpresa>
    </ProveedorContextoJoven>
  );
}
