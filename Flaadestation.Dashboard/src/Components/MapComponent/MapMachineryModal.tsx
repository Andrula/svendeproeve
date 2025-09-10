import type { FunctionComponent } from "react";
import type { MachineryStorageItem } from "../../Models/MachineryStorageItem";
import { format } from "date-fns";
import type { Machinery } from "../../Models/Machinery";

export interface MachineryModalProps {
  machines: MachineryStorageItem[];
  defaultMachines: Machinery[];
  onClose: () => void;
}

export const MapMachineryModal: FunctionComponent<MachineryModalProps> = ({
  machines,
  defaultMachines,
  onClose,
}) => {
  console.log(machines);

  const activeMachines = machines.filter((machinery) => {
    const today = new Date();
    const start = new Date(machinery.scheduledStart);
    const end = new Date(machinery.scheduledEnd);
    return today >= start && today <= end;
  });

  const futureMachines = machines.filter((machinery) => {
    const today = new Date();
    const start = new Date(machinery.scheduledStart);
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
            <h5 className="modal-title text-dark">Maskiner</h5>
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
            {activeMachines.length > 0 ? (
              <>
                <h6>
                  <strong>Maskiner med en aktiv allokering</strong>
                </h6>
                <div className="accordion" id="machineryAccordion">
                  {activeMachines.map((machinery) => (
                    <div key={machinery.itemId} className="accordion-item">
                      <h2
                        className="accordion-header"
                        id={`heading-${machinery.itemId}`}
                      >
                        <button
                          className="accordion-button collapsed"
                          type="button"
                          data-bs-toggle="collapse"
                          data-bs-target={`#collapse-${machinery.itemId}`}
                          aria-expanded="false"
                          aria-controls={`collapse-${machinery.itemId}`}
                        >
                          <div className="d-flex justify-content-between align-items-center w-100 me-3">
                            <div>
                              <strong>
                                {machinery.name}
                              </strong>
                            </div>
                            <small className="text-muted">
                              {format(
                                new Date(machinery.scheduledStart),
                                "dd/MM"
                              )}{" "}
                              -{" "}
                              {format(new Date(machinery.scheduledEnd), "dd/MM")}
                            </small>
                          </div>
                        </button>
                      </h2>
                      <div
                        id={`collapse-${machinery.itemId}`}
                        className="accordion-collapse collapse"
                        aria-labelledby={`heading-${machinery.itemId}`}
                        data-bs-parent="#machineryAccordion"
                      >
                        <div className="accordion-body">
                          <div className="row">
                            {machinery.itemNote && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">
                                  📒 Note for maskine
                                </h6>
                                <p className="mb-0">{machinery.itemNote}</p>
                              </div>
                            )}
                            {machinery.storageItemNote && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">📒 Note for allokering</h6>
                                <p className="mb-0">
                                  {machinery.storageItemNote}
                                </p>
                              </div>
                            )}
                          </div>
                          <div className="row">
                            <div className="col-12 mb-3">
                              <h6 className="mb-2">📅 Allokeringsperiode</h6>
                              <p className="mb-0">
                                {format(
                                  new Date(machinery.scheduledStart),
                                  "dd-MM-yyyy"
                                )}{" "}
                                til{" "}
                                {format(
                                  new Date(machinery.scheduledEnd),
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
                Ingen aktivt allokerede værtkøjer
              </h6>
            )}

            {defaultMachines.length > 0 && (
              <>
                <hr className="mx-auto my-3"></hr>
                <h6>
                  <strong>
                    Maskiner allokeret til denne lokation gennem standardlager
                  </strong>
                </h6>
                <div className="accordion" id="machineryAccordion">
                  {defaultMachines.map((machinery) => (
                    <div key={machinery.itemId} className="accordion-item">
                      <h2
                        className="accordion-header"
                        id={`heading-${machinery.itemId}`}
                      >
                        <button
                          className="accordion-button collapsed"
                          type="button"
                          data-bs-toggle="collapse"
                          data-bs-target={`#collapse-${machinery.itemId}`}
                          aria-expanded="false"
                          aria-controls={`collapse-${machinery.itemId}`}
                        >
                          <div className="d-flex justify-content-between align-items-center w-100 me-3">
                            <div>
                              <strong>
                                {machinery.name}
                              </strong>
                            </div>
                          </div>
                        </button>
                      </h2>
                      <div
                        id={`collapse-${machinery.itemId}`}
                        className="accordion-collapse collapse"
                        aria-labelledby={`heading-${machinery.itemId}`}
                        data-bs-parent="#machineryAccordion"
                      >
                        <div className="accordion-body">
                          <div className="row">
                            {machinery.note && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">
                                  📒 Note for maskine
                                </h6>
                                <p className="mb-0">{machinery.note}</p>
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

            {futureMachines.length > 0 && (
              <>
                <hr className="mx-auto my-3"></hr>
                <h6>
                  <strong>
                    Maskiner allokeret til denne lokation i fremtiden
                  </strong>
                </h6>
                <div className="accordion" id="machineryAccordion">
                  {futureMachines.map((machinery) => (
                    <div key={machinery.itemId} className="accordion-item">
                      <h2
                        className="accordion-header"
                        id={`heading-${machinery.itemId}`}
                      >
                        <button
                          className="accordion-button collapsed"
                          type="button"
                          data-bs-toggle="collapse"
                          data-bs-target={`#collapse-${machinery.itemId}`}
                          aria-expanded="false"
                          aria-controls={`collapse-${machinery.itemId}`}
                        >
                          <div className="d-flex justify-content-between align-items-center w-100 me-3">
                            <div>
                              <strong>
                                {machinery.name}
                              </strong>
                            </div>
                            <small className="text-muted">
                              {format(
                                new Date(machinery.scheduledStart),
                                "dd/MM"
                              )}{" "}
                              -{" "}
                              {format(new Date(machinery.scheduledEnd), "dd/MM")}
                            </small>
                          </div>
                        </button>
                      </h2>
                      <div
                        id={`collapse-${machinery.itemId}`}
                        className="accordion-collapse collapse"
                        aria-labelledby={`heading-${machinery.itemId}`}
                        data-bs-parent="#machineryAccordion"
                      >
                        <div className="accordion-body">
                          <div className="row">
                            {machinery.itemNote && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">
                                  📒 Note for maskine
                                </h6>
                                <p className="mb-0">{machinery.itemNote}</p>
                              </div>
                            )}
                            {machinery.storageItemNote && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">📒 Note for allokering</h6>
                                <p className="mb-0">
                                  {machinery.storageItemNote}
                                </p>
                              </div>
                            )}
                          </div>
                          <div className="row">
                            <div className="col-12 mb-3">
                              <h6 className="mb-2">📅 Allokeringsperiode</h6>
                              <p className="mb-0">
                                {format(
                                  new Date(machinery.scheduledStart),
                                  "dd-MM-yyyy"
                                )}{" "}
                                til{" "}
                                {format(
                                  new Date(machinery.scheduledEnd),
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
