import "./App.css";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-icons/font/bootstrap-icons.min.css";
import Layout from "./Layout";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import EmployeeComponent from "./Components/EmployeeComponent/EmployeeComponent";
import VehicleComponent from "./Components/VehicleComponent/VehicleComponent";
import MachineryComponent from "./Components/MachineryComponent/MachineryComponent";
import ToolComponent from "./Components/ToolComponent/ToolComponent";
import MapComponent from "./Components/MapComponent/MapComponent";

function App() {
  return (
    <BrowserRouter>
      <Layout>
        <Routes>
          <Route
            path="/"
            element={
              <div className="container mt-4">
                <h1>Home</h1>
              </div>
            }
          />{" "}
          {/* TODO: Home/Forside component*/}
          <Route
            path="/employees"
            element={
              <div className="position-relative p-4 h-100">
                <div className="container">
                  <EmployeeComponent />
                </div>
              </div>
            }
          />
          <Route
            path="/vehicles"
            element={
              <div className="position-relative p-4 h-100">
                <div className="container">
                  <VehicleComponent />
                </div>
              </div>
            }
          />
          <Route
            path="/machines"
            element={
              <div className="position-relative p-4 h-100">
                <div className="container">
                  <MachineryComponent />
                </div>
              </div>
            }
          />
          <Route
            path="/tools"
            element={
              <div className="position-relative p-4 h-100">
                <div className="container">
                  <ToolComponent />
                </div>
              </div>
            }
          />
          <Route path="/map" element={<MapComponent />} />
        </Routes>
      </Layout>
    </BrowserRouter>
  );
}

export default App;
