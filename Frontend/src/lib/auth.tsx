import { createContext, useContext, useState, useCallback, useEffect, type ReactNode } from 'react';
import { jwtDecode } from 'jwt-decode';
import type { AuthUser } from '@/types/models';

interface AuthContextType {
  user: AuthUser | null;
  token: string | null;
  login: (token: string, user: AuthUser) => void;
  logout: () => void;
  isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType | null>(null);

function decodeToken(token: string): AuthUser | null {
  try {
    const decoded = jwtDecode<{ id: string; name: string; email: string; role: 'Admin' | 'User' }>(token);
    return decoded;
  } catch {
    return null;
  }
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [token, setToken] = useState<string | null>(() => {
    if (typeof window !== 'undefined') return localStorage.getItem('token');
    return null;
  });
  const [user, setUser] = useState<AuthUser | null>(() => {
    if (typeof window !== 'undefined') {
      const t = localStorage.getItem('token');
      const u = localStorage.getItem('user');
      if (u) try { return JSON.parse(u); } catch { /* ignore */ }
      if (t) return decodeToken(t);
    }
    return null;
  });

  const login = useCallback((newToken: string, newUser: AuthUser) => {
    localStorage.setItem('token', newToken);
    localStorage.setItem('user', JSON.stringify(newUser));
    setToken(newToken);
    setUser(newUser);
  }, []);

  const logout = useCallback(() => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setToken(null);
    setUser(null);
  }, []);

  useEffect(() => {
    if (token && !user) {
      const decoded = decodeToken(token);
      if (!decoded) {
        logout();
      } else {
        setUser(decoded);
      }
    }
  }, [token, user, logout]);

  return (
    <AuthContext.Provider value={{ user, token, login, logout, isAuthenticated: !!token && !!user }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used within AuthProvider');
  return ctx;
}
