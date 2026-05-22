import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Project } from '../models';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ProjectService {
  private readonly baseUrl = `${environment.apiUrl}/Projects`;
  private http = inject(HttpClient);

  getProjects(): Observable<Project[]> {
    return this.http.get<Project[]>(this.baseUrl);
  }

  createProject(nombre: string): Observable<Project> {
    return this.http.post<Project>(this.baseUrl, { nombre });
  }

  updateProject(id: number, nombre: string): Observable<Project> {
    return this.http.put<Project>(`${this.baseUrl}/${id}`, { nombre });
  }

  deleteProject(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
