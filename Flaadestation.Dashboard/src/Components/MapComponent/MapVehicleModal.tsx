import type { FunctionComponent } from "react";
import type { VehicleStorageItem } from "../../Models/VehicleStorageItem";
import { format } from "date-fns";
import type { Vehicle } from "../../Models/Vehicle";

export interface VehicleModalProps {
  vehicles: VehicleStorageItem[];
  defaultVehicles: Vehicle[];
  onClose: () => void;
}

export const MapVehicleModal: FunctionComponent<VehicleModalProps> = ({
  vehicles,
  defaultVehicles,
  onClose,
}) => {
  const activeVehicles = vehicles.filter((vehicle) => {
    const today = new Date();
    const start = new Date(vehicle.scheduledStart);
    const end = new Date(vehicle.scheduledEnd);
    return today >= start && today <= end;
  });

  const futureVehicles = vehicles.filter((vehicle) => {
    const today = new Date();
    const start = new Date(vehicle.scheduledStart);
    return start > today;
  });

  return (
    <div
      className="position-fixed"
      style={{
        top: "55px",
        left: "0",
        right: 0,
        bottom: 0,
        backgroundColor: "rgba(0,0,0,0.7)",
        zIndex: 1000,
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        padding: "20px",
      }}
    >
      <div
        className="modal-dialog"
        style={{ margin: 0, maxWidth: "80%", width: "600px" }}
      >
        <div
          className="modal-content shadow-lg"
          style={{
            backgroundColor: "white",
            border: "1px solid #dee2e6",
            maxHeight: "70vh",
            display: "flex",
            flexDirection: "column",
          }}
        >
          <div
            className="modal-header"
            style={{
              backgroundColor: "#f8f9fa",
              borderBottom: "1px solid #dee2e6",
              padding: "16px 24px",
              flexShrink: 0,
            }}
          >
            <h5 className="modal-title text-dark">Køretøjer</h5>
            <button type="button" className="btn-close" onClick={onClose} />
          </div>
          <div
            className="modal-body"
            style={{
              backgroundColor: "white",
              padding: "24px",
              overflowY: "auto",
              flex: 1,
            }}
          >
            {activeVehicles.length > 0 ? (
              <>
                <h6>
                  <strong>Køretøjer med en aktiv allokering</strong>
                </h6>
                <div className="accordion" id="vehicleAccordion">
                  {activeVehicles.map((vehicle) => (
                    <div key={vehicle.itemId} className="accordion-item">
                      <h2
                        className="accordion-header"
                        id={`heading-${vehicle.itemId}`}
                      >
                        <button
                          className="accordion-button collapsed"
                          type="button"
                          data-bs-toggle="collapse"
                          data-bs-target={`#collapse-${vehicle.itemId}`}
                          aria-expanded="false"
                          aria-controls={`collapse-${vehicle.itemId}`}
                        >
                          <div className="d-flex justify-content-between align-items-center w-100 me-3">
                            <div>
                              <strong>{vehicle.model}</strong>
                              <span className="text-muted ms-2">
                                ({vehicle.licensePlate})
                              </span>
                            </div>
                            <small className="text-muted">
                              {format(
                                new Date(vehicle.scheduledStart),
                                "dd/MM"
                              )}{" "}
                              -{" "}
                              {format(new Date(vehicle.scheduledEnd), "dd/MM")}
                            </small>
                          </div>
                        </button>
                      </h2>
                      <div
                        id={`collapse-${vehicle.itemId}`}
                        className="accordion-collapse collapse"
                        aria-labelledby={`heading-${vehicle.itemId}`}
                        data-bs-parent="#vehicleAccordion"
                      >
                        <div className="accordion-body">
                          <div className="row">
                            {vehicle.itemNote && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">📒 Note for køretøj</h6>
                                <p className="mb-0">{vehicle.itemNote}</p>
                              </div>
                            )}
                            {vehicle.storageItemNote && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">
                                  📒 Note for allokering
                                </h6>
                                <p className="mb-0">
                                  {vehicle.storageItemNote}
                                </p>
                              </div>
                            )}
                          </div>
                          <div className="row">
                            <div className="col-12 mb-3">
                              <h6 className="mb-2">📅 Allokeringsperiode</h6>
                              <p className="mb-0">
                                {format(
                                  new Date(vehicle.scheduledStart),
                                  "dd-MM-yyyy"
                                )}{" "}
                                til{" "}
                                {format(
                                  new Date(vehicle.scheduledEnd),
                                  "dd-MM-yyyy"
                                )}
                              </p>
                            </div>

                            <div className="col-md-6 mb-3">
                              <h6 className="mb-2">👥 Medarbejdere</h6>
                              {vehicle.employees.length > 0 ? (
                                <div>
                                  {vehicle.employees.map((employee) => (
                                    <span
                                      key={employee.itemId}
                                      className="badge bg-primary me-1 mb-1"
                                    >
                                      {employee.firstName} {employee.lastName}
                                    </span>
                                  ))}
                                </div>
                              ) : (
                                <p className="text-muted fst-italic mb-0">
                                  Ingen medarbejdere allokeret
                                </p>
                              )}
                            </div>

                            <div className="col-md-6 mb-3">
                              <h6 className="mb-2">🔧 Værktøj</h6>
                              {vehicle.tools.length > 0 ? (
                                <div>
                                  {vehicle.tools.map((tool) => (
                                    <span
                                      key={tool.itemId}
                                      className="badge bg-success me-1 mb-1"
                                    >
                                      {tool.name}
                                    </span>
                                  ))}
                                </div>
                              ) : (
                                <p className="text-muted fst-italic mb-0">
                                  Intet værktøj allokeret
                                </p>
                              )}
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  ))}
                </div>
              </>
            ) : (
              <h6 className="text-muted fst-italic">
                Ingen aktivt allokerede køretøjer
              </h6>
            )}

            {defaultVehicles.length > 0 && (
              <>
                <hr className="mx-auto my-3"></hr>
                <h6>
                  <strong>
                    Køretøjer allokeret til denne lokation gennem standardlager
                  </strong>
                </h6>
                <div className="accordion" id="vehicleAccordion">
                  {defaultVehicles.map((vehicle) => (
                    <div key={vehicle.itemId} className="accordion-item">
                      <h2
                        className="accordion-header"
                        id={`heading-${vehicle.itemId}`}
                      >
                        <button
                          className="accordion-button collapsed"
                          type="button"
                          data-bs-toggle="collapse"
                          data-bs-target={`#collapse-${vehicle.itemId}`}
                          aria-expanded="false"
                          aria-controls={`collapse-${vehicle.itemId}`}
                        >
                          <div className="d-flex justify-content-between align-items-center w-100 me-3">
                            <div>
                              <strong>{vehicle.model}</strong>
                              <span className="text-muted ms-2">
                                ({vehicle.licensePlate})
                              </span>
                            </div>
                          </div>
                        </button>
                      </h2>
                      <div
                        id={`collapse-${vehicle.itemId}`}
                        className="accordion-collapse collapse"
                        aria-labelledby={`heading-${vehicle.itemId}`}
                        data-bs-parent="#vehicleAccordion"
                      >
                        <div className="accordion-body">
                          {vehicle.note && (
                            <div className="row">
                              <div className="col-md-6 mb-3">
                                <h6 className="mb-2">📒 Note for køretøj</h6>
                                <p className="mb-0">{vehicle.note}</p>
                              </div>
                            </div>
                          )}
                          <div className="row">
                            <div className="col-md-6 mb-3">
                              <h6 className="mb-2">👥 Medarbejdere</h6>
                              {vehicle.employees.length > 0 ? (
                                <div>
                                  {vehicle.employees.map((employee) => (
                                    <span
                                      key={employee.itemId}
                                      className="badge bg-primary me-1 mb-1"
                                    >
                                      {employee.firstName} {employee.lastName}
                                    </span>
                                  ))}
                                </div>
                              ) : (
                                <p className="text-muted fst-italic mb-0">
                                  Ingen medarbejdere allokeret
                                </p>
                              )}
                            </div>

                            <div className="col-md-6 mb-3">
                              <h6 className="mb-2">🔧 Værktøj</h6>
                              {vehicle.tools.length > 0 ? (
                                <div>
                                  {vehicle.tools.map((tool) => (
                                    <span
                                      key={tool.itemId}
                                      className="badge bg-success me-1 mb-1"
                                    >
                                      {tool.name}
                                    </span>
                                  ))}
                                </div>
                              ) : (
                                <p className="text-muted fst-italic mb-0">
                                  Intet værktøj allokeret
                                </p>
                              )}
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  ))}
                </div>
              </>
            )}

            {futureVehicles.length > 0 && (
              <>
                <hr className="mx-auto my-3"></hr>
                <h6>
                  <strong>
                    Køretøjer allokeret til denne lokation  i fremtiden
                  </strong>
                </h6>
                <div className="accordion" id="vehicleAccordion">
                  {futureVehicles.map((vehicle) => (
                    <div key={vehicle.itemId} className="accordion-item">
                      <h2
                        className="accordion-header"
                        id={`heading-${vehicle.itemId}`}
                      >
                        <button
                          className="accordion-button collapsed"
                          type="button"
                          data-bs-toggle="collapse"
                          data-bs-target={`#collapse-${vehicle.itemId}`}
                          aria-expanded="false"
                          aria-controls={`collapse-${vehicle.itemId}`}
                        >
                          <div className="d-flex justify-content-between align-items-center w-100 me-3">
                            <div>
                              <strong>{vehicle.model}</strong>
                              <span className="text-muted ms-2">
                                ({vehicle.licensePlate})
                              </span>
                            </div>
                            <small className="text-muted">
                              {format(
                                new Date(vehicle.scheduledStart),
                                "dd/MM"
                              )}{" "}
                              -{" "}
                              {format(new Date(vehicle.scheduledEnd), "dd/MM")}
                            </small>
                          </div>
                        </button>
                      </h2>
                      <div
                        id={`collapse-${vehicle.itemId}`}
                        className="accordion-collapse collapse"
                        aria-labelledby={`heading-${vehicle.itemId}`}
                        data-bs-parent="#vehicleAccordion"
                      >
                        <div className="accordion-body">
                          <div className="row">
                            {vehicle.itemNote && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">📒 Note for køretøj</h6>
                                <p className="mb-0">{vehicle.itemNote}</p>
                              </div>
                            )}
                            {vehicle.storageItemNote && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">📒 Note for allokering</h6>
                                <p className="mb-0">
                                  {vehicle.storageItemNote}
                                </p>
                              </div>
                            )}
                          </div>
                          <div className="row">
                            <div className="col-12 mb-3">
                              <h6 className="mb-2">📅 Allokeringsperiode</h6>
                              <p className="mb-0">
                                {format(
                                  new Date(vehicle.scheduledStart),
                                  "dd-MM-yyyy"
                                )}{" "}
                                til{" "}
                                {format(
                                  new Date(vehicle.scheduledEnd),
                                  "dd-MM-yyyy"
                                )}
                              </p>
                            </div>

                            <div className="col-md-6 mb-3">
                              <h6 className="mb-2">👥 Medarbejdere</h6>
                              {vehicle.employees.length > 0 ? (
                                <div>
                                  {vehicle.employees.map((employee) => (
                                    <span
                                      key={employee.itemId}
                                      className="badge bg-primary me-1 mb-1"
                                    >
                                      {employee.firstName} {employee.lastName}
                                    </span>
                                  ))}
                                </div>
                              ) : (
                                <p className="text-muted fst-italic mb-0">
                                  Ingen medarbejdere allokeret
                                </p>
                              )}
                            </div>

                            <div className="col-md-6 mb-3">
                              <h6 className="mb-2">🔧 Værktøj</h6>
                              {vehicle.tools.length > 0 ? (
                                <div>
                                  {vehicle.tools.map((tool) => (
                                    <span
                                      key={tool.itemId}
                                      className="badge bg-success me-1 mb-1"
                                    >
                                      {tool.name}
                                    </span>
                                  ))}
                                </div>
                              ) : (
                                <p className="text-muted fst-italic mb-0">
                                  Intet værktøj allokeret
                                </p>
                              )}
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  ))}
                </div>
              </>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};
