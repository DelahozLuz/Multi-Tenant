import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../core/services/auth.service';
import { WorkspaceRol } from '../core/models';



@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, MatSnackBarModule, MatProgressSpinnerModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  loading = signal(false);
  step1Complete = signal(false);
  email = signal('');
  workspaces = signal<WorkspaceRol[]>([]);
  tempToken = signal('');

  formStep1: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  formStep2: FormGroup = this.fb.group({
    workspaceId: ['', Validators.required]
  });

  ngOnInit(): void {
    this.authService.logout();
  }

  onStep1Submit(): void {
    if (this.formStep1.invalid) return;
    this.loading.set(true);

    this.authService.login(this.formStep1.value).subscribe({
      next: (response) => {
        this.loading.set(false);
        this.email.set(response.email);
        this.workspaces.set(response.workspaces);
        this.tempToken.set(response.tempToken);
        this.step1Complete.set(true);
      },
      error: (err: any) => {
        this.loading.set(false);
        const message = err.error?.message || 'Credenciales inválidas';
        this.snackBar.open(message, 'Cerrar', { duration: 4000 });
      }
    });
  }

  onStep2Submit(): void {
    if (this.formStep2.invalid) return;
    this.loading.set(true);

    const workspaceId = this.formStep2.value.workspaceId;
    const selectedWorkspace = this.workspaces().find(ws => ws.workspaceId === workspaceId);

    this.authService.getToken(workspaceId, this.tempToken()).subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['/projects']);
      },
      error: (err: any) => {
        this.loading.set(false);
        if (err.status === 401) {
          this.snackBar.open(
            `No tienes acceso al workspace "${selectedWorkspace?.nombreWorkspace}".`,
            'Cerrar',
            { duration: 5000 }
          );
        } else {
          this.snackBar.open('Error al obtener token', 'Cerrar', { duration: 4000 });
        }
      }
    });
  }

  goBack(): void {
    this.step1Complete.set(false);
    this.workspaces.set([]);
    this.tempToken.set('');
    this.formStep1.reset();
  }
}
