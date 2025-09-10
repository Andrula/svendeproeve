import { useState, useEffect, useMemo } from 'react';
import { employeeService, EmployeeModel, HttpError } from '../../Services/EmployeeService';
import EmployeeModal, { type EmployeeFormData } from '../EmployeeComponent/EmployeeModal';
import { useAuth } from '../../Auth/AuthContext';

type SortField = 'name' | 'defaultStorage' | 'availability' | 'currentAssignment' | 'phone' | 'occupation' | 'email';
type SortDirection = 'asc' | 'desc';

export default function EmployeeComponent() {
  const { user } = useAuth();
  const [employees, setEmployees] = useState<EmployeeModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [selectedEmployee, setSelectedEmployee] = useState<EmployeeModel | null>(null);

  const [searchTerm, setSearchTerm] = useState('');
  const [sortField, setSortField] = useState<SortField>('name');
  const [sortDirection, setSortDirection] = useState<SortDirection>('asc');
  const [selectedOccupation, setSelectedOccupation] = useState<string>('');

  useEffect(() => {
    loadEmployees();
  }, []);

  const uniqueOccupations = useMemo(() => {
    const occupations = employees.map(emp => emp.employee.occupation.name);
    return [...new Set(occupations)].sort();
  }, [employees]);

  const filteredAndSortedEmployees = useMemo(() => {
    let filtered = employees;

    if (searchTerm.trim()) {
      const searchLower = searchTerm.toLowerCase();
      filtered = filtered.filter(item => {
        return (
          item.fullName.toLowerCase().includes(searchLower) ||
          item.employee.email.toLowerCase().includes(searchLower) ||
          item.employee.phone.toLowerCase().includes(searchLower) ||
          item.employee.occupation.name.toLowerCase().includes(searchLower) ||
          item.data.defaultStorage?.name.toLowerCase().includes(searchLower) ||
          item.currentAssignment?.note.toLowerCase().includes(searchLower) ||
          item.nextAssignment?.note.toLowerCase().includes(searchLower)
        );
      });
    }

    if (selectedOccupation) {
      filtered = filtered.filter(item => 
        item.employee.occupation.name === selectedOccupation
      );
    }

    const sorted = [...filtered].sort((a, b) => {
      let aValue: string | number;
      let bValue: string | number;

      switch (sortField) {
        case 'name':
          aValue = a.fullName.toLowerCase();
          bValue = b.fullName.toLowerCase();
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
        case 'email':
          aValue = a.employee.email ? 1 : 0;
          bValue = b.employee.email ? 1 : 0;
          break;
        case 'phone':
          aValue = a.employee.phone ? 1 : 0;
          bValue = b.employee.phone ? 1 : 0;
          break;
        case 'occupation':
          aValue = a.employee.occupation.name ? 1 : 0;
          bValue = b.employee.occupation.name ? 1 : 0;
          break;
        default:
          aValue = a.fullName.toLowerCase();
          bValue = b.fullName.toLowerCase();
      }

      if (sortDirection == 'asc') {
        return aValue > bValue ? 1 : aValue < bValue ? -1 : 0;
      }
      else {
        return aValue < bValue ? 1 : aValue > bValue ? -1 : 0;
      }
    });

    return sorted;
  }, [employees, searchTerm, sortField, sortDirection, selectedOccupation]);

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
      ? <i className="bi bi-arrow-up" style={{ color: '#fff' }}></i>
      : <i className="bi bi-arrow-down" style={{ color: '#fff' }}></i>;
  };

  const loadEmployees = async () => {
    try {
      setLoading(true);
      setError(null);

      const employeeModels = await employeeService.getAllEmployees();
      setEmployees(employeeModels);
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

      setError(`Fejl ved indlæsning af medarbejdere: ${errorMessage}`);
      console.error('Fejl ved indlæsning af medarbejdere:', err);
    } finally {
      setLoading(false);
    }
  };

  const openCreateModal = () => {
    setSelectedEmployee(null);
    setModalOpen(true);
  };

  const openEditModal = (employee: EmployeeModel) => {
    setSelectedEmployee(employee);
    setModalOpen(true);
  };

  const closeModal = () => {
    setModalOpen(false);
    setSelectedEmployee(null);
  };

  const handleSaveEmployee = async (formData: EmployeeFormData) => {
    if (selectedEmployee) {
      const updatedEmployee = await employeeService.updateEmployee(selectedEmployee.data.itemId, formData);
      setEmployees(employees.map(emp =>
        emp.data.itemId === updatedEmployee.data.itemId ? updatedEmployee : emp
      ));
    } else {
      await employeeService.createEmployee(formData, user!.companyId);
      await loadEmployees();
    }
  };

  const handleDeleteEmployee = async (employee: EmployeeModel) => {
    if (window.confirm(`Er du sikker på, at du vil slette ${employee.fullName}?`)) {
      try {
        await employeeService.deleteEmployee(employee.data.itemId);
        setEmployees(employees.filter(emp => emp.data.itemId !== employee.data.itemId));
      } catch (err) {
        console.error('Error deleting employee:', err);
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
          <span className="ms-2">Indlæser medarbejdere...</span>
        </div>
      </div>
    );
  }

  return (
    <div className="position-relative">
      <div className="container mt-4">
        <div className="d-flex justify-content-between align-items-center mb-4">
          <h1>Medarbejdere</h1>
          <button className="btn btn-primary" onClick={openCreateModal}>
            <i className="bi bi-plus"></i> Tilføj medarbejder
          </button>
        </div>

        {error && (
          <div className="alert alert-danger" role="alert">
            {error}
            <button
              className="btn btn-link p-0 ms-2"
              onClick={loadEmployees}
            >
              Prøv igen
            </button>
          </div>
        )}

        {!error && (
          <>
            <div className="row mb-4">
              <div className="col-md-6">
                <div className="input-group">
                  <span className="input-group-text">
                    <i className="bi bi-search"></i>
                  </span>
                  <input
                    type="text"
                    className="form-control"
                    placeholder="Søg efter navn, email, telefon, stilling, lager..."
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
              <div className="col-md-3">
                <select
                  className="form-select"
                  value={selectedOccupation}
                  onChange={(e) => setSelectedOccupation(e.target.value)}
                >
                  <option value="">Alle stillinger</option>
                  {uniqueOccupations.map(occupation => (
                    <option key={occupation} value={occupation}>
                      {occupation}
                    </option>
                  ))}
                </select>
              </div>
              <div className="col-md-3">
                <div className="btn-group w-100" role="group">
                  <button
                    type="button"
                    className={`btn ${sortField === 'name' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => handleSort('name')}
                  >
                    Navn {getSortIcon('name')}
                  </button>
                  <button
                    type="button"
                    className={`btn ${sortField === 'availability' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => handleSort('availability')}
                  >
                    Status {getSortIcon('availability')}
                  </button>
                </div>
              </div>
            </div>

            <div className="mb-3">
              <small className="text-muted">
                Viser {filteredAndSortedEmployees.length} af {employees.length} medarbejdere
                {searchTerm && (
                  <span> (søgning: "{searchTerm}")</span>
                )}
                {selectedOccupation && (
                  <span> (stilling: "{selectedOccupation}")</span>
                )}
              </small>
            </div>

            {filteredAndSortedEmployees.length === 0 ? (
              <div className="alert alert-info">
                {searchTerm ? 
                  `Ingen medarbejdere matchede søgningen "${searchTerm}".` : 
                  'Ingen medarbejdere fundet.'
                }
              </div>
            ) : (
              <div className="row">
                {filteredAndSortedEmployees.map((employee) => {
                  const currentAssignment = employee.currentAssignment;
                  const nextAssignment = employee.nextAssignment;

                  const isLedig = !currentAssignment;

                  return (
                    <div
                      key={employee.data.itemId}
                      className="col-md-6 col-lg-6 mb-3 d-flex"
                    >
                      <div className="card w-100">
                        <div className="card-header bg-dark">
                          <h5 className="card-title d-flex justify-content-between align-items-center">
                            <strong className="text-app-primary">
                              {employee.fullName}
                            </strong>
                            {isLedig && (
                              <span className="badge bg-success">Ledig</span>
                            )}
                          </h5>
                        </div>
                        <div className="card-body">
                          <div className="card-text">
                            <p><strong>Stilling:</strong> {employee.data.occupation.name}</p>
                            <p><strong>Email:</strong> {employee.data.email}</p>
                            <p><strong>Telefon:</strong> {employee.formatPhoneNumber()}</p>
                            <p><strong>Tilknyttet:</strong> {employee.data.vehicle ? (<>{employee.data.vehicle.model}</>) : (<>{employee.data.defaultStorage?.name}</>)}</p>
                            {employee.data.note && (
                              <>
                                <p><strong>Note:</strong> {employee.data.note}</p>
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
                            onClick={() => openEditModal(employee)}
                          >
                            <i className="bi bi-pencil"></i> Rediger
                          </button>
                          <button
                            className="btn btn-sm btn-outline-danger"
                            onClick={() => handleDeleteEmployee(employee)}
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
          <EmployeeModal
            isOpen={modalOpen}
            onClose={closeModal}
            onSave={handleSaveEmployee}
            employee={selectedEmployee}
            title={selectedEmployee ? 'Rediger medarbejder' : 'Tilføj ny medarbejder'}
          />
        )}
      </div>
    </div>
  );
}