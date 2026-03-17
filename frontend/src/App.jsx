// Componente raiz de la aplicacion. Define el enrutamiento principal del flujo del joven y de empresa.

import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { ProveedorContextoJoven } from './contexto/ContextoJoven';
import { ProveedorContextoEmpresa } from './contexto/ContextoEmpresa';
import Navegacion from './componentes/comunes/Navegacion';
import RutaProtegida from './componentes/comunes/RutaProtegida';
import Login from './componentes/Login';
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
              {/* Ruta inicial redirige al inicio de sesion */}
              <Route path="/" element={<Navigate to="/login" replace />} />
              <Route path="/login" element={<Login />} />

              {/* Flujo del joven */}
              <Route path="/registro" element={<Registro />} />
              <Route path="/curriculum" element={<RutaProtegida rolPermitido="Joven"><CreacionCurriculum /></RutaProtegida>} />
              <Route path="/ofertas" element={<RutaProtegida rolPermitido="Joven"><ListaOfertas /></RutaProtegida>} />
              <Route path="/ofertas/:ofertaId" element={<RutaProtegida rolPermitido="Joven"><DetalleOferta /></RutaProtegida>} />
              <Route path="/mis-postulaciones" element={<RutaProtegida rolPermitido="Joven"><MisPostulaciones /></RutaProtegida>} />
              <Route path="/cursos" element={<RutaProtegida rolPermitido="Joven"><ListaCursos /></RutaProtegida>} />
              <Route path="/cursos/:cursoId/examen" element={<RutaProtegida rolPermitido="Joven"><ExamenCurso /></RutaProtegida>} />

              {/* Flujo de empresa */}
              <Route path="/empresa/registro" element={<RegistroEmpresa />} />
              <Route path="/empresa/ofertas" element={<RutaProtegida rolPermitido="Empresa"><GestionOfertas /></RutaProtegida>} />
              <Route path="/empresa/ofertas/nueva" element={<RutaProtegida rolPermitido="Empresa"><CreacionOferta /></RutaProtegida>} />
              <Route path="/empresa/ofertas/:ofertaId/candidatos" element={<RutaProtegida rolPermitido="Empresa"><CandidatosFiltrados /></RutaProtegida>} />
              <Route path="/empresa/ofertas/:ofertaId/candidatos/:postulacionId/feedback" element={<RutaProtegida rolPermitido="Empresa"><FeedbackPostulante /></RutaProtegida>} />

              {/* Ruta no encontrada */}
              <Route path="*" element={<Navigate to="/login" replace />} />
            </Routes>
          </main>
        </BrowserRouter>
      </ProveedorContextoEmpresa>
    </ProveedorContextoJoven>
  );
}
