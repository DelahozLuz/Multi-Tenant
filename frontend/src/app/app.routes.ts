import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ProjectsComponent } from './projects/projects.component';
import { authGuard } from './core/guards/auth.guard';
export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'projects', component: ProjectsComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: '/login' }
];
