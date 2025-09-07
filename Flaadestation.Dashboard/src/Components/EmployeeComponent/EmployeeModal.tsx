import { useState, useEffect } from 'react';
import { EmployeeModel } from '../../Models/Employee';
import { vehicleService, VehicleModel } from '../../Services/VehicleService';

export interface EmployeeFormData {
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    note: string;
    occupationId: string;
    vehicleId?: string;
    defaultStorageId: string;
}

interface EmployeeModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSave: (data: EmployeeFormData) => Promise<void>;
    employee?: EmployeeModel | null;
    title: string;
}

export default function EmployeeModal({ isOpen, onClose, onSave, employee, title }: EmployeeModalProps) {
    const [formData, setFormData] = useState<EmployeeFormData>({
        firstName: '',
        lastName: '',
        email: '',
        phone: '',
        note: '',
        occupationId: '',
        vehicleId: '',
        defaultStorageId: '',
    });

    const [vehicles, setVehicles] = useState<VehicleModel[]>([]);
    const [loading, setLoading] = useState(false);
    const [errors, setErrors] = useState<Record<string, string>>({});

    useEffect(() => {
        if (isOpen) {
            loadVehicles();
            if (employee) {
                setFormData({
                    firstName: employee.data.firstName,
                    lastName: employee.data.lastName,
                    email: employee.data.email,
                    phone: employee.data.phone,
                    note: employee.data.note,
                    occupationId: employee.data.occupation.occupationId,
                    vehicleId: employee.data.vehicle?.itemId || '',
                    defaultStorageId: employee.data.defaultStorage.storageId,
                });
            } else {
                setFormData({
                    firstName: '',
                    lastName: '',
                    email: '',
                    phone: '',
                    note: '',
                    occupationId: '',
                    vehicleId: '',
                    defaultStorageId: '',
                });
            }
            setErrors({});
        }
    }, [isOpen, employee]);

    const loadVehicles = async () => {
        try {
            const vehicleModels = await vehicleService.getAllVehicles();
            setVehicles(vehicleModels);
        } catch (error) {
            console.error('Error loading vehicles:', error);
        }
    };

    const handleChange = (field: keyof EmployeeFormData, value: string) => {
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
        
        if (errors[field]) {
            setErrors(prev => ({ ...prev, [field]: '' }));
        }
    };

    const validateForm = (): boolean => {
        const newErrors: Record<string, string> = {};

        if (!formData.firstName.trim()) newErrors.firstName = 'Fornavn er påkrævet';
        if (!formData.lastName.trim()) newErrors.lastName = 'Efternavn er påkrævet';
        if (!formData.email.trim()) newErrors.email = 'Email er påkrævet';
        if (!formData.phone.trim()) newErrors.phone = 'Telefon er påkrævet';

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (formData.email && !emailRegex.test(formData.email)) {
            newErrors.email = 'Ugyldig email format';
        }

        const cleanPhone = formData.phone.replace(/\D/g, '');
        if (formData.phone && cleanPhone.length !== 8) {
            newErrors.phone = 'Telefon skal være 8 cifre';
        }

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!validateForm()) return;

        setLoading(true);
        try {
            const submitData = {
                ...formData,
                vehicleId: formData.vehicleId || undefined
            };
            await onSave(submitData);
            onClose();
        } catch (error) {
            console.error('Error saving employee:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleBackdropClick = (e: React.MouseEvent) => {
        if (e.target === e.currentTarget && !loading) {
            onClose();
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
            onClick={handleBackdropClick}
        >
            <div className="modal-dialog" style={{ margin: 0, maxWidth: '90%', width: '600px' }}>
                <div className="modal-content shadow-lg" style={{ backgroundColor: 'white', border: '1px solid #dee2e6' }}>
                    <div className="modal-header" style={{ backgroundColor: '#f8f9fa', borderBottom: '1px solid #dee2e6', padding: '16px 24px' }}>
                        <h5 className="modal-title text-dark">{title}</h5>
                        <button
                            type="button"
                            className="btn-close"
                            onClick={onClose}
                            disabled={loading}
                        />
                    </div>

                    <form onSubmit={handleSubmit}>
                        <div className="modal-body" style={{ backgroundColor: 'white', padding: '24px' }}>
                            <div className="row">
                                <div className="col-md-6 mb-3">
                                    <label className="form-label text-dark">Fornavn *</label>
                                    <input
                                        type="text"
                                        className={`form-control ${errors.firstName ? 'is-invalid' : ''}`}
                                        value={formData.firstName}
                                        onChange={(e) => handleChange('firstName', e.target.value)}
                                        disabled={loading}
                                        style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                                    />
                                    {errors.firstName && <div className="invalid-feedback">{errors.firstName}</div>}
                                </div>

                                <div className="col-md-6 mb-3">
                                    <label className="form-label text-dark">Efternavn *</label>
                                    <input
                                        type="text"
                                        className={`form-control ${errors.lastName ? 'is-invalid' : ''}`}
                                        value={formData.lastName}
                                        onChange={(e) => handleChange('lastName', e.target.value)}
                                        disabled={loading}
                                        style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                                    />
                                    {errors.lastName && <div className="invalid-feedback">{errors.lastName}</div>}
                                </div>
                            </div>

                            <div className="mb-3">
                                <label className="form-label text-dark">Email *</label>
                                <input
                                    type="email"
                                    className={`form-control ${errors.email ? 'is-invalid' : ''}`}
                                    value={formData.email}
                                    onChange={(e) => handleChange('email', e.target.value)}
                                    disabled={loading}
                                    style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                                />
                                {errors.email && <div className="invalid-feedback">{errors.email}</div>}
                            </div>

                            <div className="mb-3">
                                <label className="form-label text-dark">Telefon *</label>
                                <input
                                    type="tel"
                                    className={`form-control ${errors.phone ? 'is-invalid' : ''}`}
                                    value={formData.phone}
                                    onChange={(e) => handleChange('phone', e.target.value)}
                                    placeholder="12345678"
                                    disabled={loading}
                                    style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                                />
                                {errors.phone && <div className="invalid-feedback">{errors.phone}</div>}
                            </div>

                            <div className="mb-3">
                                <label className="form-label text-dark">Stilling</label>
                                <select
                                    className="form-select"
                                    value={formData.occupationId}
                                    onChange={(e) => handleChange('occupationId', e.target.value)}
                                    disabled={loading}
                                    style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                                >
                                    <option value="">Vælg stilling...</option>
                                    <option value="f1234567-1234-1234-1234-123456789012">Tømrer</option>
                                    <option value="f2345678-2345-2345-2345-234567890123">Elektriker</option>
                                </select>
                            </div>

                            <div className="mb-3">
                                <label className="form-label text-dark">Tilknyttet køretøj</label>
                                <select
                                    className="form-select"
                                    value={formData.vehicleId}
                                    onChange={(e) => handleChange('vehicleId', e.target.value)}
                                    disabled={loading}
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
                                <label className="form-label text-dark">Standard lager</label>
                                {formData.vehicleId ? (
                                    <>
                                        <div className="alert alert-info">
                                            <small>
                                                Køretøjet er allokeret til {vehicles.find(v => v.data.itemId === formData.vehicleId)?.data.defaultStorage?.name || 'et lager'}. 
                                                Ved at forsætte vil denne medarbejder blive tilknyttet det lager.
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
                                        value={formData.defaultStorageId}
                                        onChange={(e) => handleChange('defaultStorageId', e.target.value)}
                                        disabled={loading}
                                        style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                                    >
                                        <option value="">Vælg lager...</option>
                                        <option value="a4111111-1111-1111-1111-111111111111">Hovedlager Odense</option>
                                        <option value="a4222222-2222-2222-2222-222222222222">Lager København</option>
                                    </select>
                                )}
                            </div>

                            <div className="mb-3">
                                <label className="form-label text-dark">Note</label>
                                <textarea
                                    className="form-control"
                                    rows={3}
                                    value={formData.note}
                                    onChange={(e) => handleChange('note', e.target.value)}
                                    disabled={loading}
                                    style={{ backgroundColor: 'white', border: '1px solid #ced4da' }}
                                />
                            </div>
                        </div>

                        <div className="modal-footer" style={{ backgroundColor: '#f8f9fa', borderTop: '1px solid #dee2e6', gap: '10px', padding: '16px 24px' }}>
                            <button
                                type="button"
                                className="btn btn-secondary"
                                onClick={onClose}
                                disabled={loading}
                            >
                                Annuller
                            </button>
                            <button
                                type="submit"
                                className="btn btn-primary"
                                disabled={loading}
                            >
                                {loading && <span className="spinner-border spinner-border-sm me-2" />}
                                {employee ? 'Opdater' : 'Opret'}
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}