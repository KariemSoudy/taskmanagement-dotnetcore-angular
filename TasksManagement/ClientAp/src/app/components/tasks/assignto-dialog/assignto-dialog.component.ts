import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Task } from 'src/app/models/task';
import { UsersService } from 'src/app/services/users.service';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-assignto-dialog',
  templateUrl: './assignto-dialog.component.html',
  styleUrls: ['./assignto-dialog.component.css']
})
export class AssigntoDialogComponent implements OnInit {

  constructor(
    private _usersService: UsersService,
    public dialogRef: MatDialogRef<AssigntoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Task) {

    if (!data.assignedToUser) {
      // dummy user
      data.assignedToUser = {
        id: '0',
        username: '',
        email: '',
        role: '',
        token: ''
      };
    }

    this.initialSelectedUserId = data.assignedToUser.id;
  }

  users: User[];
  assignButtonDisabled = true;
  initialSelectedUserId: string;

  onNoClick(): void {
    this.dialogRef.close(null);
  }

  getAllUsers() {
    this._usersService.getAll().subscribe(users => {
      this.users = users;
    });
  }

  ngOnInit() {
    this.getAllUsers();
  }

  onChange(value) {
    this.assignButtonDisabled = (value === this.initialSelectedUserId);
  }

}
