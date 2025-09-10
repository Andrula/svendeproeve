import type { FunctionComponent } from "react";
import type { ToolStorageItem } from "../../Models/ToolStorageItem";
import { format } from "date-fns";
import type { Tool } from "../../Models/Tool";

export interface ToolModalProps {
  tools: ToolStorageItem[];
  defaultTools: Tool[];
  onClose: () => void;
}

export const MapToolModal: FunctionComponent<ToolModalProps> = ({
  tools,
  defaultTools,
  onClose,
}) => {
  console.log(tools);

  const activeTools = tools.filter((tool) => {
    const today = new Date();
    const start = new Date(tool.scheduledStart);
    const end = new Date(tool.scheduledEnd);
    return today >= start && today <= end;
  });

  const futureTools = tools.filter((tool) => {
    const today = new Date();
    const start = new Date(tool.scheduledStart);
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
            <h5 className="modal-title text-dark">Værktøj</h5>
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
            {activeTools.length > 0 ? (
              <>
                <h6>
                  <strong>Værktøj med en aktiv allokering:</strong>
                </h6>
                <div className="accordion" id="toolAccordion">
                  {activeTools.map((tool) => (
                    <div key={tool.itemId} className="accordion-item">
                      <h2
                        className="accordion-header"
                        id={`heading-${tool.itemId}`}
                      >
                        <button
                          className="accordion-button collapsed"
                          type="button"
                          data-bs-toggle="collapse"
                          data-bs-target={`#collapse-${tool.itemId}`}
                          aria-expanded="false"
                          aria-controls={`collapse-${tool.itemId}`}
                        >
                          <div className="d-flex justify-content-between align-items-center w-100 me-3">
                            <div>
                              <strong>
                                {tool.name}
                              </strong>
                            </div>
                            <small className="text-muted">
                              {format(
                                new Date(tool.scheduledStart),
                                "dd/MM"
                              )}{" "}
                              -{" "}
                              {format(new Date(tool.scheduledEnd), "dd/MM")}
                            </small>
                          </div>
                        </button>
                      </h2>
                      <div
                        id={`collapse-${tool.itemId}`}
                        className="accordion-collapse collapse"
                        aria-labelledby={`heading-${tool.itemId}`}
                        data-bs-parent="#toolAccordion"
                      >
                        <div className="accordion-body">
                          <div className="row">
                            {tool.itemNote && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">
                                  📒 Note for værktøj
                                </h6>
                                <p className="mb-0">{tool.itemNote}</p>
                              </div>
                            )}
                            {tool.storageItemNote && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">📒 Note for allokering</h6>
                                <p className="mb-0">
                                  {tool.storageItemNote}
                                </p>
                              </div>
                            )}
                          </div>
                          <div className="row">
                            <div className="col-12 mb-3">
                              <h6 className="mb-2">📅 Allokeringsperiode</h6>
                              <p className="mb-0">
                                {format(
                                  new Date(tool.scheduledStart),
                                  "dd-MM-yyyy"
                                )}{" "}
                                til{" "}
                                {format(
                                  new Date(tool.scheduledEnd),
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

            {defaultTools.length > 0 && (
              <>
                <hr className="mx-auto my-3"></hr>
                <h6>
                  <strong>
                    Værktøj allokeret til dette job gennem standardlager
                  </strong>
                </h6>
                <div className="accordion" id="toolAccordion">
                  {defaultTools.map((tool) => (
                    <div key={tool.itemId} className="accordion-item">
                      <h2
                        className="accordion-header"
                        id={`heading-${tool.itemId}`}
                      >
                        <button
                          className="accordion-button collapsed"
                          type="button"
                          data-bs-toggle="collapse"
                          data-bs-target={`#collapse-${tool.itemId}`}
                          aria-expanded="false"
                          aria-controls={`collapse-${tool.itemId}`}
                        >
                          <div className="d-flex justify-content-between align-items-center w-100 me-3">
                            <div>
                              <strong>
                                {tool.name}
                              </strong>
                            </div>
                          </div>
                        </button>
                      </h2>
                      <div
                        id={`collapse-${tool.itemId}`}
                        className="accordion-collapse collapse"
                        aria-labelledby={`heading-${tool.itemId}`}
                        data-bs-parent="#toolAccordion"
                      >
                        <div className="accordion-body">
                          <div className="row">
                            {tool.note && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">
                                  📒 Note for værktøj
                                </h6>
                                <p className="mb-0">{tool.note}</p>
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

            {futureTools.length > 0 && (
              <>
                <hr className="mx-auto my-3"></hr>
                <h6>
                  <strong>
                    Værktøj allokeret til dette job i fremtiden:
                  </strong>
                </h6>
                <div className="accordion" id="toolAccordion">
                  {futureTools.map((tool) => (
                    <div key={tool.itemId} className="accordion-item">
                      <h2
                        className="accordion-header"
                        id={`heading-${tool.itemId}`}
                      >
                        <button
                          className="accordion-button collapsed"
                          type="button"
                          data-bs-toggle="collapse"
                          data-bs-target={`#collapse-${tool.itemId}`}
                          aria-expanded="false"
                          aria-controls={`collapse-${tool.itemId}`}
                        >
                          <div className="d-flex justify-content-between align-items-center w-100 me-3">
                            <div>
                              <strong>
                                {tool.name}
                              </strong>
                            </div>
                            <small className="text-muted">
                              {format(
                                new Date(tool.scheduledStart),
                                "dd/MM"
                              )}{" "}
                              -{" "}
                              {format(new Date(tool.scheduledEnd), "dd/MM")}
                            </small>
                          </div>
                        </button>
                      </h2>
                      <div
                        id={`collapse-${tool.itemId}`}
                        className="accordion-collapse collapse"
                        aria-labelledby={`heading-${tool.itemId}`}
                        data-bs-parent="#toolAccordion"
                      >
                        <div className="accordion-body">
                          <div className="row">
                            {tool.itemNote && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">
                                  📒 Note for værktøj
                                </h6>
                                <p className="mb-0">{tool.itemNote}</p>
                              </div>
                            )}
                            {tool.storageItemNote && (
                              <div className="col-6 mb-3">
                                <h6 className="mb-2">📒 Note for allokering</h6>
                                <p className="mb-0">
                                  {tool.storageItemNote}
                                </p>
                              </div>
                            )}
                          </div>
                          <div className="row">
                            <div className="col-12 mb-3">
                              <h6 className="mb-2">📅 Allokeringsperiode</h6>
                              <p className="mb-0">
                                {format(
                                  new Date(tool.scheduledStart),
                                  "dd-MM-yyyy"
                                )}{" "}
                                til{" "}
                                {format(
                                  new Date(tool.scheduledEnd),
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
