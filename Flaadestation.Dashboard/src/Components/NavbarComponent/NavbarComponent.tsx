import React from "react";
import { Link } from "react-router-dom";

interface NavbarProps {
  toggleSidebar: () => void;
}

const NavbarComponent: React.FC<NavbarProps> = ({ toggleSidebar }) => {
  return (
    <nav className="navbar navbar-dark bg-dark">
      <div className="container-fluid d-flex align-items-center">
        <div className="d-flex align-items-center">
          <button className="btn btn-dark" onClick={toggleSidebar}>
            <span className="navbar-toggler-icon"></span>
          </button>
          <div className="vr bg-light mx-3"></div>
          <Link className="navbar-brand mb-0 text-app-primary" to="/">
            Flådestationen
          </Link>
        </div>

        <div className="ms-auto">
          {/* TODO: evt login/logud */}
        </div>
      </div>
    </nav>
  );
};

export default NavbarComponent;