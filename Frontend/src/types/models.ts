export interface User {
  id: string;
  name: string;
  email: string;
  role: 'Admin' | 'User';
  createdAt: string;
}

export interface Project {
  id: string;
  name: string;
  description?: string;
  createdByAdminId: string;
  createdAt: string;
}

export interface Allocation {
  id: string;
  userId: string;
  userName: string;
  projectId: string;
  projectName: string;
  isCompleted: boolean;
  completedAt?: string;
  assignedAt: string;
}

export interface AuthResponse {
  token: string;
  user: {
    id: string;
    name: string;
    email: string;
    role: 'Admin' | 'User';
  };
}

export interface AuthUser {
  id: string;
  name: string;
  email: string;
  role: 'Admin' | 'User';
}
