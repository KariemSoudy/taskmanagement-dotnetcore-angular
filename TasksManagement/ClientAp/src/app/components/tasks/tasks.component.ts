import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { TasksService } from 'src/app/services/tasks.service';
import { Task } from 'src/app/models/task';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { _CdkColumnDefBase } from '@angular/cdk/table';
import { AuthService } from 'src/app/services/auth.service';
import { User } from 'src/app/models/user';
import { UsersService } from 'src/app/services/users.service';
import { MatDialog, MatSnackBar, MatTableDataSource, MatPaginator } from '@angular/material';
import { AssigntoDialogComponent } from './assignto-dialog/assignto-dialog.component';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css']
})
export class TasksComponent implements OnInit {
  tasks: Task[];
  user: User;
  newFormVisible = false;
  isAdmin = false;
  isSupport = false;

  constructor(
    private _tasksService: TasksService,
    private _fb: FormBuilder,
    private _authService: AuthService,
    public dialog: MatDialog,
    public snackBar: MatSnackBar) { }

  newForm: FormGroup = this._fb.group({
    title: ['', Validators.required],
    description: ['']
  });

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


  /**
   * form methods
   */
  showNewForm() {
    this.newFormVisible = this.isSupport;
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

  closeNewForm() {
    this.newFormVisible = false;
  }

  showMessage(message: string, isSuccess: boolean) {
    this.snackBar.open(message, 'dismiss', { panelClass: [(isSuccess === true ? 'green' : 'red') + '-snackbar'] });
  }


  /**
   * api tasks methods
   */
  getAllTasks() {
    this._tasksService.getAll().subscribe(tasks => {
      this.tasks = tasks;
    });
  }

  addTask() {

    if (this.newForm.valid) {
      this._tasksService.addNewTask(this.newForm.value.title, this.newForm.value.description)
        .subscribe(task => {
          this.getAllTasks();

          this.newForm.reset();

          this.newFormVisible = false;

          this.showMessage(`Task "${task.title}" created`, true);
        });
    }
  }

  deleteTask(taskID: number) {

    if (confirm('Are you sure?')) {
      this._tasksService.deleteTask(taskID)
        .subscribe(task => {
          this.getAllTasks();
          if (task) {
            this.showMessage(`Task "${task.title}" deleted`, true);
          }
        }, error => {
          this.showMessage(error, false);
        });
    }
  }

  finishTask(taskID: number) {

    if (confirm('Are you sure?')) {
      this._tasksService.finishTask(taskID)
        .subscribe(task => {
          this.getAllTasks();
          if (task) {
            this.showMessage(`Task "${task.title}" completed`, true);
          }
        }, error => {
          this.showMessage(error, false);
        });
    }
  }

  assignTask(taskID: number, userID: string) {

    this._tasksService.assignTask(taskID, userID)
      .subscribe(task => {
        this.getAllTasks();

        if (task) {
          if (task.assignedToUser) {
            this.showMessage(`Task "${task.title}" assigned to ${task.assignedToUser.username}`, true);
          } else {
            this.showMessage(`Task "${task.title}" released`, true);
          }

        }
      }, error => {
        this.showMessage(error, false);
      });
  }
}
