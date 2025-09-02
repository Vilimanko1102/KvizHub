import axios from "axios";
import { AuthResponse } from "../models/AuthResponse";

const API_URL = process.env.REACT_APP_API_URL_USERS; // npr. http://localhost:5000

// ====================
// Frontend modeli / DTO-i
// ====================
export interface LoginDto {
    usernameOrEmail: string;
    password: string;
}

export interface RegisterDto {
    username: string;
    email: string;
    password: string;
    avatarUrl?: string;
}

export interface User {
    id: number;
    username: string;
    email: string;
    avatarUrl?: string;
    isAdmin?: boolean;
    role: string;
}

export interface UserAuthResponse {
    token: string;
    expiration: string;
    user: User;
}

// ====================
// Auth service
// ====================
export const loginUser = async (loginData: LoginDto): Promise<UserAuthResponse> => {
    const { data } = await axios.post<UserAuthResponse>(`${API_URL}/login`, loginData);
    return data;
};

export const registerUser = async (registerData: RegisterDto): Promise<UserAuthResponse> => {
    const { data } = await axios.post<UserAuthResponse>(`${API_URL}/register`, registerData);
    return data;
};

export const getCurrentUser = async (token: string): Promise<User> => {
    const { data } = await axios.get<User>(`${API_URL}/me`, {
        headers: { Authorization: `Bearer ${token}` }
    });
    return data;
};

const AuthService = { loginUser, registerUser, getCurrentUser };
export default AuthService;
