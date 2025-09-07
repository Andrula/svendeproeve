import { useState } from "react";
import { useLocation, Navigate, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { useAuth } from "../../Auth/AuthContext";

export interface CreateOwnerFormData {
  email: string;
  password: string;
  company: string;
  amountOfLicenses: number;
}

interface PaymentFormData extends CreateOwnerFormData {
  cardNumber: string;
  expiryDate: string;
  cvc: string;
}

const CheckoutComponent: React.FC = () => {
    const { registerOwner } = useAuth();
    const navigate = useNavigate();
  const location = useLocation();
  const data = location.state as CreateOwnerFormData | undefined;

  const [formData, setFormData] = useState<PaymentFormData | null>(
    data ? { ...data, cardNumber: "", expiryDate: "", cvc: "" } : null
  );
  const [errors, setErrors] = useState<
    Partial<Record<keyof PaymentFormData, string>>
  >({});
  
  if (!data) return <Navigate to="/register" replace />;

  const validate = () => {
    if (!formData) return false;
    const newErrors: typeof errors = {};

    if (
      !formData.cardNumber ||
      !/^\d{16}$/.test(formData.cardNumber.replace(/\s+/g, ""))
    ) {
      newErrors.cardNumber = "Indtast et gyldigt 16-cifret kortnummer";
    }
    if (
      !formData.expiryDate ||
      !/^(0[1-9]|1[0-2])\/\d{2}$/.test(formData.expiryDate)
    ) {
      newErrors.expiryDate = "Indtast en gyldig udløbsdato (MM/YY)";
    }
    if (!formData.cvc || !/^\d{3,4}$/.test(formData.cvc)) {
      newErrors.cvc = "Indtast en gyldig CVC";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validate() || !formData) return;


    const success = await registerOwner({
        email: data.email,
        password: data.password,
        companyName: data.company,
        companyAddressId: '0623D863-2049-489E-93D0-4C0280A9F3C1',
        amountOfLicenses: data.amountOfLicenses
    })
    
    if (success) {
        toast.success("Bruger oprettet!");
        navigate("/login", { replace: true });
    }
  };

  return (
    <div className="d-flex justify-content-center mt-5">
      <div className="card shadow-lg" style={{ width: "38rem" }}>
        <div className="card-header text-center bg-dark text-app-primary">
          <h4>Checkout</h4>
        </div>
        <form onSubmit={handleSubmit} noValidate>
          <div className="card-body">
            <div className="form-floating mb-3">
              <input
                id="cardNumber"
                placeholder="1234 5678 9012 3456"
                type="text"
                value={formData?.cardNumber || ""}
                onChange={(e) =>
                  setFormData(
                    (prev) => prev && { ...prev, cardNumber: e.target.value }
                  )
                }
                className={`form-control ${
                  errors.cardNumber ? "is-invalid" : ""
                }`}
              />
              {errors.cardNumber && (
                <div className="invalid-feedback">{errors.cardNumber}</div>
              )}
              <label htmlFor="cardNumber">Kortnummer</label>
            </div>

            <div className="form-floating mb-3">
              <input
                id="expiryDate"
                placeholder="MM/YY"
                type="text"
                value={formData?.expiryDate || ""}
                onChange={(e) =>
                  setFormData(
                    (prev) => prev && { ...prev, expiryDate: e.target.value }
                  )
                }
                className={`form-control ${
                  errors.expiryDate ? "is-invalid" : ""
                }`}
              />
              {errors.expiryDate && (
                <div className="invalid-feedback">{errors.expiryDate}</div>
              )}
              <label htmlFor="expiryDate">Udløbsdato</label>
            </div>

            <div className="form-floating mb-3">
              <input
                id="cvc"
                placeholder="CVC"
                type="text"
                value={formData?.cvc || ""}
                onChange={(e) =>
                  setFormData(
                    (prev) => prev && { ...prev, cvc: e.target.value }
                  )
                }
                className={`form-control ${errors.cvc ? "is-invalid" : ""}`}
              />
              {errors.cvc && (
                <div className="invalid-feedback">{errors.cvc}</div>
              )}
              <label htmlFor="cvc">CVC</label>
            </div>

            <div className="text-center mt-3">
              <button type="submit" className="btn btn-primary w-100">
                Betal
              </button>
            </div>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CheckoutComponent;
