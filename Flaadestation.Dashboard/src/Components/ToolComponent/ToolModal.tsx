import { useState, useEffect } from 'react';
import { ToolModel } from '../../Models/Tool';
import { vehicleService, VehicleModel } from '../../Services/VehicleService';

export interface ToolFormData {
  name: string;
  note: string;
  vehicleId?: string;
  defaultStorageId: string;
}

interface ToolModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSave: (formData: ToolFormData) => Promise<void>;
  tool: ToolModel | null;
  title: string;
}

export default function ToolModal({ isOpen, onClose, onSave, tool, title }: ToolModalProps) {
  const [formData, setFormData] = useState<ToolFormData>({
    name: '',
    note: '',
    vehicleId: '',
    defaultStorageId: ''
  });
  const [vehicles, setVehicles] = useState<VehicleModel[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (isOpen) {
      loadVehicles();
      if (tool) {
        setFormData({
          name: tool.name,
          note: tool.note,
          vehicleId: tool.data.vehicle?.itemId || '',
          defaultStorageId: tool.data.defaultStorage?.storageId || ''
        });
      } else {
        setFormData({
          name: '',
          note: '',
          vehicleId: '',
          defaultStorageId: ''
        });
      }
    }
  }, [tool, isOpen]);

  const loadVehicles = async () => {
    try {
      const vehicleModels = await vehicleService.getAllVehicles();
      setVehicles(vehicleModels);
    } catch (error) {
      console.error('Error loading vehicles:', error);
    }
  };

  const handleChange = (field: keyof ToolFormData, value: string) => {
    setFormData(prev => ({ ...prev, [field]: value }));
    
    if (field === 'vehicleId' && value) {
      const selectedVehicle = vehicles.find(v => v.data.itemId === value);
      if (selectedVehicle?.data.defaultStorage) {
        setFormData(prev => ({ 
          ...prev, 
          [field]: value,
          defaultStorageId: selectedVehicle.data.defaultStorage.storageId
        }));
      }
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    
    try {
      const submitData = {
        ...formData,
        vehicleId: formData.vehicleId || undefined
      };
      await onSave(submitData);
      onClose();
    } catch (error) {
      console.error('Error saving tool:', error);
    } finally {
      setLoading(false);
    }
  };

  if (!isOpen) return null;

  return (
    <div
      className="position-fixed"
      style={{ 
        top: '55px',
        left: '0',
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
      <div className="modal-dialog" style={{ margin: 0, maxWidth: '90%', width: '500px' }}>
        <div className="modal-content shadow-lg" style={{ backgroundColor: 'white', border: '1px solid #dee2e6' }}>
          <div className="modal-header" style={{ backgroundColor: '#f8f9fa', borderBottom: '1px solid #dee2e6', padding: '16px 24px' }}>
            <h5 className="modal-title text-dark">{title}</h5>
            <button type="button" className="btn-close" onClick={onClose}></button>
          </div>
          <form onSubmit={handleSubmit}>
            <div className="modal-body" style={{ backgroundColor: 'white', padding: '24px' }}>
              <div className="mb-3">
                <label htmlFor="name" className="form-label text-dark">Navn *</label>
                <input
                  type="text"
                  className="form-control"
                  id="name"
                  value={formData.name}
                  onChange={(e) => handleChange('name', e.target.value)}
                  required
                  style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                />
              </div>

              <div className="mb-3">
                <label htmlFor="vehicleId" className="form-label text-dark">Tilknyttet køretøj</label>
                <select
                  className="form-select"
                  id="vehicleId"
                  value={formData.vehicleId}
                  onChange={(e) => handleChange('vehicleId', e.target.value)}
                  style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                >
                  <option value="">Ingen køretøj...</option>
                  {vehicles.map((vehicle) => (
                    <option key={vehicle.data.itemId} value={vehicle.data.itemId}>
                      {vehicle.model} {vehicle.licensePlate ? `(${vehicle.licensePlate})` : ''}
                    </option>
                  ))}
                </select>
              </div>

              <div className="mb-3">
                <label htmlFor="defaultStorageId" className="form-label text-dark">Standard lager</label>
                {formData.vehicleId ? (
                  <>
                    <div className="alert alert-info">
                      <small>
                        Køretøjet er allokeret til {vehicles.find(v => v.data.itemId === formData.vehicleId)?.data.defaultStorage?.name || 'et lager'}. 
                        Ved at oprette dette værktøj vil det blive tilknyttet det lager.
                      </small>
                    </div>
                    <input
                      type="text"
                      className="form-control"
                      value={vehicles.find(v => v.data.itemId === formData.vehicleId)?.data.defaultStorage?.name || ''}
                      disabled
                      readOnly
                      style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                    />
                  </>
                ) : (
                  <select
                    className="form-select"
                    id="defaultStorageId"
                    value={formData.defaultStorageId}
                    onChange={(e) => handleChange('defaultStorageId', e.target.value)}
                    required
                    style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                  >
                    <option value="">Vælg lager...</option>
                    <option value="a4111111-1111-1111-1111-111111111111">Hovedlager Odense</option>
                    <option value="a4222222-2222-2222-2222-222222222222">Lager København</option>
                  </select>
                )}
              </div>
              
              <div className="mb-3">
                <label htmlFor="note" className="form-label text-dark">Note</label>
                <textarea
                  className="form-control"
                  id="note"
                  rows={3}
                  value={formData.note}
                  onChange={(e) => handleChange('note', e.target.value)}
                  style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                />
              </div>
            </div>
            <div className="modal-footer" style={{ backgroundColor: '#f8f9fa', borderTop: '1px solid #dee2e6', gap: '10px', padding: '16px 24px' }}>
              <button type="button" className="btn btn-secondary" onClick={onClose}>
                Annuller
              </button>
              <button type="submit" className="btn btn-primary" disabled={loading}>
                {loading ? 'Gemmer...' : 'Gem'}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}