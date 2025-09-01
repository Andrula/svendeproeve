import { useState, useEffect } from 'react';
import { EmployeeModel } from '../../Models/Employee';

export interface EmployeeFormData {
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    note: string;
    occupationId: string;
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
        defaultStorageId: '',
    });

    const [loading, setLoading] = useState(false);
    const [errors, setErrors] = useState<Record<string, string>>({});

    useEffect(() => {
        if (isOpen) {
            if (employee) {
                setFormData({
                    firstName: employee.data.firstName,
                    lastName: employee.data.lastName,
                    email: employee.data.email,
                    phone: employee.data.phone,
                    note: employee.data.note,
                    occupationId: employee.data.occupation.occupationId,
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
                    defaultStorageId: '',
                });
            }
            setErrors({});
        }
    }, [isOpen, employee]);

    const handleChange = (field: keyof EmployeeFormData, value: string) => {
        setFormData(prev => ({ ...prev, [field]: value }));
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
            await onSave(formData);
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

    if (!isOpen) return null;

    return (
        <div
            className="position-absolute top-0 start-0 w-100 d-flex justify-content-center align-items-start"
            style={{
                backgroundColor: 'rgba(0,0,0,0.5)',
                zIndex: 1050,
                height: '100%',
                minHeight: '600px',
                paddingTop: '2rem', 
                paddingBottom: '2rem' 
            }}
            onClick={handleBackdropClick}
        >
            <div className="modal-dialog" style={{ margin: '0', maxWidth: '600px', width: '90%' }}>
                <div
                    className="modal-content"
                    style={{
                        backgroundColor: 'white',
                        borderRadius: '0.375rem',
                        boxShadow: '0 0.125rem 0.25rem rgba(0, 0, 0, 0.075)'
                    }}
                >
                    <div className="modal-header" style={{ padding: '1rem 1.5rem' }}>
                        <h5 className="modal-title">{title}</h5>
                        <button
                            type="button"
                            className="btn-close"
                            onClick={onClose}
                            disabled={loading}
                        />
                    </div>

                    <form onSubmit={handleSubmit}>
                        <div className="modal-body" style={{ padding: '1.5rem' }}>
                            <div className="row">
                                <div className="col-md-6 mb-3">
                                    <label className="form-label">Fornavn *</label>
                                    <input
                                        type="text"
                                        className={`form-control ${errors.firstName ? 'is-invalid' : ''}`}
                                        value={formData.firstName}
                                        onChange={(e) => handleChange('firstName', e.target.value)}
                                        disabled={loading}
                                    />
                                    {errors.firstName && <div className="invalid-feedback">{errors.firstName}</div>}
                                </div>

                                <div className="col-md-6 mb-3">
                                    <label className="form-label">Efternavn *</label>
                                    <input
                                        type="text"
                                        className={`form-control ${errors.lastName ? 'is-invalid' : ''}`}
                                        value={formData.lastName}
                                        onChange={(e) => handleChange('lastName', e.target.value)}
                                        disabled={loading}
                                    />
                                    {errors.lastName && <div className="invalid-feedback">{errors.lastName}</div>}
                                </div>
                            </div>

                            <div className="mb-3">
                                <label className="form-label">Email *</label>
                                <input
                                    type="email"
                                    className={`form-control ${errors.email ? 'is-invalid' : ''}`}
                                    value={formData.email}
                                    onChange={(e) => handleChange('email', e.target.value)}
                                    disabled={loading}
                                />
                                {errors.email && <div className="invalid-feedback">{errors.email}</div>}
                            </div>

                            <div className="mb-3">
                                <label className="form-label">Telefon *</label>
                                <input
                                    type="tel"
                                    className={`form-control ${errors.phone ? 'is-invalid' : ''}`}
                                    value={formData.phone}
                                    onChange={(e) => handleChange('phone', e.target.value)}
                                    placeholder="12345678"
                                    disabled={loading}
                                />
                                {errors.phone && <div className="invalid-feedback">{errors.phone}</div>}
                            </div>

                            <div className="mb-3">
                                <label className="form-label">Stilling</label>
                                <select
                                    className="form-select"
                                    value={formData.occupationId}
                                    onChange={(e) => handleChange('occupationId', e.target.value)}
                                    disabled={loading}
                                >
                                    <option value="">Vælg stilling...</option>
                                    <option value="f1234567-1234-1234-1234-123456789012">Tømrer</option>
                                    <option value="f2345678-2345-2345-2345-234567890123">Elektriker</option>
                                </select>
                            </div>

                            <div className="mb-3">
                                <label className="form-label">Standard lager</label>
                                <select
                                    className="form-select"
                                    value={formData.defaultStorageId}
                                    onChange={(e) => handleChange('defaultStorageId', e.target.value)}
                                    disabled={loading}
                                >
                                    <option value="">Vælg lager...</option>
                                    <option value="a4111111-1111-1111-1111-111111111111">Hovedlager Odense</option>
                                    <option value="a4222222-2222-2222-2222-222222222222">Lager København</option>
                                </select>
                            </div>

                            <div className="mb-3">
                                <label className="form-label">Note</label>
                                <textarea
                                    className="form-control"
                                    rows={3}
                                    value={formData.note}
                                    onChange={(e) => handleChange('note', e.target.value)}
                                    disabled={loading}
                                />
                            </div>
                        </div>

                        <div className="modal-footer" style={{ padding: '1rem 1.5rem' }}>
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
                                className="btn btn-primary ms-2"
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