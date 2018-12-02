import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { User } from '../models/user';

@Injectable()
export class AuthTokenInterceptor implements HttpInterceptor {
  constructor(private _authService: AuthService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // skip for login page
    if (req.url.toLowerCase().indexOf('api/token/generate') !== -1) {
      return next.handle(req);
    }

    const user: User = this._authService.getLoggedInUser();

    if (user && user.token) {
      // append token to request
      const request = req.clone({
        headers: req.headers.set('Authorization', 'Bearer ' + user.token)
      });
      return next.handle(request);
    } else {
      return next.handle(req);
    }
  }
}
