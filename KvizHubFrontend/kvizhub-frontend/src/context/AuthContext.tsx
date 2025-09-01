import React, { createContext, useContext, useState, ReactNode, useEffect } from "react";
import { loginUser, registerUser, getCurrentUser } from "../services/authService";

interface User {
    id: number;
    username: string;
    email: string;
    avatarUrl?: string;
    isAdmin?: boolean;
}

interface AuthContextType {
    user: User | null;
    login: (usernameOrEmail: string, password: string) => Promise<void>;
    register: (data: { username: string; email: string; password: string; avatarUrl?: string }) => Promise<void>;
    logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [user, setUser] = useState<User | null>(null);

    useEffect(() => {
        const token = localStorage.getItem("token");
        if (token) {
            getCurrentUser(token).then(u => setUser(u)).catch(() => localStorage.removeItem("token"));
        }
    }, []);

    const login = async (usernameOrEmail: string, password: string) => {
        const response = await loginUser(usernameOrEmail, password);
        localStorage.setItem("token", response.token);
        setUser(response.user);
    };

    const register = async (data: { username: string; email: string; password: string; avatarUrl?: string }) => {
        const response = await registerUser(data);
        localStorage.setItem("token", response.token);
        setUser(response.user);
    };

    const logout = () => {
        localStorage.removeItem("token");
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{ user, login, register, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) throw new Error("useAuth must be used within AuthProvider");
    return context;
};
