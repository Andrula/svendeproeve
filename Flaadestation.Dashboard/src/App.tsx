import "./App.css";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min.js";
import "bootstrap-icons/font/bootstrap-icons.min.css";
import Layout from "./Layout";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import EmployeeComponent from "./Components/EmployeeComponent/EmployeeComponent";
import VehicleComponent from "./Components/VehicleComponent/VehicleComponent";
import MachineryComponent from "./Components/MachineryComponent/MachineryComponent";
import ToolComponent from "./Components/ToolComponent/ToolComponent";
import MapComponent from "./Components/MapComponent/MapComponent";
import { ProtectedRoute } from "./Auth/ProtectedRoute";
import HomeComponent from "./Components/HomeComponent/HomeComponent";
import LoginComponent from "./Components/LoginComponent/LoginComponent";
import RegisterComponent from "./Components/RegisterComponent/RegisterComponent";
import CheckoutComponent from "./Components/CheckoutComponent/CheckoutComponent";
import LicenseComponent from "./Components/LicenseComponent/LicenseComponent";

function App() {
  return (
    <BrowserRouter>
      <Layout>
        <Routes>
          <Route
            path="/"
            element={
              <div className="position-relative p-4 h-100">
                <div className="container">
                  <HomeComponent />
                </div>
              </div>
            }
          />
          <Route
            path="/login"
            element={
              <div className="position-relative p-4 h-100">
                <div className="container">
                  <LoginComponent />
                </div>
              </div>
            }
          />
          <Route
            path="/register"
            element={
              <div className="position-relative p-4 h-100">
                <div className="container">
                  <RegisterComponent />
                </div>
              </div>
            }
          />
          <Route
            path="/register/checkout"
            element={
              <div className="position-relative p-4 h-100">
                <div className="container">
                  <CheckoutComponent />
                </div>
              </div>
            }
          />
          <Route
            path="/employees"
            element={
              <div className="position-relative p-4 h-100">
                <div className="container">
                  <ProtectedRoute>
                    <EmployeeComponent />
                  </ProtectedRoute>
                </div>
              </div>
            }
          />
          <Route
            path="/vehicles"
            element={
              <div className="position-relative p-4 h-100">
                <div className="container">
                  <ProtectedRoute>
                    <VehicleComponent />
                  </ProtectedRoute>
                </div>
              </div>
            }
          />
          <Route
            path="/machines"
            element={
              <div className="position-relative p-4 h-100">
                <div className="container">
                  <ProtectedRoute>
                    <MachineryComponent />
                  </ProtectedRoute>
                </div>
              </div>
            }
          />
          <Route
            path="/tools"
            element={
              <div className="position-relative p-4 h-100">
                <div className="container">
                  <ProtectedRoute>
                    <ToolComponent />
                  </ProtectedRoute>
                </div>
              </div>
            }
          />
          <Route
            path="/licenses"
            element={
              <div className="position-relative p-4 h-100">
                <div className="container">
                  <ProtectedRoute>
                    <LicenseComponent />
                  </ProtectedRoute>
                </div>
              </div>
            }
          />
          <Route
            path="/map"
            element={
              <div className="position-relative h-100" style={{ padding: 0 }}>
                <ProtectedRoute>
                  <MapComponent />
                </ProtectedRoute>
              </div>
            }
          />
        </Routes>
      </Layout>
    </BrowserRouter>
  );
}

export default App;
