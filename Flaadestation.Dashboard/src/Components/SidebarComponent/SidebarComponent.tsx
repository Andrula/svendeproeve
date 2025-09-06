import React from "react";
import "./SidebarComponent.css";
import { NavLink } from "react-router-dom";
import { useAuth } from "../../Auth/AuthContext";

interface SidebarProps {
  isOpen: boolean;
}

const SidebarComponent: React.FC<SidebarProps> = ({ isOpen }) => {
  const { user } = useAuth();
console.log(user);

  return (
    <div
      className={`bg-dark text-white position-fixed top-0 start-0 h-100 p-3 sidebar ${
        isOpen ? "sidebar-open" : ""
      }`}
    >
      <div className="d-flex justify-content-center mb-4">
        <i className="bi bi-geo-alt-fill h3 text-app-primary"></i>
      </div>
      <ul className="nav flex-column">
        <li className="nav-item">
          <NavLink
            to="/employees"
            className={({ isActive }) =>
              `nav-link sidebar-link ${isActive ? "active-link" : ""}`
            }
          >
            Medarbejdere
          </NavLink>
        </li>
        <li className="nav-item">
          <NavLink
            to="/vehicles"
            className={({ isActive }) =>
              `nav-link sidebar-link ${isActive ? "active-link" : ""}`
            }
          >
            Køretøjer
          </NavLink>
        </li>
        <li className="nav-item">
          <NavLink
            to="/machines"
            className={({ isActive }) =>
              `nav-link sidebar-link ${isActive ? "active-link" : ""}`
            }
          >
            Maskiner
          </NavLink>
        </li>
        <li className="nav-item">
          <NavLink
            to="/tools"
            className={({ isActive }) =>
              `nav-link sidebar-link ${isActive ? "active-link" : ""}`
            }
          >
            Værktøj
          </NavLink>
        </li>
          <li className="nav-item">
          <NavLink
            to="/map"
            className={({ isActive }) =>
              `nav-link sidebar-link ${isActive ? "active-link" : ""}`
            }
          >
            Kort
          </NavLink>
        </li>
        {user?.isCompanyOwner && (
          <li className="nav-item">
          <NavLink
            to="/licenses"
            className={({ isActive }) =>
              `nav-link sidebar-link ${isActive ? "active-link" : ""}`
            }
          >
            Licenser
          </NavLink>
        </li>
        )}
      </ul>
    </div>
  );
};

export default SidebarComponent;
