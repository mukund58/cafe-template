import axios from 'axios';
export interface UserResponse {
  id: string; // Map your Guid here
  name: string;
  email: string;
  role: number; // Since Role maps to an integer in Postgres now
  profileImagePath: string | null;
  createdAt: string;
}

export interface AuthResponse {
  token: string;
  name: string;
  email: string;
  role: string;
}
export const api = axios.create({
  baseURL: 'http://localhost:5000/auth', // Points directly to your mapped API port
});


export const authService = {
  login: async (email: string, password: string): Promise<AuthResponse> => {
    const response = await api.post<AuthResponse>('/login', { email, password });
    localStorage.setItem('token', response.data.token); // Save token
    return response.data;
  },
    // Added: Register method to match your C# AuthEndpoints.cs
  register: async (name: string, email: string, password: string): Promise<AuthResponse> => {
    const response = await api.post<AuthResponse>('/register', { name, email, password });
    localStorage.setItem('token', response.data.token); // Auto-login after registration
    return response.data;
  },
  getMe: async (): Promise<UserResponse> => {
    const response = await api.get<UserResponse>('/me');
    return response.data;
  }
};

