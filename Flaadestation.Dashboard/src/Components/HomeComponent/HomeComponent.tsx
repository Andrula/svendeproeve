import { NavLink } from "react-router-dom";
import { useAuth } from "../../Auth/AuthContext";

export default function HomeComponent() {
  const { user } = useAuth();

  return (
    <>
      <div className="d-grid gap-2 col-6 mx-auto">
        <h1 className="mb-5 text-center">Velkommen til Flådestationen</h1>

        {user ? (
          <></>
        ) : (
          <>
            <NavLink className="btn btn-primary" to="/login">
              Login
            </NavLink>
            <NavLink className="btn btn-primary" to="/register">
              Opret
            </NavLink>
          </>
        )}
      </div>
    </>
  );
}
