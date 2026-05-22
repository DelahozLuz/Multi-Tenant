import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  const isAuthRequest = req.url.includes('/api/Auth/login') ||
                        req.url.includes('/api/Auth/token')                   

  if (isAuthRequest) {
    return next(req);
  }

  const token = authService.token();

  if (token) {
    const authReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
    return next(authReq);
  }

  return next(req);
};
