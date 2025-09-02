import { useState, useEffect } from 'react';
import { MachineryModel } from '../../Models/Machinery'; 

export interface MachineryFormData {
  name: string;
  note: string;
  defaultStorageId: string;
}

interface MachineryModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSave: (formData: MachineryFormData) => Promise<void>;
  machinery: MachineryModel | null;
  title: string;
}

export default function MachineryModal({ isOpen, onClose, onSave, machinery, title }: MachineryModalProps) {
  const [formData, setFormData] = useState<MachineryFormData>({
    name: '',
    note: '',
    defaultStorageId: ''
  });
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (machinery) {
      setFormData({
        name: machinery.name,
        note: machinery.note,
        defaultStorageId: machinery.data.defaultStorage?.storageId || ''
      });
    } else {
      setFormData({
        name: '',
        note: '',
        defaultStorageId: ''
      });
    }
  }, [machinery, isOpen]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    
    try {
      await onSave(formData);
      onClose();
    } catch (error) {
      console.error('Error saving machinery:', error);
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
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                  required
                  style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                />
              </div>

              <div className="mb-3">
                <label htmlFor="defaultStorageId" className="form-label text-dark">Standard lager *</label>
                <select
                  className="form-select"
                  id="defaultStorageId"
                  value={formData.defaultStorageId}
                  onChange={(e) => setFormData({ ...formData, defaultStorageId: e.target.value })}
                  required
                  style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                >
                  <option value="">Vælg lager...</option>
                  <option value="a4111111-1111-1111-1111-111111111111">Hovedlager Odense</option>
                  <option value="a4222222-2222-2222-2222-222222222222">Lager København</option>
                </select>
              </div>
              
              <div className="mb-3">
                <label htmlFor="note" className="form-label text-dark">Note</label>
                <textarea
                  className="form-control"
                  id="note"
                  rows={3}
                  value={formData.note}
                  onChange={(e) => setFormData({ ...formData, note: e.target.value })}
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