import { useEffect, useState, useMemo } from "react";
import { VehicleModel } from "../../Models/Vehicle";
import {
  HttpError,
  vehicleService,
} from "../../Services/VehicleService";
import VehicleModal, { type VehicleFormData } from './VehicleModal';
import VehicleInventoryModal from './VehicleInventoryModal';
import { useAuth } from "../../Auth/AuthContext";

type SortField = 'model' | 'licensePlate' | 'availability' | 'currentAssignment' | 'defaultStorage';
type SortDirection = 'asc' | 'desc';

export default function VehicleComponent() {
  const { user } = useAuth();
  const [vehicles, setVehicles] = useState<VehicleModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [inventoryModalOpen, setInventoryModalOpen] = useState(false);
  const [selectedVehicle, setSelectedVehicle] = useState<VehicleModel | null>(null);
  const [selectedInventoryVehicle, setSelectedInventoryVehicle] = useState<VehicleModel | null>(null);

  const [searchTerm, setSearchTerm] = useState('');
  const [sortField, setSortField] = useState<SortField>('model');
  const [sortDirection, setSortDirection] = useState<SortDirection>('asc');

  useEffect(() => {
    loadVehicles();
  }, []);

  const filteredAndSortedVehicles = useMemo(() => {
    let filtered = vehicles;

    if (searchTerm.trim()) {
      const searchLower = searchTerm.toLowerCase();
      filtered = vehicles.filter(item => {
        return (
          item.model.toLowerCase().includes(searchLower) ||
          item.licensePlate?.toLowerCase().includes(searchLower) ||
          item.data.note?.toLowerCase().includes(searchLower) ||
          item.data.defaultStorage?.name.toLowerCase().includes(searchLower) ||
          item.currentAssignment?.note.toLowerCase().includes(searchLower) ||
          item.nextAssignment?.note.toLowerCase().includes(searchLower) ||
          item.employees.some(emp => 
            `${emp.firstName} ${emp.lastName}`.toLowerCase().includes(searchLower)
          )
        );
      });
    }

    const sorted = [...filtered].sort((a, b) => {
      let aValue: string | number;
      let bValue: string | number;

      switch (sortField) {
        case 'model':
          aValue = a.model.toLowerCase();
          bValue = b.model.toLowerCase();
          break;
        case 'licensePlate':
          aValue = a.licensePlate?.toLowerCase() || '';
          bValue = b.licensePlate?.toLowerCase() || '';
          break;
        case 'defaultStorage':
          aValue = a.data.defaultStorage?.name.toLowerCase() || '';
          bValue = b.data.defaultStorage?.name.toLowerCase() || '';
          break;
        case 'availability':
          aValue = a.isAvailable ? 1 : 0;
          bValue = b.isAvailable ? 1 : 0;
          break;
        case 'currentAssignment':
          aValue = a.currentAssignment ? 1 : 0;
          bValue = b.currentAssignment ? 1 : 0;
          break;
        default:
          aValue = a.model.toLowerCase();
          bValue = b.model.toLowerCase();
      }

      if (sortDirection === 'asc') {
        return aValue > bValue ? 1 : aValue < bValue ? -1 : 0;
      } 
      else {
        return aValue < bValue ? 1 : aValue > bValue ? -1 : 0;
      }
    });

    return sorted;
  }, [vehicles, searchTerm, sortField, sortDirection]);

  const handleSort = (field: SortField) => {
    if (sortField === field) {
      setSortDirection(sortDirection === 'asc' ? 'desc' : 'asc');
    } else {
      setSortField(field);
      setSortDirection('asc');
    }
  };

  const getSortIcon = (field: SortField) => {
    if (sortField !== field) {
      return <i className="bi bi-arrow-down-up text-muted"></i>;
    }
    return sortDirection === 'asc' 
      ? <i className="bi bi-arrow-up" style={{color: '#fff'}}></i>
      : <i className="bi bi-arrow-down" style={{color: '#fff'}}></i>;
  };

  const loadVehicles = async () => {
    try {
      setLoading(true);
      setError(null);

      const vehicleModels = await vehicleService.getAllVehicles();
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

  const openCreateModal = () => {
    setSelectedVehicle(null);
    setModalOpen(true);
  };

  const openEditModal = (vehicle: VehicleModel) => {
    setSelectedVehicle(vehicle);
    setModalOpen(true);
  };

  const closeModal = () => {
    setModalOpen(false);
    setSelectedVehicle(null);
  };

  const openInventoryModal = (vehicle: VehicleModel) => {
    setSelectedInventoryVehicle(vehicle);
    setInventoryModalOpen(true);
  };

  const closeInventoryModal = () => {
    setInventoryModalOpen(false);
    setSelectedInventoryVehicle(null);
  };

  const handleSaveVehicle = async (formData: VehicleFormData) => {
    if (selectedVehicle) {
      const updatedVehicle = await vehicleService.updateVehicle(selectedVehicle.data.itemId, formData);
      setVehicles(vehicles.map(vehicle =>
        vehicle.data.itemId === updatedVehicle.data.itemId ? updatedVehicle : vehicle
      ));
    } else {
      await vehicleService.createVehicle(formData, user!.companyId);
      await loadVehicles();
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
      <div className="container mt-4">
        <div className="d-flex justify-content-center">
          <div className="spinner-border" role="status">
            <span className="visually-hidden">Indlæser data...</span>
          </div>
          <span className="ms-2">Indlæser køretøjer...</span>
        </div>
      </div>
    );
  }

  return (
    <div className="position-relative">
      <div className="container mt-4">
        <div className="d-flex justify-content-between align-items-center mb-4">
          <h1>Køretøjer</h1>
          <button className="btn btn-primary" onClick={openCreateModal}>
            <i className="bi bi-plus"></i> Tilføj køretøj
          </button>
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

        {!error && (
          <>
            <div className="row mb-4">
              <div className="col-md-8">
                <div className="input-group">
                  <span className="input-group-text">
                    <i className="bi bi-search"></i>
                  </span>
                  <input
                    type="text"
                    className="form-control"
                    placeholder="Søg efter model, nummerplade, medarbejdere, lager..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                  />
                  {searchTerm && (
                    <button
                      className="btn btn-outline-secondary"
                      type="button"
                      onClick={() => setSearchTerm('')}
                    >
                      <i className="bi bi-x"></i>
                    </button>
                  )}
                </div>
              </div>
              <div className="col-md-4">
                <div className="btn-group w-100" role="group">
                  <button
                    type="button"
                    className={`btn ${sortField === 'model' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => handleSort('model')}
                  >
                    Model {getSortIcon('model')}
                  </button>
                  <button
                    type="button"
                    className={`btn ${sortField === 'availability' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => handleSort('availability')}
                  >
                    Status {getSortIcon('availability')}
                  </button>
                  <button
                    type="button"
                    className={`btn ${sortField === 'licensePlate' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => handleSort('licensePlate')}
                  >
                    Nummerplade {getSortIcon('licensePlate')}
                  </button>
                </div>
              </div>
            </div>

            <div className="mb-3">
              <small className="text-muted">
                Viser {filteredAndSortedVehicles.length} af {vehicles.length} køretøjer
                {searchTerm && (
                  <span> (søgning: "{searchTerm}")</span>
                )}
              </small>
            </div>

            {filteredAndSortedVehicles.length === 0 ? (
              <div className="alert alert-info">
                {searchTerm ? 
                  `Ingen køretøjer matchede søgningen "${searchTerm}".` : 
                  'Ingen køretøjer fundet.'
                }
              </div>
            ) : (
              <div className="row">
                {filteredAndSortedVehicles.map((vehicleModel) => {
                  const currentAssignment = vehicleModel.currentAssignment;
                  const nextAssignment = vehicleModel.nextAssignment;

                  const isLedig = !currentAssignment;

                  return (
                    <div
                      key={vehicleModel.data.itemId}
                      className="col-md-6 col-lg-6 mb-3 d-flex"
                    >
                      <div className="card w-100">
                        <div className="card-header bg-dark">
                          <h5 className="card-title d-flex justify-content-between align-items-center">
                            <strong className="text-app-primary">
                              {vehicleModel.model}
                            </strong>
                            {isLedig && (
                              <span className="badge bg-success">Ledig</span>
                            )}
                          </h5>
                        </div>
                        <div className="card-body">
                          <div className="card-text">
                            {vehicleModel.licensePlate ? (
                              <p><strong>Nummerplade:</strong> {vehicleModel.licensePlate}</p>
                            ) : (
                              <p className="text-muted fst-italic">
                                Ingen nummerplade registreret
                              </p>
                            )}
                            <p><strong>Tilknyttet:</strong> {vehicleModel.data.defaultStorage.name}</p>
                            {vehicleModel.data.note && (
                              <>
                                <p><strong>Note:</strong> {vehicleModel.data.note}</p>
                              </>
                            )}
                            {currentAssignment ? (
                              <>
                                <strong>Nuværende opgave:</strong>
                                <div className="bg-light p-2 rounded small">
                                  <small className="text-muted">
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
                            {vehicleModel.employees.length > 0 ? (
                              <>
                                <p><strong>Medarbejdere:</strong></p>
                                <div className="mb-2">
                                  {vehicleModel.employees.map((employee) => (
                                    <span key={employee.itemId} className="badge bg-secondary me-1 mb-1">
                                      {employee.firstName} {employee.lastName}
                                    </span>
                                  ))}
                                </div>
                              </>
                            ) : (
                              <>
                                <p className="text-muted fst-italic">
                                  Ingen medarbejdere allokeret
                                </p>
                              </>
                            )}
                            {vehicleModel.tools.length > 0 ? (
                              <>
                                <button
                                  className="btn btn-primary btn-sm"
                                  onClick={() => openInventoryModal(vehicleModel)}
                                >
                                  <i className="bi bi-eye me-1"></i>
                                  Se indhold ({vehicleModel.tools.length})
                                </button>
                                <br />
                              </>
                            ) : (
                              <>
                                <p className="text-muted fst-italic">
                                  Intet værktøj allokeret
                                </p>
                              </>
                            )}
                          </div>
                        </div>
                        <div className="card-footer d-flex justify-content-between">
                          <button
                            className="btn btn-sm btn-outline-primary me-2"
                            onClick={() => openEditModal(vehicleModel)}
                          >
                            <i className="bi bi-pencil"></i> Rediger
                          </button>
                          <button
                            className="btn btn-sm btn-outline-danger"
                            onClick={() => handleDeleteVehicle(vehicleModel)}
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
          </>
        )}

        {modalOpen && (
          <VehicleModal
            isOpen={modalOpen}
            onClose={closeModal}
            onSave={handleSaveVehicle}
            vehicle={selectedVehicle}
            title={selectedVehicle ? 'Rediger køretøj' : 'Tilføj nyt køretøj'}
          />
        )}

        {inventoryModalOpen && (
          <VehicleInventoryModal
            isOpen={inventoryModalOpen}
            onClose={closeInventoryModal}
            vehicle={selectedInventoryVehicle}
          />
        )}
      </div>
    </div>
  );
}