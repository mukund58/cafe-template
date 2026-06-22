import axios from 'axios';

export const api = axios.create({
  baseURL: 'http://localhost:5000', // Points directly to your mapped API port
});

// Automatically injects the JWT token if it exists in storage
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

