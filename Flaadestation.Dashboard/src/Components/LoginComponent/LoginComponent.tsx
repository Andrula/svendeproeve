import { Navigate } from "react-router-dom";
import { useAuth } from "../../Auth/AuthContext";
import { useState } from "react";
import { toast } from "react-toastify";

export default function LoginComponent() {
  const { user, login } = useAuth();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [errors, setErrors] = useState<{ email?: string; password?: string }>(
    {}
  );

  const validate = () => {
    const newErrors: typeof errors = {};

    if (!email) {
      newErrors.email = "Indtast venligst din email";
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      newErrors.email = "Ugyldig email";
    }

    if (!password) {
      newErrors.password = "Indtast venligst din adgangskode";
    } else if (password.length < 6) {
      newErrors.password = "Din adgangskode skal være på mindst 6 karakterer";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validate()) return;

    const success = await login(email, password);
    if (!success) toast.error("Der skete en fejl!");
    else toast.success("Du er nu logget ind!")
  };

  if (user) return <Navigate to="/" replace />;

  return (
    <div className="d-flex justify-content-center mt-5">
      <div className="card shadow-lg" style={{ width: "38rem" }}>
        <div className="card-header text-center bg-dark text-app-primary">
          <h4>Login</h4>
        </div>
        <form onSubmit={handleSubmit} noValidate>
          <div className="card-body">
            <div className=" form-floating mb-3">
              <input
                id="email"
                placeholder="mail@flaadestationen.dk"
                type="text"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className={`form-control ${errors.email ? "is-invalid" : ""}`}
              />
              {errors.email && (
                <div className="invalid-feedback">{errors.email}</div>
              )}
              <label htmlFor="email">Email</label>
            </div>

            <div className="form-floating mb-3">
              <input
                id="password"
                placeholder="password"
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className={`form-control ${
                  errors.password ? "is-invalid" : ""
                }`}
              />
              {errors.password && (
                <div className="invalid-feedback">{errors.password}</div>
              )}
              <label htmlFor="password">Adgangskode</label>
            </div>

            <div className="text-center mt-3">
              <button type="submit" className="btn btn-primary w-100">
                Login
              </button>
            </div>
          </div>
        </form>
      </div>
    </div>
  );
}
