import { Injectable, signal, computed, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { LoginRequest, LoginResponse, TokenResponse } from '../models';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/Auth`;
  private http = inject(HttpClient);
  private router = inject(Router);

  private userSignal = signal<LoginResponse | null>(null);
  private tokenSignal = signal<string | null>(null);
  private currentWorkspaceSignal = signal<number | null>(null);
  private currentRolSignal = signal<string | null>(null);
  private tempTokenSignal = signal<string | null>(null);

  user = computed(() => this.userSignal());
  token = computed(() => this.tokenSignal());
  currentWorkspace = computed(() => this.currentWorkspaceSignal());
  currentRol = computed(() => this.currentRolSignal());
  isAuthenticated = computed(() => !!this.tokenSignal());

  constructor() { this.loadFromStorage(); }

  private loadFromStorage(): void {
    const token = localStorage.getItem('token');
    const user = localStorage.getItem('user');
    if (token) this.tokenSignal.set(token);
    if (user) {
      const userData = JSON.parse(user);
      this.userSignal.set(userData);
      this.tokenSignal.set(userData.token);
      this.currentWorkspaceSignal.set(userData.workspaceId);
      this.currentRolSignal.set(userData.rol);
    }
  }

  login(request: LoginRequest) {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, request).pipe(
      tap(response => {
        this.userSignal.set(response);
        this.tempTokenSignal.set(response.tempToken);
      })
    );
  }

  getToken(workspaceId: number, tempToken: string): any {
    return this.http.post<TokenResponse>(`${this.apiUrl}/token`, { workspaceId, tempToken }).pipe(
      tap(response => {
        this.tokenSignal.set(response.token);
        this.currentWorkspaceSignal.set(response.workspaceId);
        this.currentRolSignal.set(response.rol);

        const userData = this.userSignal();
        if (userData) {
          const updatedUser = { ...userData, token: response.token, workspaceId: response.workspaceId, rol: response.rol };
          localStorage.setItem('user', JSON.stringify(updatedUser));
        }
        localStorage.setItem('token', response.token);
      })
    );
  }

  logout(): void {
    this.userSignal.set(null);
    this.tokenSignal.set(null);
    this.currentWorkspaceSignal.set(null);
    this.currentRolSignal.set(null);
    this.tempTokenSignal.set(null);
    localStorage.removeItem('user');
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}
