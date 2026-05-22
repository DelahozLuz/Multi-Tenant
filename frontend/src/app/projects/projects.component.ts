import { Component, inject, OnInit, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';

import { AuthService } from '../core/services/auth.service';
import { Project } from '../core/models';
import { ProjectService } from '../core/services/project.service';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatToolbarModule,
    MatCardModule,
    MatButtonModule,
    MatListModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatSnackBarModule,
    MatTooltipModule
  ],
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css']
})
export class ProjectsComponent implements OnInit {
  authService = inject(AuthService);
  private projectService = inject(ProjectService);
  private fb = inject(FormBuilder);
  private snackBar = inject(MatSnackBar);
  projects: Project[] = [];
  form: FormGroup = this.fb.group({ nombre: ['', Validators.required] });

  canCreate = computed(() => {
    const rol = this.authService.currentRol();
    return rol === 'Admin' || rol === 'Editor';
  });

  canDelete = computed(() => {
    const rol = this.authService.currentRol();
    return rol === 'Admin';
  });

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.projectService.getProjects().subscribe({
      next: (data) => this.projects = data,
      error: () => this.snackBar.open('Error al cargar proyectos', 'Cerrar', { duration: 3000 })
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.projectService.createProject(this.form.value.nombre).subscribe({
      next: (project) => {
        this.projects.push(project);
        this.form.reset();
        this.snackBar.open('Proyecto creado', 'Cerrar', { duration: 3000 });
      },
      error: (err) => this.snackBar.open(err.status === 403 ? 'Sin permisos' : 'Error al crear', 'Cerrar', { duration: 3000 })
    });
  }

  updateProject(project: Project): void {
    const newName = prompt('Nuevo nombre del proyecto:', project.nombre);
    if (newName === null || newName.trim() === '') return;

    this.projectService.updateProject(project.id, newName.trim()).subscribe({
      next: (updatedProject) => {
        const index = this.projects.findIndex(p => p.id === updatedProject.id);
        if (index !== -1) { this.projects[index] = updatedProject; }
        this.snackBar.open('Proyecto actualizado', 'Cerrar', { duration: 3000 });
      }
    });
  }

  deleteProject(project: Project): void {
    if (!confirm(`¿Estás seguro?`)) return;
    this.projectService.deleteProject(project.id).subscribe({
      next: () => {
        this.projects = this.projects.filter(p => p.id !== project.id);
        this.snackBar.open('Eliminado', 'Cerrar', { duration: 3000 });
      }
    });
  }

  logout(): void { this.authService.logout(); }
}
