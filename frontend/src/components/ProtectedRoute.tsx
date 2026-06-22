import React, { useEffect, useState } from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { api } from '../api/client';

export const ProtectedRoute = () => {
  const token = localStorage.getItem('token');
  const [isVerifying, setIsVerifying] = useState(true);
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    if (!token) {
      setIsAuthenticated(false);
      setIsVerifying(false);
      return;
    }

    // Ping the backend to check if the token is completely valid
    api.get('/auth/me')
      .then(() => {
        setIsAuthenticated(true);
      })
      .catch(() => {
        localStorage.removeItem('token'); // Clear bad tokens
        setIsAuthenticated(false);
      })
      .finally(() => {
        setIsVerifying(false);
      });
  }, [token]);

  if (isVerifying) {
    return <div className="p-8 text-center font-medium">Verifying session...</div>;
  }

  return isAuthenticated ? <Outlet /> : <Navigate to="/login" replace />;
};

