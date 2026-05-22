export interface LoginRequest { email: string; password: string }

export interface WorkspaceRol {
  workspaceId: number;
  nombreWorkspace: string;
  rol: string;
}

export interface LoginResponse { 
  usuarioId: number; 
  email: string; 
  tempToken: string;
  workspaces: WorkspaceRol[];
}

export interface TokenResponse { 
  token: string; 
  workspaceId: number; 
  rol: string;
  expiration: string;
}

export interface Project { id: number; nombre: string; workspaceId: number }