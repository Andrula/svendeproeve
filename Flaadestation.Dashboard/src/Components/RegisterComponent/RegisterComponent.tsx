import { Navigate, useNavigate } from "react-router-dom";
import { useAuth } from "../../Auth/AuthContext";
import { useState } from "react";
import "./RegisterComponent.css";
import type { CreateOwnerFormData } from "../CheckoutComponent/CheckoutComponent";
import { toast } from "react-toastify";

export default function RegisterComponent() {
  const navigate = useNavigate();
  const { user, registerStaff } = useAuth();

  const [ownerEmail, setEmail] = useState("");
  const [ownerPassword, setPassword] = useState("");
  const [ownerConfirmPassword, setOwnerConfirmPassword] = useState("");
  const [ownerCompany, setCompany] = useState("");
  const [ownerAmountOfLicenses, setLicenses] = useState<number>(1);
  const [ownerErrors, setErrors] = useState<{
    email?: string;
    password?: string;
    confirmPassword?: string;
    company?: string;
    amountOfLicenses?: string;
  }>({});

  const [staffEmail, setStaffEmail] = useState("");
  const [staffPassword, setStaffPassword] = useState("");
  const [staffConfirmPassword, setStaffConfirmPassword] = useState("");
  const [staffLicenseKey, setStaffLicenseKey] = useState("");
  const [staffErrors, setStaffErrors] = useState<{
    email?: string;
    password?: string;
    confirmPassword?: string;
    licenseKey?: string;
  }>({});

  if (user) return <Navigate to="/" replace />;

  const [activeTab, setActiveTab] = useState<"owner" | "staff">("owner");
  const validateOwnerForm = () => {
    const newErrors: typeof ownerErrors = {};

    if (!ownerEmail) {
      newErrors.email = "Indtast venligst din email";
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(ownerEmail)) {
      newErrors.email = "Ugyldig email";
    }

    if (!ownerPassword) {
      newErrors.password = "Indtast venligst en adgangskode";
    } else if (ownerPassword.length < 6) {
      newErrors.password = "Din adgangskode skal være på mindst 6 karakterer";
    }

    if (ownerConfirmPassword !== ownerPassword) {
      newErrors.confirmPassword = "Adgangskoderne matcher ikke";
    }

    if (!ownerCompany) {
      newErrors.company = "Indtast venligst et firmanavn";
    } else if (ownerCompany.length <= 2) {
      newErrors.email = "Dit firmanavn skal være på mindst 3 karakterer";
    }

    if (ownerAmountOfLicenses <= 0) {
      newErrors.amountOfLicenses = "Indtast venligst et gyldigt antal licenser";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const validateStaffForm = () => {
    const newErrors: typeof staffErrors = {};

    if (!staffEmail) {
      newErrors.email = "Indtast venligst din email";
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(staffEmail)) {
      newErrors.email = "Ugyldig email";
    }

    if (!staffPassword) {
      newErrors.password = "Indtast venligst en adgangskode";
    } else if (staffPassword.length < 6) {
      newErrors.password = "Din adgangskode skal være på mindst 6 karakterer";
    }

    if (staffConfirmPassword !== staffPassword) {
      newErrors.confirmPassword = "Adgangskoderne matcher ikke";
    }

    if (!staffLicenseKey) {
      newErrors.licenseKey = "Indtast venligst din licensnøgle";
    } else if (
      !/^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[1-5][0-9a-fA-F]{3}-[89abAB][0-9a-fA-F]{3}-[0-9a-fA-F]{12}$/.test(
        staffLicenseKey
      )
    ) {
      newErrors.licenseKey = "Ugyldig GUID licensnøgle";
    }

    setStaffErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmitOwnerForm = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validateOwnerForm()) return;

    const formData: CreateOwnerFormData = {
      email: ownerEmail,
      password: ownerPassword,
      company: ownerCompany,
      amountOfLicenses: ownerAmountOfLicenses,
    };

    navigate("/register/checkout", { state: formData });
  };

  const handleSubmitStaffForm = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validateStaffForm()) return;

    const success = await registerStaff({
      email: staffEmail,
      password: staffPassword,
      licenseKey: staffLicenseKey,
    });

    if (success) {
      toast.success("Bruger oprettet!");
      navigate("/login", { replace: true });
    } else {
      toast.error("Ugyldig licensnøgle");
    }
  };

  return (
    <div className="d-flex justify-content-center mt-5">
      <div className="card shadow-lg" style={{ width: "38rem" }}>
        <div className="card-header p-0">
          <ul className="nav nav-tabs nav-fill">
            <li className="nav-item">
              <button
                className={`nav-link ${activeTab === "owner" ? "active" : ""}`}
                onClick={() => setActiveTab("owner")}
              >
                Opret som arbejdsgiver
              </button>
            </li>
            <li className="nav-item">
              <button
                className={`nav-link ${activeTab === "staff" ? "active" : ""}`}
                onClick={() => setActiveTab("staff")}
              >
                Opret som administration
              </button>
            </li>
          </ul>
        </div>

        <div className="card-body">
          {activeTab === "owner" && (
            <form onSubmit={handleSubmitOwnerForm} noValidate>
              <div className="form-floating mb-3">
                <input
                  id="email"
                  placeholder="mail@flaadestationen.dk"
                  type="text"
                  value={ownerEmail}
                  onChange={(e) => setEmail(e.target.value)}
                  className={`form-control ${
                    ownerErrors.email ? "is-invalid" : ""
                  }`}
                />
                {ownerErrors.email && (
                  <div className="invalid-feedback">{ownerErrors.email}</div>
                )}
                <label htmlFor="email" className="form-label">
                  Email
                </label>
              </div>
              <div className="form-floating mb-3">
                <input
                  id="password"
                  placeholder="password"
                  type="password"
                  value={ownerPassword}
                  onChange={(e) => setPassword(e.target.value)}
                  className={`form-control ${
                    ownerErrors.password ? "is-invalid" : ""
                  }`}
                />
                {ownerErrors.password && (
                  <div className="invalid-feedback">{ownerErrors.password}</div>
                )}
                <label htmlFor="password" className="form-label">
                  Adgangskode
                </label>
              </div>

              <div className="form-floating mb-3">
                <input
                  id="ownerConfirmPassword"
                  type="password"
                  value={ownerConfirmPassword}
                  onChange={(e) => setOwnerConfirmPassword(e.target.value)}
                  className={`form-control ${
                    ownerErrors.confirmPassword ? "is-invalid" : ""
                  }`}
                  placeholder="confirm password"
                />
                {ownerErrors.confirmPassword && (
                  <div className="invalid-feedback">
                    {ownerErrors.confirmPassword}
                  </div>
                )}
                <label htmlFor="ownerConfirmPassword">
                  Bekræft adgangskode
                </label>
              </div>

              <div className="form-floating mb-3">
                <input
                  id="company"
                  placeholder="firma"
                  type="text"
                  value={ownerCompany}
                  onChange={(e) => setCompany(e.target.value)}
                  className={`form-control ${
                    ownerErrors.email ? "is-invalid" : ""
                  }`}
                />
                {ownerErrors.email && (
                  <div className="invalid-feedback">{ownerErrors.email}</div>
                )}
                <label htmlFor="company" className="form-label">
                  Firmanavn
                </label>
              </div>
              <div className="form-floating mb-3">
                <input
                  id="licenses"
                  placeholder="Antal licenser"
                  type="number"
                  min={1}
                  max={20}
                  step={1}
                  value={ownerAmountOfLicenses}
                  onChange={(e) => {
                    const val = e.target.value;
                    let intVal = parseInt(val);
                    if (!isNaN(intVal)) {
                      if (intVal > 20) intVal = 20;
                      if (intVal <= 0) intVal = 1;

                      setLicenses(intVal);
                    } else e.target.value = ownerAmountOfLicenses.toString();
                  }}
                  className={`form-control ${
                    ownerErrors.amountOfLicenses ? "is-invalid" : ""
                  }`}
                />
                {ownerErrors.amountOfLicenses && (
                  <div className="invalid-feedback">
                    {ownerErrors.amountOfLicenses}
                  </div>
                )}
                <label htmlFor="licenses" className="form-label">
                  Antal licenser
                </label>
              </div>
              <button type="submit" className="btn btn-primary w-100">
                Fortsæt til betaling
              </button>
            </form>
          )}

          {activeTab === "staff" && (
            <form onSubmit={handleSubmitStaffForm} noValidate>
              <div className="form-floating mb-3">
                <input
                  id="staffEmail"
                  placeholder="mail@flaadestationen.dk"
                  type="text"
                  value={staffEmail}
                  onChange={(e) => setStaffEmail(e.target.value)}
                  className={`form-control ${
                    staffErrors.email ? "is-invalid" : ""
                  }`}
                />
                {staffErrors.email && (
                  <div className="invalid-feedback">{staffErrors.email}</div>
                )}
                <label htmlFor="staffEmail">Email</label>
              </div>

              <div className="form-floating mb-3">
                <input
                  id="staffPassword"
                  placeholder="password"
                  type="password"
                  value={staffPassword}
                  onChange={(e) => setStaffPassword(e.target.value)}
                  className={`form-control ${
                    staffErrors.password ? "is-invalid" : ""
                  }`}
                />
                {staffErrors.password && (
                  <div className="invalid-feedback">{staffErrors.password}</div>
                )}
                <label htmlFor="staffPassword">Adgangskode</label>
              </div>

              <div className="form-floating mb-3">
                <input
                  id="staffConfirmPassword"
                  type="password"
                  value={staffConfirmPassword}
                  onChange={(e) => setStaffConfirmPassword(e.target.value)}
                  className={`form-control ${
                    staffErrors.confirmPassword ? "is-invalid" : ""
                  }`}
                  placeholder="confirm password"
                />
                {staffErrors.confirmPassword && (
                  <div className="invalid-feedback">
                    {staffErrors.confirmPassword}
                  </div>
                )}
                <label htmlFor="staffConfirmPassword">
                  Bekræft adgangskode
                </label>
              </div>

              <div className="form-floating mb-3">
                <input
                  id="staffLicenseKey"
                  placeholder="XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"
                  type="text"
                  value={staffLicenseKey}
                  onChange={(e) => setStaffLicenseKey(e.target.value)}
                  className={`form-control ${
                    staffErrors.licenseKey ? "is-invalid" : ""
                  }`}
                />
                {staffErrors.licenseKey && (
                  <div className="invalid-feedback">
                    {staffErrors.licenseKey}
                  </div>
                )}
                <label htmlFor="staffLicenseKey">Licensnøgle</label>
              </div>

              <button type="submit" className="btn btn-primary w-100">
                Opret
              </button>
            </form>
          )}
        </div>
      </div>
    </div>
  );
}
