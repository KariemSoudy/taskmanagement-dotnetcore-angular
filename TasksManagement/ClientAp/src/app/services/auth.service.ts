import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable, BehaviorSubject } from 'rxjs';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private userBehavior: BehaviorSubject<User>;
  public user: Observable<User>;
  private baseURL = 'http://localhost:32598/';

  constructor(private _http: HttpClient) {// , @Inject('BASE_URL') _baseUrl: string) {
    // this.baseURL = _baseUrl;
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
      .post<User>(this.baseURL + '/api/token/generate', { username, password })
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
