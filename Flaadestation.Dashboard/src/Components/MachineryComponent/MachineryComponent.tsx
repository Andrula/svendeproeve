import { useState, useEffect } from 'react';
import { MachineryModel } from '../../Models/Machinery'; 
import { 
  machineryService, 
  HttpError, 
  DEFAULT_COMPANY_ID 
} from '../../Services/machineryService'; 
import MachineryModal, { type MachineryFormData } from './MachineryModal';

export default function MachineryComponent() {
  const [machinery, setMachinery] = useState<MachineryModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [selectedMachinery, setSelectedMachinery] = useState<MachineryModel | null>(null);

  useEffect(() => {
    loadMachinery();
  }, []);

  const loadMachinery = async () => {
    try {
      setLoading(true);
      setError(null);

      if (!DEFAULT_COMPANY_ID) {
        throw new Error('Company ID ikke konfigureret. Kontakt administrator.');
      }

      const machineryModels = await machineryService.getAllMachinery(DEFAULT_COMPANY_ID);
      setMachinery(machineryModels);
    } catch (err) {
      let errorMessage = 'Ukendt fejl';

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

      setError(`Fejl ved indlæsning af maskiner: ${errorMessage}`);
      console.error('Fejl ved indlæsning af maskiner:', err);
    } finally {
      setLoading(false);
    }
  };

  const openCreateModal = () => {
    setSelectedMachinery(null);
    setModalOpen(true);
  };

  const openEditModal = (machineryItem: MachineryModel) => {
    setSelectedMachinery(machineryItem);
    setModalOpen(true);
  };

  const closeModal = () => {
    setModalOpen(false);
    setSelectedMachinery(null);
  };

  const handleSaveMachinery = async (formData: MachineryFormData) => {
    if (selectedMachinery) {
      const updatedMachinery = await machineryService.updateMachinery(selectedMachinery.data.itemId, formData);
      setMachinery(machinery.map(item =>
        item.data.itemId === updatedMachinery.data.itemId ? updatedMachinery : item
      ));
    } else {
      const newMachinery = await machineryService.createMachinery(formData, DEFAULT_COMPANY_ID);
      setMachinery([...machinery, newMachinery]);
    }
  };

  const handleDeleteMachinery = async (machineryItem: MachineryModel) => {
    if (window.confirm(`Er du sikker på, at du vil slette ${machineryItem.name}?`)) {
      try {
        await machineryService.deleteMachinery(machineryItem.data.itemId);
        setMachinery(machinery.filter(item => item.data.itemId !== machineryItem.data.itemId));
      } catch (err) {
        console.error('Error deleting machinery:', err);
      }
    }
  };

  if (loading) {
    return (
      <div className="container mt-4">
        <div className="d-flex justify-content-center">
          <div className="spinner-border" role="status">
            <span className="visually-hidden">Indlæser data...</span>
          </div>
          <span className="ms-2">Indlæser maskiner...</span>
        </div>
      </div>
    );
  }

  return (
    <div className="position-relative" style={{ minHeight: '100vh' }}>
      <div className="container mt-4">
        <div className="d-flex justify-content-between align-items-center mb-4">
          <h1>Maskiner</h1>
          <button className="btn btn-primary" onClick={openCreateModal}>
            <i className="bi bi-plus"></i> Tilføj maskine
          </button>
        </div>

        {error && (
          <div className="alert alert-danger" role="alert">
            {error}
            <button 
              className="btn btn-link p-0 ms-2" 
              onClick={loadMachinery}
            >
              Prøv igen
            </button>
          </div>
        )}

        {!error && machinery.length === 0 ? (
          <div className="alert alert-info">
            Ingen maskiner fundet.
          </div>
        ) : (
          <div className="row">
            {machinery.map((machineryItem) => {
              const currentAssignment = machineryItem.currentAssignment;
              const nextAssignment = machineryItem.nextAssignment;

              return (
                <div
                  key={machineryItem.data.itemId}
                  className="col-md-6 col-lg-6 mb-3 d-flex"
                >
                  <div className="card w-100">
                    <div className="card-header bg-dark">
                      <h5 className="card-title d-flex justify-content-between align-items-center">
                        <strong className="text-app-primary">
                          {machineryItem.name}
                        </strong>
                        {machineryItem.isAvailable && (
                          <span className="badge bg-success">Ledig</span>
                        )}
                      </h5>
                    </div>
                    <div className="card-body">
                      <div className="card-text">
                        {machineryItem.data.defaultStorage ? (
                          <p><strong>Standard lager:</strong> {machineryItem.data.defaultStorage.name}</p>
                        ) : (
                          <p className="text-muted fst-italic">ingen standard lager registreret</p>
                        )}
                        {machineryItem.note ? (
                          <>
                            <p><strong>Note:</strong> {machineryItem.note}</p>
                          </>
                        ) : (
                          <>
                            <p className="text-muted fst-italic">
                              ingen note registreret
                            </p>
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
                        ) : nextAssignment ? (
                          <>
                            <strong>Næste opgave:</strong>
                            <div className="bg-light p-2 rounded small">
                              <small className="text-muted">
                                {nextAssignment.note}
                                <br />
                                {new Date(
                                    nextAssignment.scheduledStart
                                ).toLocaleDateString("da-DK")}{" "}
                                -{" "}
                                {new Date(
                                    nextAssignment.scheduledEnd
                                ).toLocaleDateString("da-DK")}
                              </small>
                            </div>
                          </>
                        ) : (
                          <>
                            <p className="text-muted fst-italic">
                              Ikke allokeret til nogle opgaver
                            </p>
                          </>
                        )}
                      </div>
                    </div>
                    <div className="card-footer d-flex justify-content-between">
                      <button 
                        className="btn btn-sm btn-outline-primary me-2"
                        onClick={() => openEditModal(machineryItem)}
                      >
                        <i className="bi bi-pencil"></i> Rediger
                      </button>
                      <button 
                        className="btn btn-sm btn-outline-danger"
                        onClick={() => handleDeleteMachinery(machineryItem)}
                      >
                        <i className="bi bi-trash"></i> Slet
                      </button>
                    </div>
                  </div>
                </div>
              );
            })}
          </div>
        )}

        {modalOpen && (
          <MachineryModal
            isOpen={modalOpen}
            onClose={closeModal}
            onSave={handleSaveMachinery}
            machinery={selectedMachinery}
            title={selectedMachinery ? 'Rediger maskine' : 'Tilføj ny maskine'}
          />
        )}

        <div className="mt-3">
          <small className="text-muted">
            Viser {machinery.length} maskiner
          </small>
        </div>
      </div>
    </div>
  );
}