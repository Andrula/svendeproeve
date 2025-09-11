import { useState, useEffect } from 'react';
import { dawaService } from '../../Services/DawaService';

export interface JobFormData {
  title: string;
  description: string;
  scheduledStart: string;
  scheduledEnd: string;
  addressId: string;
}

export interface BaseFormData {
  name: string;
  addressId: string;
}

interface MapModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSaveJob: (formData: JobFormData) => Promise<void>;
  onSaveBase: (formData: BaseFormData) => Promise<void>;
  title: string;
}

export default function MapModal({ isOpen, onClose, onSaveJob, onSaveBase, title }: MapModalProps) {
  const [creationType, setCreationType] = useState<'job' | 'base'>('job');
  const [jobData, setJobData] = useState<JobFormData>({
    title: '',
    description: '',
    scheduledStart: '',
    scheduledEnd: '',
    addressId: ''
  });
  const [baseData, setBaseData] = useState<BaseFormData>({
    name: '',
    addressId: ''
  });
  const [addressQuery, setAddressQuery] = useState('');
  const [addressSuggestions, setAddressSuggestions] = useState<any[]>([]);
  const [selectedAddress, setSelectedAddress] = useState<any>(null);
  const [loading, setLoading] = useState(false);
  const [loadingAddresses, setLoadingAddresses] = useState(false);

  useEffect(() => {
    if (addressQuery.length > 2) {
      searchAddresses(addressQuery);
    } else {
      setAddressSuggestions([]);
    }
  }, [addressQuery]);

  const searchAddresses = async (query: string) => {
    setLoadingAddresses(true);
    try {
      const suggestions = await dawaService.getAutoComplete(query); 
      console.log('Address suggestions:', suggestions);
      setAddressSuggestions(suggestions);
    } catch (error) {
      console.error('Error fetching addresses:', error);
      setAddressSuggestions([]);
    } finally {
      setLoadingAddresses(false);
    }
  };

  const selectAddress = (suggestion: any) => {
    try {
      console.log('Selected suggestion:', suggestion); 
      const addressData = suggestion.adresse;
      setSelectedAddress(addressData);
      setAddressQuery(suggestion.tekst);
      setAddressSuggestions([]);
      
      if (creationType === 'job') {
        setJobData({ ...jobData, addressId: addressData.id });
      } else {
        setBaseData({ ...baseData, addressId: addressData.id });
      }
    } catch (error) {
      console.error('Error selecting address:', error);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    
    try {
      if (creationType === 'job') {
        if (!selectedAddress) {
          alert('Vælg venligst en adresse');
          return;
        }
        await onSaveJob(jobData);
      } else {
        if (!selectedAddress) {
          alert('Vælg venligst en adresse');
          return;
        }
        await onSaveBase(baseData);
      }
      handleClose();
    } catch (error) {
      console.error('Error saving:', error);
      alert('Fejl ved oprettelse. Prøv igen.');
    } finally {
      setLoading(false);
    }
  };

  const handleClose = () => {
    setCreationType('job');
    setJobData({
      title: '',
      description: '',
      scheduledStart: '',
      scheduledEnd: '',
      addressId: ''
    });
    setBaseData({
      name: '',
      addressId: ''
    });
    setAddressQuery('');
    setAddressSuggestions([]);
    setSelectedAddress(null);
    onClose();
  };

  const handleCreationTypeChange = (type: 'job' | 'base') => {
    setCreationType(type);
    setAddressQuery('');
    setAddressSuggestions([]);
    setSelectedAddress(null);
    if (type === 'job') {
      setJobData({ ...jobData, addressId: '' });
    } else {
      setBaseData({ ...baseData, addressId: '' });
    }
  };

  if (!isOpen) return null;

  return (
    <div
      className="position-fixed"
      style={{ 
        top: '0',
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
      <div className="modal-dialog" style={{ margin: 0, maxWidth: '90%', width: '600px' }}>
        <div className="modal-content shadow-lg" style={{ backgroundColor: 'white', border: '1px solid #dee2e6' }}>
          <div className="modal-header" style={{ backgroundColor: '#f8f9fa', borderBottom: '1px solid #dee2e6', padding: '16px 24px' }}>
            <h5 className="modal-title text-dark">{title}</h5>
            <button type="button" className="btn-close" onClick={handleClose}></button>
          </div>
          <form onSubmit={handleSubmit}>
            <div className="modal-body" style={{ backgroundColor: 'white', padding: '24px' }}>
              
              <div className="mb-4">
                <label htmlFor="creationType" className="form-label text-dark">Type *</label>
                <select
                  className="form-select"
                  id="creationType"
                  value={creationType}
                  onChange={(e) => handleCreationTypeChange(e.target.value as 'job' | 'base')}
                  style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                >
                  <option value="job">Opgave</option>
                  <option value="base">Base</option>
                </select>
              </div>

              {creationType === 'job' ? (
                <>
                  <div className="mb-3">
                    <label htmlFor="jobTitle" className="form-label text-dark">Titel *</label>
                    <input
                      type="text"
                      className="form-control"
                      id="jobTitle"
                      value={jobData.title}
                      onChange={(e) => setJobData({ ...jobData, title: e.target.value })}
                      required
                      style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                    />
                  </div>

                  <div className="mb-3">
                    <label htmlFor="description" className="form-label text-dark">Beskrivelse</label>
                    <textarea
                      className="form-control"
                      id="description"
                      rows={3}
                      value={jobData.description}
                      onChange={(e) => setJobData({ ...jobData, description: e.target.value })}
                      style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                    />
                  </div>

                  <div className="row">
                    <div className="col-md-6 mb-3">
                      <label htmlFor="scheduledStart" className="form-label text-dark">Start dato *</label>
                      <input
                        type="date"
                        className="form-control"
                        id="scheduledStart"
                        value={jobData.scheduledStart}
                        onChange={(e) => setJobData({ ...jobData, scheduledStart: e.target.value })}
                        required
                        style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                      />
                    </div>
                    <div className="col-md-6 mb-3">
                      <label htmlFor="scheduledEnd" className="form-label text-dark">Slut dato *</label>
                      <input
                        type="date"
                        className="form-control"
                        id="scheduledEnd"
                        value={jobData.scheduledEnd}
                        onChange={(e) => setJobData({ ...jobData, scheduledEnd: e.target.value })}
                        required
                        style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                      />
                    </div>
                  </div>
                </>
              ) : (
                <div className="mb-3">
                  <label htmlFor="baseName" className="form-label text-dark">Navn *</label>
                  <input
                    type="text"
                    className="form-control"
                    id="baseName"
                    value={baseData.name}
                    onChange={(e) => setBaseData({ ...baseData, name: e.target.value })}
                    required
                    style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                  />
                </div>
              )}

              <div className="mb-3">
                <label htmlFor="address" className="form-label text-dark">Adresse *</label>
                <div className="position-relative">
                  <input
                    type="text"
                    className="form-control"
                    id="address"
                    value={addressQuery}
                    onChange={(e) => setAddressQuery(e.target.value)}
                    placeholder="Søg efter adresse..."
                    required
                    style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                  />
                  {loadingAddresses && (
                    <div className="position-absolute top-50 end-0 translate-middle-y me-3">
                      <div className="spinner-border spinner-border-sm" role="status">
                        <span className="visually-hidden">Søger...</span>
                      </div>
                    </div>
                  )}
                  {addressSuggestions && addressSuggestions.length > 0 && (
                    <div 
                      className="position-absolute w-100 bg-white border rounded shadow-sm"
                      style={{ top: '100%', zIndex: 1050, maxHeight: '200px', overflowY: 'auto' }}
                    >
                      {addressSuggestions.map((suggestion, index) => (
                        <div
                          key={index}
                          className="p-2 border-bottom cursor-pointer"
                          style={{ cursor: 'pointer' }}
                          onClick={() => selectAddress(suggestion)}
                          onMouseEnter={(e) => e.currentTarget.style.backgroundColor = '#f8f9fa'}
                          onMouseLeave={(e) => e.currentTarget.style.backgroundColor = 'white'}
                        >
                          <div className="fw-bold">{suggestion.tekst || 'Unknown address'}</div>
                          <small className="text-muted">
                            {suggestion.adresse?.postnr && suggestion.adresse?.postnrnavn 
                              ? `${suggestion.adresse.postnr} ${suggestion.adresse.postnrnavn}`
                              : 'No postal info available'
                            }
                          </small>
                        </div>
                      ))}
                    </div>
                  )}
                </div>
                {selectedAddress && (
                  <div className="mt-2 p-2 bg-light rounded">
                    <small className="text-success">
                      <i className="bi bi-check-circle me-1"></i>
                      Valgt: {selectedAddress.vejnavn} {selectedAddress.husnr}, {selectedAddress.postnr} {selectedAddress.postnrnavn}
                    </small>
                  </div>
                )}
              </div>
            </div>
            <div className="modal-footer" style={{ backgroundColor: '#f8f9fa', borderTop: '1px solid #dee2e6', gap: '10px', padding: '16px 24px' }}>
              <button type="button" className="btn btn-secondary" onClick={handleClose}>
                Annuller
              </button>
              <button type="submit" className="btn btn-primary" disabled={loading || !selectedAddress}>
                {loading ? 'Opretter...' : `Opret ${creationType === 'job' ? 'Opgave' : 'Base'}`}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}