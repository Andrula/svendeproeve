import React, { useState } from "react";
import type { PropsWithChildren } from "react";
import "./Layout.css";
import SidebarComponent from "./Components/SidebarComponent/SidebarComponent";
import NavbarComponent from "./Components/NavbarComponent/NavbarComponent";
import { ToastContainer } from "react-toastify";
import { useAuth } from "./Auth/AuthContext";

const Layout: React.FC<PropsWithChildren> = ({ children }) => {
  const { user } = useAuth();
  const [isOpen, setIsOpen] = useState(false);

  return (
    <div className="d-flex min-vh-100">
      {user && <SidebarComponent isOpen={isOpen} />}

      <div
        className={`d-flex flex-column page-content ${isOpen ? "shifted" : ""}`}
      >
        <NavbarComponent toggleSidebar={() => setIsOpen(!isOpen)} />
        <main className="flex-grow-1">
          <ToastContainer className="mt-5" theme="colored" />
          {children}
        </main>
      </div>
    </div>
  );
};

export default Layout;
