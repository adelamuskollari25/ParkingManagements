export enum UserRole {
  Admin  = 'Admin',
  Attendant = 'Attendant',
  Viewer = 'Viewer'
}

export interface User {
  id: string;
  email: string;
  pass: string;
  role: UserRole;
  isActive: boolean;
  createdAt?: Date;
  updatedAt?: Date;
}

//use it later
export interface AuthResponse {
  user: User;
  token: string;
  refreshToken?: string;
  expiresIn: number;
}

export interface LogInRequest {
  email: string;
  password: string;
}
