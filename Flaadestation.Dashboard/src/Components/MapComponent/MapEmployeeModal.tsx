import type { FunctionComponent } from "react";
import type { EmployeeStorageItem } from "../../Models/EmployeeStorageItem";
import { format } from "date-fns";
import type { Employee } from "../../Models/Employee";

export interface EmployeeModalProps {
  employees: EmployeeStorageItem[];
  defaultEmployees: Employee[];
  onClose: () => void;
}

export const MapEmployeeModal: FunctionComponent<EmployeeModalProps> = ({
  employees,
  defaultEmployees,
  onClose,
}) => {
  console.log(employees);

  const activeEmployees = employees.filter((employee) => {
    const today = new Date();
    const start = new Date(employee.scheduledStart);
    const end = new Date(employee.scheduledEnd);
    return today >= start && today <= end;
  });

  const futureEmployees = employees.filter((employee) => {
    const today = new Date();
    const start = new Date(employee.scheduledStart);
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
            <h5 className="modal-title text-dark">Medarbejdere</h5>
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
            {activeEmployees.length > 0 ? (
              <>
                <h6>
                  <strong>Medarbejdere med en aktiv allokering</strong>
                </h6>
                <div className="accordion" id="employeeAccordion">
                  {activeEmployees.map((employee) => (
                    <div key={employee.itemId} className="accordion-item">
                      <h2
                        className="accordion-header"
                        id={`heading-${employee.itemId}`}
                      >
                        <button
                          className="accordion-button collapsed"
                          type="button"
                          data-bs-toggle="collapse"
                          data-bs-target={`#collapse-${employee.itemId}`}
                          aria-expanded="false"
                          aria-controls={`collapse-${employee.itemId}`}
                        >
                          <div className="d-flex justify-content-between align-items-center w-100 me-3">
                            <div>
                              <strong>
                                {employee.firstName} {employee.lastName}
                              </strong>
                            </div>
                            <small className="text-muted">
                              {format(
                                new Date(employee.scheduledStart),
                                "dd/MM"
                              )}{" "}
                              -{" "}
                              {format(new Date(employee.scheduledEnd), "dd/MM")}
                            </small>
                          </div>
                        </button>
                      </h2>
                      <div
                        id={`collapse-${employee.itemId}`}
                        className="accordion-collapse collapse"
                        aria-labelledby={`heading-${employee.itemId}`}
                        data-bs-parent="#employeeAccordion"
                      >
                        <div className="accordion-body">
                          <div className="row">
                            {employee.itemNote && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">
                                  📒 Note for medarbejder
                                </h6>
                                <p className="mb-0">{employee.itemNote}</p>
                              </div>
                            )}
                            {employee.storageItemNote && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">📒 Note for allokering</h6>
                                <p className="mb-0">
                                  {employee.storageItemNote}
                                </p>
                              </div>
                            )}
                          </div>
                          <div className="row">
                            <div className="col-12 mb-3">
                              <h6 className="mb-2">📅 Allokeringsperiode</h6>
                              <p className="mb-0">
                                {format(
                                  new Date(employee.scheduledStart),
                                  "dd-MM-yyyy"
                                )}{" "}
                                til{" "}
                                {format(
                                  new Date(employee.scheduledEnd),
                                  "dd-MM-yyyy"
                                )}
                              </p>
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
                Ingen aktivt allokerede medarbejdere
              </h6>
            )}

            {defaultEmployees.length > 0 && (
              <>
                <hr className="mx-auto my-3"></hr>
                <h6>
                  <strong>
                    Medarbejdere allokeret til denne lokation gennem standardlager
                  </strong>
                </h6>
                <div className="accordion" id="employeeAccordion">
                  {defaultEmployees.map((employee) => (
                    <div key={employee.itemId} className="accordion-item">
                      <h2
                        className="accordion-header"
                        id={`heading-${employee.itemId}`}
                      >
                        <button
                          className="accordion-button collapsed"
                          type="button"
                          data-bs-toggle="collapse"
                          data-bs-target={`#collapse-${employee.itemId}`}
                          aria-expanded="false"
                          aria-controls={`collapse-${employee.itemId}`}
                        >
                          <div className="d-flex justify-content-between align-items-center w-100 me-3">
                            <div>
                              <strong>
                                {employee.firstName} {employee.lastName}
                              </strong>
                            </div>
                          </div>
                        </button>
                      </h2>
                      <div
                        id={`collapse-${employee.itemId}`}
                        className="accordion-collapse collapse"
                        aria-labelledby={`heading-${employee.itemId}`}
                        data-bs-parent="#employeeAccordion"
                      >
                        <div className="accordion-body">
                          <div className="row">
                            {employee.note && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">
                                  📒 Note for medarbejder
                                </h6>
                                <p className="mb-0">{employee.note}</p>
                              </div>
                            )}
                          </div>
                        </div>
                      </div>
                    </div>
                  ))}
                </div>
              </>
            )}

            {futureEmployees.length > 0 && (
              <>
                <hr className="mx-auto my-3"></hr>
                <h6>
                  <strong>
                    Medarbejdere allokeret til denne lokation i fremtiden
                  </strong>
                </h6>
                <div className="accordion" id="employeeAccordion">
                  {futureEmployees.map((employee) => (
                    <div key={employee.itemId} className="accordion-item">
                      <h2
                        className="accordion-header"
                        id={`heading-${employee.itemId}`}
                      >
                        <button
                          className="accordion-button collapsed"
                          type="button"
                          data-bs-toggle="collapse"
                          data-bs-target={`#collapse-${employee.itemId}`}
                          aria-expanded="false"
                          aria-controls={`collapse-${employee.itemId}`}
                        >
                          <div className="d-flex justify-content-between align-items-center w-100 me-3">
                            <div>
                              <strong>
                                {employee.firstName} {employee.lastName}
                              </strong>
                            </div>
                            <small className="text-muted">
                              {format(
                                new Date(employee.scheduledStart),
                                "dd/MM"
                              )}{" "}
                              -{" "}
                              {format(new Date(employee.scheduledEnd), "dd/MM")}
                            </small>
                          </div>
                        </button>
                      </h2>
                      <div
                        id={`collapse-${employee.itemId}`}
                        className="accordion-collapse collapse"
                        aria-labelledby={`heading-${employee.itemId}`}
                        data-bs-parent="#employeeAccordion"
                      >
                        <div className="accordion-body">
                          <div className="row">
                            {employee.itemNote && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">
                                  📒 Note for medarbejder
                                </h6>
                                <p className="mb-0">{employee.itemNote}</p>
                              </div>
                            )}
                            {employee.storageItemNote && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">📒 Note for allokering</h6>
                                <p className="mb-0">
                                  {employee.storageItemNote}
                                </p>
                              </div>
                            )}
                          </div>
                          <div className="row">
                            <div className="col-12 mb-3">
                              <h6 className="mb-2">📅 Allokeringsperiode</h6>
                              <p className="mb-0">
                                {format(
                                  new Date(employee.scheduledStart),
                                  "dd-MM-yyyy"
                                )}{" "}
                                til{" "}
                                {format(
                                  new Date(employee.scheduledEnd),
                                  "dd-MM-yyyy"
                                )}
                              </p>
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
