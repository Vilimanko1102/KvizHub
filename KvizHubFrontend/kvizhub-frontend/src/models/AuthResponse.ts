import { User } from "./User";

export interface AuthResponse {
    token: string;
    expiration: string; 
    user: User;
}