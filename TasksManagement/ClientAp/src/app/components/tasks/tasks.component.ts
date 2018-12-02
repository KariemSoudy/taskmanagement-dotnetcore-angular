import { Component, OnInit } from '@angular/core';
import { TasksService } from 'src/app/services/tasks.service';
import { Task } from 'src/app/models/task';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css']
})
export class TasksComponent implements OnInit {

  constructor(private _tasksService: TasksService) { }

  tasks: Task[];

  ngOnInit() {
    this._tasksService.getAll().subscribe(
      tasks => {
        this.tasks = tasks;
      }
    );
  }

}
