import './App.css'
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-icons/font/bootstrap-icons.min.css";
import Layout from './Layout';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import EmployeeComponent from './Components/EmployeeComponent/EmployeeComponent';
import VehicleComponent from './Components/VehicleComponent/VehicleComponent';
import MachineryComponent from './Components/MachineryComponent/MachineryComponent';
import ToolComponent from './Components/ToolComponent/ToolComponent';

function App() {

  return (
    <BrowserRouter>
      <Layout>
        <Routes>
          {/* 'container mt-4' skal inkluderes i komponenter/pages, som vi gerne vil give en 'fixed width' og centrere på skærmen
              Jeg har ikke gjort den global, da jeg tænker vi ikke skal bruge samme styling i 'Map' */}
          <Route path="/" element={<div className='container mt-4'><h1>Home</h1></div>} /> { /* TODO: Home/Forside component*/ }
          <Route path="/employees" element={<EmployeeComponent />} />
          <Route path="/vehicles" element={<VehicleComponent />} />
          <Route path="/machines" element={<MachineryComponent />} />
          <Route path="/tools" element={<ToolComponent />} />
        </Routes>
      </Layout>
    </BrowserRouter>
  )
}

export default App
