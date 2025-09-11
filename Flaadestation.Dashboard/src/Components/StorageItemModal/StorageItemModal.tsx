import { useEffect, useState, type FunctionComponent } from "react";
import type { StorageItemModel } from "../../Models/StorageItem";
import { BaseModel } from "../../Models/Base";
import { jobService, type JobModel } from "../../Services/JobService";
import { baseService } from "../../Services/BaseService";
import type { User } from "../../Models/user";
import { storageItemService } from "../../Services/StorageItemService";
import "./StorageItemModal.css";
import classNames from "classnames";
import { format } from "date-fns";

interface Props {
  itemId: string;
  storageItems: StorageItemModel[];
  user: User;
  onClose: () => void;
}

export interface CreateStorageItemRequest {
  note: string;
  scheduledStart: string;
  scheduledEnd: string;
  itemId: string;
  companyId: string;
}

interface FormErrors {
  note?: string;
  scheduledStart?: string;
  scheduledEnd?: string;
}

export const StorageItemModal: FunctionComponent<Props> = ({
  itemId,
  storageItems,
  user,
  onClose,
}) => {
  const [bases, setBases] = useState<BaseModel[]>([]);
  const [jobs, setJobs] = useState<JobModel[]>([]);
  const [showForm, setShowForm] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [formData, setFormData] = useState<CreateStorageItemRequest>({
    note: "",
    scheduledStart: "",
    scheduledEnd: "",
    itemId: itemId,
    companyId: user.companyId || "",
  });
  const [errors, setErrors] = useState<FormErrors>({});

  useEffect(() => {
    loadBases();
    loadJobs();
  }, []);

  const loadBases = async () => {
    const baseModels = await baseService.getBasesByCompany();
    setBases(baseModels);
  };

  const loadJobs = async () => {
    const jobModels = await jobService.getJobsByCompany();
    setJobs(jobModels);
  };

  const getStorageName = (storageId: string) => {
    const jobTitle = jobs.find((job) => job.data.storage.storageId == storageId)
      ?.data.title;
    if (jobTitle) return jobTitle;

    const baseName = bases.find(
      (base) => base.data.storage.storageId == storageId
    )?.data.name;
    if (baseName) return baseName;
  };

  const validateForm = (): boolean => {
    const newErrors: FormErrors = {};

    if (!formData.note.trim()) {
      newErrors.note = "Note er påkrævet";
    }

    if (!formData.scheduledStart) {
      newErrors.scheduledStart = "Planlagt start er påkrævet";
    }

    if (!formData.scheduledEnd) {
      newErrors.scheduledEnd = "Planlagt slut er påkrævet";
    }

    if (formData.scheduledStart && formData.scheduledEnd) {
      const startDate = new Date(formData.scheduledStart);
      const endDate = new Date(formData.scheduledEnd);

      if (startDate >= endDate) {
        newErrors.scheduledEnd = "Slutdato skal være efter startdato";
      }
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleInputChange = (
    field: keyof CreateStorageItemRequest,
    value: string
  ) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
    if (errors[field as keyof FormErrors]) {
      setErrors((prev) => ({ ...prev, [field]: undefined }));
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!validateForm()) {
      return;
    }

    setIsSubmitting(true);
    try {
      const newStorageItem = await storageItemService.createStorageItem(
        formData
      );
      storageItems.push(newStorageItem);

      setFormData({
        note: "",
        scheduledStart: "",
        scheduledEnd: "",
        itemId: itemId,
        companyId: user.companyId || "",
      });
      setShowForm(false);
    } catch (error) {
      console.error("Error creating storage item:", error);
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleCancelForm = () => {
    setShowForm(false);
    setFormData({
      note: "",
      scheduledStart: "",
      scheduledEnd: "",
      itemId: itemId,
      companyId: user.companyId || "",
    });
    setErrors({});
  };

  const deleteStorageItem = async (storageItemId: string) => {
    if (confirm("Er du sikker på, at du vil slette denne allokering?")) {
        await storageItemService.deleteStorageItem(storageItemId);
        storageItems = storageItems.filter(storageItem => storageItem.data.storageItemId !== storageItemId)
    }
  }

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
            <h5 className="modal-title text-dark">Allokeringer</h5>
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
            {!showForm && (
              <div className="mb-3">
                <button
                  type="button"
                  className="btn btn-primary"
                  onClick={() => setShowForm(true)}
                >
                  Tilføj allokering
                </button>
              </div>
            )}

            {showForm && (
              <div
                className={classNames(
                  "form-container",
                  showForm ? "expanded" : "collapsed"
                )}
              >
                <div
                  className={classNames(
                    "card mb-3 form-card",
                    showForm ? "show" : "hide"
                  )}
                >
                  <div className="card-header d-flex justify-content-between align-items-center">
                    <h6 className="mb-0">Ny Allokering</h6>
                  </div>
                  <div className="card-body">
                    <form onSubmit={handleSubmit}>
                      <div className="mb-3">
                        <label htmlFor="note" className="form-label">
                          Note <span className="text-danger">*</span>
                        </label>
                        <textarea
                          id="note"
                          className={`form-control ${
                            errors.note ? "is-invalid" : ""
                          }`}
                          rows={3}
                          value={formData.note}
                          onChange={(e) =>
                            handleInputChange("note", e.target.value)
                          }
                          placeholder="Indtast note..."
                        />
                        {errors.note && (
                          <div className="invalid-feedback">{errors.note}</div>
                        )}
                      </div>

                      <div className="mb-3">
                        <label htmlFor="scheduledStart" className="form-label">
                          Planlagt Start <span className="text-danger">*</span>
                        </label>
                        <input
                          type="date"
                          id="scheduledStart"
                          className={`form-control ${
                            errors.scheduledStart ? "is-invalid" : ""
                          }`}
                          value={formData.scheduledStart}
                          onChange={(e) =>
                            handleInputChange("scheduledStart", e.target.value)
                          }
                        />
                        {errors.scheduledStart && (
                          <div className="invalid-feedback">
                            {errors.scheduledStart}
                          </div>
                        )}
                      </div>

                      <div className="mb-3">
                        <label htmlFor="scheduledEnd" className="form-label">
                          Planlagt Slut <span className="text-danger">*</span>
                        </label>
                        <input
                          type="date"
                          id="scheduledEnd"
                          className={`form-control ${
                            errors.scheduledEnd ? "is-invalid" : ""
                          }`}
                          value={formData.scheduledEnd}
                          onChange={(e) =>
                            handleInputChange("scheduledEnd", e.target.value)
                          }
                        />
                        {errors.scheduledEnd && (
                          <div className="invalid-feedback">
                            {errors.scheduledEnd}
                          </div>
                        )}
                      </div>

                      <div className="d-flex gap-2">
                        <button
                          type="submit"
                          className="btn btn-success"
                          disabled={isSubmitting}
                        >
                          {isSubmitting ? (
                            <>
                              <span
                                className="spinner-border spinner-border-sm me-2"
                                role="status"
                                aria-hidden="true"
                              ></span>
                              Gemmer...
                            </>
                          ) : (
                            "Gem Allokering"
                          )}
                        </button>
                        <button
                          type="button"
                          className="btn btn-secondary"
                          onClick={handleCancelForm}
                          disabled={isSubmitting}
                        >
                          Annuller
                        </button>
                      </div>
                    </form>
                  </div>
                </div>
              </div>
            )}

            <hr className="mx-auto my-3 w-100"></hr>

            {storageItems.map((storageItem) => {
              return (
                <div key={storageItem.data.storageItemId} className="card">
                  <div className="card-header">
                    <div className="card-title">
                        <div className="d-flex justify-content-center align-items-center">

                      <h5 className="text-sm font-semibold text-gray-800">
                        {getStorageName(storageItem.data.storage.storageId)}
                      </h5>
                      <div className="ms-auto" role="button" onClick={() => deleteStorageItem(storageItem.data.storageItemId)}>

                      <i className="bi bi-trash-fill"></i>
                      </div>
                        </div>
                    </div>
                  </div>
                  <div className="card-body">
                    <div className="card-text">
                      <p>
                        📅{" "}
                        {format(
                          new Date(storageItem.data.scheduledStart),
                          "dd-MM-yyyy"
                        )}{" "}
                        –{" "}
                        {format(
                          new Date(storageItem.data.scheduledEnd),
                          "dd-MM-yyyy"
                        )}
                      </p>

                      {storageItem.data.note && (
                        <p className="mt-2 text-xs text-gray-600 leading-snug">
                          <span className="font-medium">📒 Note:</span>{" "}
                          {storageItem.data.note}
                        </p>
                      )}
                    </div>
                  </div>
                </div>
              );
            })}
          </div>
        </div>
      </div>
    </div>
  );
};
