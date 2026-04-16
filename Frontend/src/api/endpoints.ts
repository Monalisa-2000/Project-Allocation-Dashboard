import axiosClient from './axiosClient';
import type { AuthResponse, Project, User, Allocation } from '@/types/models';

export const authApi = {
  login: (email: string, password: string) =>
    axiosClient.post<AuthResponse>('/Auth/login', { email, password }).then(r => r.data),
};

export const projectsApi = {
  getAll: () => axiosClient.get<Project[]>('/Projects').then(r => r.data),
  create: (data: { name: string; description?: string }) =>
    axiosClient.post<Project>('/Projects', data).then(r => r.data),
  delete: (id: string) => axiosClient.delete(`/Projects/${id}`).then(r => r.data),
};

export const usersApi = {
  getAll: () => axiosClient.get<User[]>('/Users').then(r => r.data),
  delete: (id: string) => axiosClient.delete(`/Users/${id}`).then(r => r.data),
};

export const allocationsApi = {
  getAll: () => axiosClient.get<Allocation[]>('/Allocations').then(r => r.data),
  getMy: () => axiosClient.get<Allocation[]>('/Allocations/my').then(r => r.data),
  assign: (data: { userId: string; projectIds: string[] }) =>
    axiosClient.post('/Allocations', data).then(r => r.data),
  complete: (id: string) =>
    axiosClient.patch(`/Allocations/${id}/complete`).then(r => r.data),
  delete: (id: string) => axiosClient.delete(`/Allocations/${id}`).then(r => r.data),
};
