import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { AuthGuard } from './guards/auth.guard';
import { LoginComponent } from './components/login/login.component';
import { NoAuthGuard } from './guards/no-auth.guard';
import { TasksComponent } from './components/tasks/tasks.component';

const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'tasks', component: TasksComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent, canActivate: [NoAuthGuard] }
  // { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  // {
  //   path: 'horizontal/:id',
  //   component: HorizontalAnalysisComponent,
  //   canActivate: [AuthGuard]
  // },
  // {
  //   path: 'vertical/:id',
  //   component: ProfileComponent,
  //   canActivate: [AuthGuard]
  // },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
