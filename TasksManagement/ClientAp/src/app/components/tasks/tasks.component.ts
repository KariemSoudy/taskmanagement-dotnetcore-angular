import { Component, OnInit } from '@angular/core';
import { TasksService } from 'src/app/services/tasks.service';
import { Task } from 'src/app/models/task';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { _CdkColumnDefBase } from '@angular/cdk/table';
import { AuthService } from 'src/app/services/auth.service';
import { User } from 'src/app/models/user';
import { UsersService } from 'src/app/services/users.service';
import { MatDialog } from '@angular/material';
import { AssigntoDialogComponent } from './assignto-dialog/assignto-dialog.component';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css']
})
export class TasksComponent implements OnInit {

  tasks: Task[];
  errorMessage: string;
  successMessage: string;
  newFormVisible = false;
  isAdmin = false;
  isSupport = false;
  user: User;
  selectedOption: string;

  constructor(
    private _tasksService: TasksService,
    private _usersService: UsersService,
    private _fb: FormBuilder,
    private _authService: AuthService,
    public dialog: MatDialog
  ) { }


  newForm: FormGroup = this._fb.group({
    title: ['', Validators.required],
    description: ['']
  });

  showNewForm() {
    this.newFormVisible = this.isSupport;
    this.clearMessages();
  }

  clearMessages() {
    this.successMessage = '';
    this.errorMessage = '';
  }

  openDialog(task: Task): void {
    const dialogRef = this.dialog.open(AssigntoDialogComponent, {
      width: '250px',
      data: task
    });

    dialogRef.afterClosed().subscribe(userID => {
      if (userID) {
        this.assignTask(task.id, userID);
      }
    });
  }


  ngOnInit() {

    this.getAllTasks();

    this.user = this._authService.getLoggedInUser();


    if (this.user && this.user.role) {
      if (this.user.role.toLowerCase() === 'support') {
        this.isAdmin = false;
        this.isSupport = true;
      } else if (this.user.role.toLowerCase() === 'admin') {
        this.isAdmin = true;
        this.isSupport = false;
      }
    }
  }

  getAllTasks() {
    this._tasksService.getAll().subscribe(tasks => {
      this.tasks = tasks;
    });
  }

  addTask() {
    this.clearMessages();

    this._tasksService.addNewTask(this.newForm.value.title, this.newForm.value.description)
      .subscribe(tasks => {
        this.tasks = tasks;

        this.newForm.reset();

        this.newFormVisible = false;
        this.successMessage = 'Task created';
      });
  }

  deleteTask(taskID: number) {
    this.clearMessages();

    if (confirm('Are you sure?')) {
      this._tasksService.deleteTask(taskID)
        .subscribe(task => {
          this.getAllTasks();
          if (task) {
            this.successMessage = `Task ${task.title} deleted`;
          }
        }, error => {
          this.errorMessage = error;
        });
    }
  }

  finishTask(taskID: number) {
    this.clearMessages();

    if (confirm('Are you sure?')) {
      this._tasksService.finishTask(taskID)
        .subscribe(task => {
          this.getAllTasks();
          if (task) {
            this.successMessage = `Task ${task.title} completed`;
          }
        }, error => {
          this.errorMessage = error;
        });
    }
  }

  assignTask(taskID: number, userID: string) {
    this.clearMessages();

    this._tasksService.assignTask(taskID, userID)
      .subscribe(task => {
        this.getAllTasks();

        if (task) {
          if (task.assignedToUser) {
            this.successMessage = `Task ${task.title} assigned to ${task.assignedToUser.username}`;
          } else {
            this.successMessage = `Task ${task.title} released`;
          }

        }
      }, error => {
        this.errorMessage = error;
      });
  }
}
