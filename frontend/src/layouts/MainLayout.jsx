// src/layouts/MainLayout.jsx
import React from "react";
import { NavLink, Outlet } from "react-router-dom";
import '../Index.css';

const MainLayout = () => {
  return (
    <div className="flex h-screen bg-gray-50">
      {/* Sidebar */}
      <aside className="w-72 bg-white border-r border-slate-200 flex flex-col gap-4 p-4">
        <div className="flex flex-col p-2">
          <h1 className="text-primary text-xl font-semibold">Gestión Fondos</h1>
          <p className="text-secondary text-sm font-normal">Plataforma</p>
        </div>
        <nav className="flex flex-col gap-3 font-semibold">
          <div>
            <NavLink
              to="/clients"
              className={({ isActive }) =>
                `block px-3 py-2 rounded-md transition-colors ${
                  isActive
                    ? "bg-accent/8 text-accent"
                    : "text-primary hover:bg-accent/8 hover:text-accent"
                }`
              }
            >
              Clientes
            </NavLink>
          </div>
          <div>
            <NavLink
              to="/funds"
              className={({ isActive }) =>
                `block px-3 py-2 rounded-md transition-colors ${
                  isActive
                    ? "bg-accent/8 text-accent"
                    : "text-primary hover:bg-accent/8 hover:text-accent"
                }`
              }
            >
              Fondos
            </NavLink>
          </div>
        </nav>
      </aside>


      <div className="flex flex-col flex-1">
       
        <main className="flex-1 p-4">
          <Outlet />
        </main>
      </div>
    </div>
  );
};

export default MainLayout;
