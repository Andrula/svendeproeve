import { useEffect, useState } from "react";
import type { LicenseModel } from "../../Models/License";
import { HttpError, licenseService } from "../../Services/LicenseService";
import { useAuth } from "../../Auth/AuthContext";
import { Navigate } from "react-router-dom";
import { format } from "date-fns";

export default function LicenseComponent() {
  const { user, loading } = useAuth();
  const [licenses, setLicenses] = useState<LicenseModel[]>([]);
  const [loadingLicenses, setLoadingLicenses] = useState(true);
  const [error, setError] = useState<string | null>(null);

  if (loading)
    return (
      <div className="container mt-4">
        <div className="d-flex justify-content-center">
          <div className="spinner-border" role="status">
            <span className="visually-hidden">Indlæser data...</span>
          </div>
          <span className="ms-2">Indlæser bruger...</span>
        </div>
      </div>
    );

  if (!user?.isCompanyOwner) <Navigate to="/" replace />;

  useEffect(() => {
    loadLicenses();
  }, []);

  const loadLicenses = async () => {
    try {
      setLoadingLicenses(true);
      setError(null);

      const licenseModels = await licenseService.getLicensesByCompany();
      setLicenses(licenseModels);
    } catch (err) {
      let errorMessage = "Ukendt fejl";

      if (err instanceof HttpError) {
        if (err.isClientError) {
          errorMessage = `Klient fejl (${err.status}): ${err.message}`;
        } else if (err.isServerError) {
          errorMessage = `Server fejl (${err.status}): ${err.message}`;
        } else {
          errorMessage = err.message;
        }
      } else if (err instanceof Error) {
        errorMessage = err.message;
      }

      setError(`Fejl ved indlæsning af licenser: ${errorMessage}`);
      console.error("Fejl ved indlæsning af licenser:", err);
    } finally {
      setLoadingLicenses(false);
    }
  };

  if (loadingLicenses) {
    return (
      <div className="container mt-4">
        <div className="d-flex justify-content-center">
          <div className="spinner-border" role="status">
            <span className="visually-hidden">Indlæser data...</span>
          </div>
          <span className="ms-2">Indlæser licenser...</span>
        </div>
      </div>
    );
  }

  return (
    <div className="position-relative">
      <div className="container mt-4">
        <div className="d-flex justify-content-between align-items-center mb-4">
          <h1>Licenser</h1>
        </div>
        {error && (
          <div className="alert alert-danger" role="alert">
            {error}
            <button className="btn btn-link p-0 ms-2" onClick={loadLicenses}>
              Prøv igen
            </button>
          </div>
        )}

        {!error && licenses.length === 0 ? (
          <div className="alert alert-info">Ingen licenser fundet.</div>
        ) : (
          <div className="row">
            {licenses.map((license) => {
              const isAvailable = license.data.user;

              return (
                <div
                  key={license.data.licenseId}
                  className="col-md-6 col-lg-6 d-flex"
                >
                  <div className="card w-100">
                    <div className="card-header bg-dark">
                      <h5 className="card-title d-flex justify-content-between align-items-center">
                        <strong className="text-app-primary">
                          {license.data.licenseKey.toUpperCase()}
                        </strong>
                        {isAvailable && (
                          <span className="badge bg-success">Ledig</span>
                        )}
                      </h5>
                    </div>
                    <div className="card-body">
                      <div className="card-text">
                        <p>
                          <strong>Bruger</strong>
                        </p>
                        {isAvailable ? (
                          <p>{license.data.user?.email}</p>
                        ) : (
                          <p className="text-muted fst-italic">
                            Endnu ingen brugere på denne licens
                          </p>
                        )}
                      </div>
                    </div>
                    <div className="card-footer text-center">
                      <p className="text-muted">
                        <strong>
                          {format(
                            new Date(license.data.validFrom),
                            "dd-MM-yyyy"
                          )}{" "}
                          {"-"}{" "}
                          {format(new Date(license.data.validTo), "dd-MM-yyyy")}
                        </strong>
                      </p>
                    </div>
                  </div>
                </div>
              );
            })}
          </div>
        )}
      </div>
    </div>
  );
}
