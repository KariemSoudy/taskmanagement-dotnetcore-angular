import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Injectable } from '@angular/core';

import { AuthService } from '../services/auth.service';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class UnauthorizedInterceptor implements HttpInterceptor {
  constructor(private _authService: AuthService, private _router: Router) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError(err => {
        // if unauthorized, logout and navigate to login,
        // this can be replaced with refresh token mechanism to refresh token instead of log user out
        if (err.status === 401) {
          this._authService.logout();
          this._router.navigate(['/login']);
        }

        const error = err.error.message || err.statusText;
        return throwError(error);
      })
    );
  }
}
