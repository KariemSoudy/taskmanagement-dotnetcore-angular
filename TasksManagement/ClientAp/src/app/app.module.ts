import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { AuthService } from './services/auth.service';
import { AuthGuard } from './guards/auth.guard';
import { NoAuthGuard } from './guards/no-auth.guard';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AuthTokenInterceptor } from './interceptors/auth-token.interceptor';
import { TasksService } from './services/tasks.service';
import { UnauthorizedInterceptor } from './interceptors/unauthorized.interceptor';
import { ReactiveFormsModule } from '@angular/forms';
import { InjectionToken } from '@angular/core';
import { TasksComponent } from './components/tasks/tasks.component';
import { MaterialsModule } from './materials.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

export const BASE_URL = new InjectionToken<string>('BASE_URL');

@NgModule({
  declarations: [AppComponent, NavMenuComponent, HomeComponent, LoginComponent, TasksComponent],
  imports: [BrowserModule, ReactiveFormsModule, AppRoutingModule, HttpClientModule, MaterialsModule, BrowserAnimationsModule],
  providers: [
    AuthService,
    TasksService,
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
    { provide: BASE_URL, useValue: document.getElementsByTagName('base')[0].href }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
