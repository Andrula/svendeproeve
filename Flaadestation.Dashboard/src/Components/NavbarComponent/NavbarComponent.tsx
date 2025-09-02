import React from "react";
import { Link } from "react-router-dom";
import { useAuth } from "../../Auth/AuthContext";
import { toast } from "react-toastify";

interface NavbarProps {
  toggleSidebar: () => void;
}

const NavbarComponent: React.FC<NavbarProps> = ({ toggleSidebar }) => {
  const { user, logout } = useAuth();

  async function handleLogout() {
    await logout();
    toast.success("Du er nu logget ud!");
  }

  return (
    <nav className="navbar navbar-dark bg-dark">
      <div className="container-fluid d-flex align-items-center">
        <div className="d-flex align-items-center">
          {user && (
            <>
              <button className="btn btn-dark" onClick={toggleSidebar}>
                <span className="navbar-toggler-icon"></span>
              </button>
              <div className="vr bg-light mx-3"></div>
            </>
          )}
          <Link className="navbar-brand mb-0 text-app-primary" to="/">
            Flådestationen
          </Link>
        </div>
        {user && (
          <div className="ms-auto">
            <button className="btn btn-danger" onClick={handleLogout}>
              Log ud
            </button>
          </div>
        )}
      </div>
    </nav>
  );
};

export default NavbarComponent;
