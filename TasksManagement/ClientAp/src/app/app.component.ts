import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from './models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(private _authService: AuthService, private _router: Router) { }

  user$: Observable<User>;

  ngOnInit(): void {
    // this should be an observable because it will not refresh to load new value, it will subscribe to observable to change based on value
    this.user$ = this._authService.user;
  }

  logout(event: Event) {
    event.preventDefault();

    this._authService.logout();

    this._router.navigate(['login']);
  }
}
