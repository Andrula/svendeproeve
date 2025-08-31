import React, { useState } from "react";
import type { PropsWithChildren } from "react";
import "./Layout.css";
import SidebarComponent from "./Components/SidebarComponent/SidebarComponent";
import NavbarComponent from "./Components/NavbarComponent/NavbarComponent";

const Layout: React.FC<PropsWithChildren> = ({ children }) => {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <div className="d-flex min-vh-100">
      <SidebarComponent isOpen={isOpen} />

      <div className={`d-flex flex-column page-content ${isOpen ? "shifted" : ""}`}>
        <NavbarComponent toggleSidebar={() => setIsOpen(!isOpen)} />
        <main className="flex-grow-1">{children}</main>
      </div>
      
    </div>
  );
};

export default Layout;
