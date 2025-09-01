import { useEffect, useState } from "react";
import { VehicleModel } from "../../Models/Vehicle";
import {
  DEFAULT_COMPANY_ID,
  HttpError,
  vehicleService,
} from "../../Services/VehicleService";

export default function VehicleComponent() {
  const [vehicles, setVehicles] = useState<VehicleModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadVehicles();
  }, []);

  const loadVehicles = async () => {
    try {
      setLoading(true);
      setError(null);

      if (!DEFAULT_COMPANY_ID) {
        throw new Error("Company ID ikke konfigureret. Kontakt administrator.");
      }

      const vehicleModels = await vehicleService.getAllVehicles(
        DEFAULT_COMPANY_ID
      );
      setVehicles(vehicleModels);
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

      setError(`Fejl ved indlæsning af køretøjer: ${errorMessage}`);
      console.error("Fejl ved indlæsning af køretøjer:", err);
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteVehicle = async (vehicleModel: VehicleModel) => {
    if (
      window.confirm(`Er du sikker på, at du vil slette ${vehicleModel.model}?`)
    ) {
      try {
        await vehicleService.deleteVehicle(vehicleModel.data.itemId);
        setVehicles(
          vehicles.filter((vehicle) => vehicle.data.itemId !== vehicleModel.data.itemId)
        );
      } catch (err) {
        console.error("Error deleting vehicle:", err);
      }
    }
  };

  if (loading) {
    return (
        <div className="d-flex justify-content-center">
          <div className="spinner-border" role="status">
            <span className="visually-hidden">Indlæser data...</span>
          </div>
          <span className="ms-2">Indlæser køretøjer...</span>
        </div>
    );
  }

  return (
    <>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>Køretøjer</h1>
      </div>

      {error && (
        <div className="alert alert-danger" role="alert">
          {error}
          <button 
            className="btn btn-link p-0 ms-2" 
            onClick={loadVehicles}
          >
            Prøv igen
          </button>
        </div>
      )}

      {!error && vehicles.length === 0 ? (
        <div className="alert alert-info">
          Ingen køretøjer fundet.
        </div>
      ) : (
        <div className="row">
          {vehicles.map((vehicle) => {
            const currentAssignment = vehicle.currentAssignment;
            
            return (
              <div
                key={vehicle.data.itemId}
                className="col-md-6 col-lg-4 mb-3"
              >
                <div className="card">
                  <div className="card-header bg-dark">
                    <h5 className="card-title">
                      <strong className="text-app-primary">
                        {vehicle.model}
                      </strong>
                      {vehicle.isAvailable && (
                        <span className="badge bg-success ms-2">Ledig</span>
                      )}
                    </h5>
                  </div>
                  <div className="card-body">
                    <div className="card-text">
                      {false ? (
                        <>
                          <p><strong>Nummerplade: </strong></p>
                          <p>{vehicle.licensePlate}</p>
                        </>
                      ) : (
                        <>
                          <p className="text-muted fst-italic">
                            ingen nummerplade registreret
                          </p>
                        </>
                      )}
                      <p><strong>Standard lager:</strong> {vehicle.data.defaultStorage.name}</p>
                      {vehicle.data.note && (
                        <>
                          <p><strong>Note:</strong> {vehicle.data.note}</p>
                        </>
                      )}
                      {currentAssignment ? (
                        <>
                          <strong>Nuværende opgave:</strong>
                          <div className="bg-light p-2 rounded small">

                          <small className="text-muted">
                            {currentAssignment.storage.name}
                            <br />
                            {currentAssignment.note}
                            <br />
                            {new Date(
                                currentAssignment.scheduledStart
                            ).toLocaleDateString("da-DK")}{" "}
                            -{" "}
                            {new Date(
                                currentAssignment.scheduledEnd
                            ).toLocaleDateString("da-DK")}
                          </small>
                            </div>
                        </>
                      ) : (
                        <>
                          <p className="text-muted fst-italic">
                            ingen nuværende opgaver
                          </p>
                        </>
                      )}
                      {vehicle.employees.length > 0 ? (
                        <>
                            <button className="btn btn-primary btn-sm mb-2">{vehicle.employees.length} Medarbejdere</button>
                            <br/>
                        </>
                      ) : (
                        <>
                            <p className="text-muted fst-italic">
                            ingen medarbejdere
                          </p>
                        </>
                      )}
                      {vehicle.tools.length > 0 ? (
                        <>
                            <button className="btn btn-primary btn-sm">{vehicle.tools.length} Værktøjer</button>
                            <br/>
                        </>
                      ) : (
                        <>
                            <p className="text-muted fst-italic">
                            ingen værktøjer
                          </p>
                        </>
                      )}
                    </div>
                    {}
                  </div>
                  <div className="card-footer d-flex justify-content-between">
                    <button className="btn btn-sm btn-outline-primary me-2">
                      <i className="bi bi-pencil"></i> Rediger
                    </button>
                    <button className="btn btn-sm btn-outline-danger"
                    onClick={() => handleDeleteVehicle(vehicle)}>
                      <i className="bi bi-trash"></i> Slet
                    </button>
                  </div>
                </div>
              </div>
            );
          })}
        </div>
      )}

      <div className="mt-3">
        <small className="text-muted">
          Viser {vehicles.length} køretøjer
        </small>
      </div>
    </>
  );
}
