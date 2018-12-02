import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable, BehaviorSubject } from 'rxjs';
import { Task } from '../models/task';
import { environment } from 'src/environments/environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class TasksService {

  // private userBehavior: BehaviorSubject<User>;
  // public user: Observable<User>;

  constructor(private _http: HttpClient, private _authService: AuthService) {
    // this.userBehavior = new BehaviorSubject<User>(
    // JSON.parse(localStorage.getItem('user'))
    // );

    // this.user = this.userBehavior.asObservable();
  }

  getAll() {
    return this._http
      .get<Task[]>(environment.BaseURL + '/api/tasks');
  }
}
