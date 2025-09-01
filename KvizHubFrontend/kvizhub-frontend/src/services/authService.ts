import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL; // npr. http://localhost:5000

export const loginUser = async (usernameOrEmail: string, password: string) => {
    const { data } = await axios.post(`${API_URL}/users/login`, { usernameOrEmail, password });
    return data; // { token, user }
};

export const registerUser = async (userData: { username: string; email: string; password: string; avatarUrl?: string }) => {
    const { data } = await axios.post(`${API_URL}/users/register`, userData);
    return data; // { token, user }
};

export const getCurrentUser = async (token: string) => {
    const { data } = await axios.get(`${API_URL}/users/me`, {
        headers: { Authorization: `Bearer ${token}` }
    });
    return data; // User object
};
