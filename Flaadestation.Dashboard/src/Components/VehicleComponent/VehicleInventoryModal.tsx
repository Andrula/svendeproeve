import { VehicleModel } from '../../Models/Vehicle';

interface VehicleInventoryModalProps {
  isOpen: boolean;
  onClose: () => void;
  vehicle: VehicleModel | null;
}

export default function VehicleInventoryModal({
  isOpen,
  onClose,
  vehicle
}: VehicleInventoryModalProps) {
  if (!isOpen || !vehicle) return null;

  return (
    <div 
      className="position-fixed"
      style={{ 
        top: '55px',
        left: 0,
        right: 0,
        bottom: 0,
        backgroundColor: 'rgba(0,0,0,0.7)',
        zIndex: 1000,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        padding: '20px'
      }}
    >
      <div className="modal-dialog modal-lg" style={{ margin: 0, maxWidth: '95%' }}>
        <div className="modal-content shadow-lg" style={{ backgroundColor: 'white', border: '1px solid #dee2e6' }}>
          <div className="modal-header" style={{ backgroundColor: '#f8f9fa', borderBottom: '1px solid #dee2e6', padding: '16px 24px' }}>
            <h5 className="modal-title text-dark">
              <i className="bi bi-tools me-2"></i>
              Indhold - {vehicle.model}
            </h5>
            <button
              type="button"
              className="btn-close"
              onClick={onClose}
              aria-label="Close"
            ></button>
          </div>

          <div className="modal-body" style={{ maxHeight: '70vh', overflowY: 'auto', backgroundColor: 'white', padding: '24px' }}>
            {vehicle.licensePlate && (
              <div className="mb-3">
                <small className="text-muted">
                  Nummerplade: <strong>{vehicle.licensePlate}</strong>
                </small>
              </div>
            )}

            <div className="mt-4">
              <h6 className="mb-3">
                <i className="bi bi-people me-2"></i>
                Tilknyttede medarbejdere
              </h6>
              {vehicle.employees.length > 0 ? (
                <div className="table-responsive">
                  <table className="table table-striped">
                    <thead>
                      <tr>
                        <th scope="col">Medarbejder</th>
                        <th scope="col">Email</th>
                      </tr>
                    </thead>
                    <tbody>
                      {vehicle.employees.map((employee) => (
                        <tr key={employee.itemId}>
                          <td>
                            {employee.firstName} {employee.lastName}
                          </td>
                          <td>
                            {employee.email ? (
                              <span className="text-muted">{employee.email}</span>
                            ) : (
                              <span className="text-muted fst-italic">Ingen email registreret</span>
                            )}
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              ) : (
                <div className="alert alert-info">
                  <i className="bi bi-info-circle me-2"></i>
                  Dette køretøj har ingen tilknyttede medarbejdere
                </div>
              )}
            </div>

            {vehicle.tools.length > 0 ? (
              <div className="table-responsive">
                <table className="table table-striped">
                  <thead>
                    <tr>
                      <th scope="col">Værktøj</th>
                      <th scope="col">Beskrivelse</th>
                    </tr>
                  </thead>
                  <tbody>
                    {vehicle.tools.map((tool, index) => (
                      <tr key={tool.itemId || index}>
                        <td>
                          {tool.name}
                        </td>
                        <td>
                          {tool.note ? (
                            <span className="text-muted">{tool.note}</span>
                          ) : (
                            <span className="text-muted fst-italic">Ingen beskrivelse</span>
                          )}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            ) : (
              <div className="alert alert-info">
                <i className="bi bi-info-circle me-2"></i>
                Dette køretøj har intet indhold registreret.
              </div>
            )}

            <div className="mt-4">
              <div className="row">
                <div className="col-md-6">
                  <div className="card border-light">
                    <div className="card-body">
                      <h6 className="card-title">
                        <i className="bi bi-geo-alt me-2"></i>
                        Tilknyttet:
                      </h6>
                      <p className="card-text mb-0">
                        {vehicle.data.defaultStorage.name}
                      </p>
                    </div>
                  </div>
                </div>

                <div className="col-md-6">
                  <div className="card border-light">
                    <div className="card-body">
                      <h6 className="card-title">
                        <i className="bi bi-tools me-2"></i>
                        Total antal værktøj
                      </h6>
                      <p className="card-text mb-0">
                        <strong>{vehicle.tools.length}</strong> elementer
                      </p>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            {vehicle.data.note && (
              <div className="mt-3">
                <div className="alert alert-secondary">
                  <h6 className="alert-heading">
                    <i className="bi bi-sticky me-2"></i>
                    Note
                  </h6>
                  <p className="mb-0">{vehicle.data.note}</p>
                </div>
              </div>
            )}
          </div>

          <div className="modal-footer" style={{ backgroundColor: '#f8f9fa', borderTop: '1px solid #dee2e6', padding: '16px 24px' }}>
            <button
              type="button"
              className="btn btn-secondary"
              onClick={onClose}
            >
              <i className="bi bi-x me-2"></i>
              Luk
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}