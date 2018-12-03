import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { AuthService } from './services/auth.service';
import { AuthGuard } from './guards/auth.guard';
import { NoAuthGuard } from './guards/no-auth.guard';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AuthTokenInterceptor } from './interceptors/auth-token.interceptor';
import { TasksService } from './services/tasks.service';
import { UnauthorizedInterceptor } from './interceptors/unauthorized.interceptor';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { TasksComponent } from './components/tasks/tasks.component';
import { MaterialsModule } from './materials.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UsersService } from './services/users.service';
import { AssigntoDialogComponent } from './components/tasks/assignto-dialog/assignto-dialog.component';
import { MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material';

@NgModule({
  declarations: [AppComponent, HomeComponent, LoginComponent, TasksComponent, AssigntoDialogComponent],
  imports: [BrowserModule, FormsModule, ReactiveFormsModule, AppRoutingModule, HttpClientModule, MaterialsModule, BrowserAnimationsModule],
  providers: [
    AuthService,
    TasksService,
    UsersService,
    AuthGuard,
    NoAuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthTokenInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: UnauthorizedInterceptor,
      multi: true
    },
    { provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: { duration: 5000 } }
  ],
  bootstrap: [AppComponent],
  entryComponents: [AssigntoDialogComponent]
})
export class AppModule { }
