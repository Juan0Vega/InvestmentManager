import MainLayout from './layouts/MainLayout'
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Clients from "./pages/Clients";
import Funds from "./pages/Funds";
function App() {
 
  return (
    <Router>
      <Routes>
        <Route element={<MainLayout />}>
          <Route path="/" element={<h1 className="text-2xl font-bold">Inicio</h1>} />
          <Route path="/Clients" element={<Clients />} />
          <Route path="/Funds" element={<Funds />} />
        </Route>
      </Routes>
    </Router>
  )
}

export default App
