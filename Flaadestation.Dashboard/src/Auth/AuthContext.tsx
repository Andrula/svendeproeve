import React, { createContext, useContext, useState, useEffect } from "react";
import { HttpClient } from "../Services/HttpClient";

type User = {
  id: string;
  email: string;
  isCompanyOwner: boolean;
  companyId: string;
};

type AuthContextType = {
  user: User | null;
  loading: Boolean;
  login: (email: string, password: string) => Promise<boolean>;
  logout: () => Promise<void>;
  registerOwner: (data: RegisterOwnerRequest) => Promise<boolean>;
  registerStaff: (data: RegisterStaffRequest) => Promise<boolean>;
  verifyLicenseKey: (data: string) => Promise<boolean>;
};

const httpClient = new HttpClient({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  timeout: 30000,
  withCredentials: true
});

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    httpClient.get<any>("/user")
      .then(res => {
        const claims = res;
        setUser({
          id: claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
          email: claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
          isCompanyOwner: claims["IsCompanyOwner"] === "true",
          companyId: claims["CompanyId"]
        });
      })
      .catch(() => setUser(null))
      .finally(() => setLoading(false));
  }, []);

  const login = async (email: string, password: string) => {
    try {

      await httpClient.post<any>("/user/login", { email, password });
      const res = await httpClient.get<any>("/user");
      const claims = res;
      setUser({
        id: claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
        email: claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
        isCompanyOwner: claims["IsCompanyOwner"] === "true",
        companyId: claims["companyId"]
      });
      return true;
    } catch (e) {
      console.log(e)
      return false;
    }
  };

  const logout = async () => {
    await httpClient.get<any>("/user/logout");
    setUser(null);
  };

  const registerOwner = async (data: RegisterOwnerRequest) => {
    try {
      await httpClient.post<any>("/User/register-owner", data);
      return true;
    } catch {
      return false;
    }
  };

  const registerStaff = async (data: RegisterStaffRequest) => {
    try {
      await httpClient.post<any>("/User/register-staff", data);
      return true;
    } catch {
      return false;
    }
  };

  const verifyLicenseKey = async (key: string) => {
    try {
      await httpClient.get<string>(`/license/key/${key}`);
      return true;
    } catch {
      return false;
    }
  };

  return (
    <AuthContext.Provider value={{ user, loading, login, logout, registerOwner, registerStaff, verifyLicenseKey }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) throw new Error("useAuth must be used within AuthProvider");
  return context;
};

export interface RegisterOwnerRequest {
  email: string;
  password: string;
  companyName: string;
  companyAddressId: string;
  amountOfLicenses: number;
}

export interface RegisterStaffRequest {
  email: string;
  password: string;
  licenseKey: string;
}