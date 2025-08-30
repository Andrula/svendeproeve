import { useState, useEffect } from 'react';
import { employeeService, EmployeeModel, HttpError, DEFAULT_COMPANY_ID } from '../../Services/employeeService';

export default function EmployeeComponent() {
  const [employees, setEmployees] = useState<EmployeeModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadEmployees();
  }, []);

  const loadEmployees = async () => {
    try {
      setLoading(true);
      setError(null);
      
      if (!DEFAULT_COMPANY_ID) {
        throw new Error('Company ID ikke konfigureret. Kontakt administrator.');
      }

      const employeeModels = await employeeService.getAllEmployees(DEFAULT_COMPANY_ID);
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

  const handleDeleteEmployee = async (employeeModel: EmployeeModel) => {
    if (window.confirm(`Er du sikker på, at du vil slette ${employeeModel.fullName}?`)) {
      try {
        await employeeService.deleteEmployee(employeeModel.data.itemId);
        setEmployees(employees.filter(emp => emp.data.itemId !== employeeModel.data.itemId));
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
    <div className="container mt-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>Medarbejdere</h1>
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

      {!error && employees.length === 0 ? (
        <div className="alert alert-info">
          Ingen medarbejdere fundet.
        </div>
      ) : (
        <div className="row">
          {employees.map((employeeModel) => {
            const currentAssignment = employeeModel.currentAssignment;
            
            return (
              <div key={employeeModel.data.itemId} className="col-md-6 col-lg-4 mb-3">
                <div className="card">
                  <div className="card-body">
                    <h5 className="card-title">
                      {employeeModel.fullName}
                      {employeeModel.isAvailable && (
                        <span className="badge bg-success ms-2">Ledig</span>
                      )}
                    </h5>
                    <p className="card-text">
                      <strong>Stilling:</strong> {employeeModel.data.occupation.name}<br />
                      <strong>Email:</strong> {employeeModel.data.email}<br />
                      <strong>Telefon:</strong> {employeeModel.formatPhoneNumber()}<br />
                      <strong>Standard lager:</strong> {employeeModel.data.defaultStorage.name}<br />
                      {employeeModel.data.note && (
                        <>
                          <strong>Note:</strong> {employeeModel.data.note}<br />
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
                      <button className="btn btn-sm btn-outline-primary">
                        <i className="bi bi-pencil"></i> Rediger
                      </button>
                      <button 
                        className="btn btn-sm btn-outline-danger"
                        onClick={() => handleDeleteEmployee(employeeModel)}
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

      <div className="mt-3">
        <small className="text-muted">
          Viser {employees.length} medarbejdere
        </small>
      </div>
    </div>
  );
}