import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Task } from '../models/task';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TasksService {
  BASE_URL: string;

  constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.BASE_URL = baseUrl;
  }

  getAll() {
    return this._http
      .get<Task[]>(this.BASE_URL + '/api/tasks');
  }

  addNewTask(title: string, description: string) {
    return this._http
      .post<Task[]>(this.BASE_URL + '/api/tasks', { title, description });
  }

  deleteTask(taskID: number) {
    return this._http
      .delete<Task>(this.BASE_URL + '/api/tasks/' + taskID);
  }

  finishTask(taskID: number) {
    return this._http
      .put<Task>(this.BASE_URL + '/api/tasks/' + taskID, { completed: true });
  }

  assignTask(taskID: number, userID: string) {
    return this._http
      .put<Task>(this.BASE_URL + '/api/tasks/' + taskID, { assignedToUser: { id: userID } });
  }
}
