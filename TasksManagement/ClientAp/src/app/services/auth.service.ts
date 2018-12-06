import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable, BehaviorSubject } from 'rxjs';
import { User } from '../models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public user: Observable<User>;
  userBehavior: BehaviorSubject<User>;
  BASE_URL: string;

  constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.BASE_URL = baseUrl;

    this.userBehavior = new BehaviorSubject<User>(
      JSON.parse(localStorage.getItem('user'))
    );

    this.user = this.userBehavior.asObservable();
  }

  public getLoggedInUser(): User {
    return this.userBehavior.value;
  }

  login(username: string, password: string) {
    return this._http
      .post<User>(this.BASE_URL + 'api/token/generate', { username, password })
      .pipe(
        map(user => {
          if (user && user.token) {
            localStorage.setItem('user', JSON.stringify(user));
            this.userBehavior.next(user);
          }
          return user;
        })
      );
  }

  logout() {
    this.userBehavior.next(null);

    localStorage.removeItem('user');
  }
}
