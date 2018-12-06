import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  BASE_URL: string;

  constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.BASE_URL = baseUrl;
  }

  getAll() {
    return this._http
      .get<User[]>(this.BASE_URL + 'api/users');
  }
}
