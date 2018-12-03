import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthTokenInterceptor } from 'src/app/interceptors/auth-token.interceptor';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  constructor(
    private _authService: AuthService,
    private _route: ActivatedRoute,
    private _router: Router,
    private _fb: FormBuilder
  ) { }

  loginForm: FormGroup = this._fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  });

  errorMessage: String;
  year: Number = new Date().getFullYear();

  returnUrl: string;

  ngOnInit() {
    this.returnUrl = this._route.snapshot.queryParams['returnUrl'] || '/';
  }

  loginUser(event: Event) {
    event.preventDefault();
    this.errorMessage = '';

    if (this.loginForm.valid) {
      this._authService.login(this.loginForm.value.username, this.loginForm.value.password)
        .subscribe(
          response => {
            if (this.returnUrl) {
              this._router.navigate([this.returnUrl]);
            } else {
              this._router.navigate(['/']);
            }
          },
        errorResponse => {
            this.errorMessage = errorResponse;
          }
        );
    }
  }
}
