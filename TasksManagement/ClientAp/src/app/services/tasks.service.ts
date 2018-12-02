import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Task } from '../models/task';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TasksService {

  constructor(private _http: HttpClient) { }

  getAll() {
    return this._http
      .get<Task[]>(environment.BaseURL + '/api/tasks');
  }

  addNewTask(title: string, description: string) {
    return this._http
      .post<Task[]>(environment.BaseURL + '/api/tasks', { title, description });
  }

  deleteTask(taskID: number) {
    return this._http
      .delete<Task>(environment.BaseURL + '/api/tasks/' + taskID);
  }

  finishTask(taskID: number) {
    return this._http
      .put<Task>(environment.BaseURL + '/api/tasks/' + taskID, { completed: true });
  }

  assignTask(taskID: number, userID: string) {
    return this._http
      .put<Task>(environment.BaseURL + '/api/tasks/' + taskID, { assignedToUser: { id: userID } });
  }
}
