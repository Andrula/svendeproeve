import { useState, useEffect } from 'react';
import { employeeService, EmployeeModel, HttpError, DEFAULT_COMPANY_ID } from '../../Services/EmployeeService';
import EmployeeModal, { type EmployeeFormData } from './EmployeeModal';
        
export default function EmployeeComponent() {
  const [employees, setEmployees] = useState<EmployeeModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [selectedEmployee, setSelectedEmployee] = useState<EmployeeModel | null>(null);

  useEffect(() => {
    loadEmployees();
  }, []);

  const loadEmployees = async () => {
    try {
      setLoading(true);
      setError(null);

      if (!DEFAULT_COMPANY_ID) {
        throw new Error('Company ID ikke konfigureret.');
      }

      const employeeModels = await employeeService.getAllEmployees(DEFAULT_COMPANY_ID);
      setEmployees(employeeModels);
    } catch (err) {
      let errorMessage = 'Ukendt fejl';

      if (err instanceof HttpError) {
        errorMessage = `Fejl ${err.status}: ${err.message}`;
      } else if (err instanceof Error) {
        errorMessage = err.message;
      }

      setError(`Fejl ved indlæsning af medarbejdere: ${errorMessage}`);
      console.error('Error loading employees:', err);
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
      const newEmployee = await employeeService.createEmployee(formData, DEFAULT_COMPANY_ID);
      setEmployees([...employees, newEmployee]);
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
            <span className="visually-hidden">Loading...</span>
          </div>
          <span className="ms-2">Indlæser medarbejdere...</span>
        </div>
      </div>
    );
  }

  return (
    <div className="position-relative" style={{ minHeight: '100vh', padding: '1.5rem' }}>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>Medarbejdere</h1>
        <button className="btn btn-primary" onClick={openCreateModal}>
          <i className="bi bi-plus"></i> Tilføj medarbejder
        </button>
      </div>

      {error && (
        <div className="alert alert-danger" role="alert">
          {error}
          <button className="btn btn-link p-0 ms-2" onClick={loadEmployees}>
            Prøv igen
          </button>
        </div>
      )}

      {!error && employees.length === 0 ? (
        <div className="alert alert-info">
          Ingen medarbejdere fundet.
        </div>
      ) : (
        <div className="row">
          {employees.map((employee) => {
            const currentAssignment = employee.currentAssignment;

            return (
              <div key={employee.data.itemId} className="col-md-6 col-lg-4 mb-3">
                <div className="card">
                  <div className="card-body">
                    <h5 className="card-title">
                      {employee.fullName}
                      {employee.isAvailable && (
                        <span className="badge bg-success ms-2">Ledig</span>
                      )}
                    </h5>
                    <p className="card-text">
                      <strong>Stilling:</strong> {employee.data.occupation.name}<br />
                      <strong>Email:</strong> {employee.data.email}<br />
                      <strong>Telefon:</strong> {employee.formatPhoneNumber()}<br />
                      <strong>Standard lager:</strong> {employee.data.defaultStorage.name}<br />
                      {employee.data.note && (
                        <>
                          <strong>Note:</strong> {employee.data.note}<br />
                        </>
                      )}
                      {currentAssignment && (
                        <>
                          <strong>Nuværende opgave:</strong><br />
                          <small className="text-muted">
                            {currentAssignment.storage.name}<br />
                            {currentAssignment.note}<br />
                            {new Date(currentAssignment.scheduledStart).toLocaleDateString('da-DK')} - {new Date(currentAssignment.scheduledEnd).toLocaleDateString('da-DK')}
                          </small>
                        </>
                      )}
                    </p>
                    <div className="btn-group" role="group">
                      <button
                        className="btn btn-sm btn-outline-primary"
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
              </div>
            );
          })}
        </div>
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

      <div className="mt-3">
        <small className="text-muted">
          Viser {employees.length} medarbejdere
        </small>
      </div>
    </div>
  );
}