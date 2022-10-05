import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/core/guards/auth-guard/auth.guard';
import { ChatLayoutComponent } from '../shared/components/chat-layout/chat-layout.component';
import { InfoPageComponent } from '../shared/components/info-page/info-page.component';
import { AdminPageComponent } from './admin-page.component';
import { AdministratorsComponent } from './administrators/administrators.component';
import { ChartsComponent } from './charts/charts.component';
import { MainComponent } from './main/main.component';
import { RolesComponent } from './roles/roles.component';
import { UserRequestInfoComponent } from './user-request-info/user-request-info.component';
import { UsersManagementComponent } from './users-management/users-management.component';
import { UsersRequestsComponent } from './users-requests/users-requests.component';

const routes: Routes = [

  { path: '', component: AdminPageComponent, },
  {
    path: 'admin', component: AdminPageComponent, canActivate: [AuthGuard],
    children: [
      { path: 'roles', component: RolesComponent, canActivate: [AuthGuard] },
      { path: 'requests/:id', component: UsersRequestsComponent, canActivate: [AuthGuard], },
      { path: 'requests-info/:id', component: UserRequestInfoComponent, canActivate: [AuthGuard] },
      { path: 'main-info', component: MainComponent, canActivate: [AuthGuard] },
      { path: 'charts', component: ChartsComponent, canActivate: [AuthGuard] },
      { path: 'users', component: UsersManagementComponent, canActivate: [AuthGuard] },
      { path: 'chat', component: ChatLayoutComponent, canActivate: [AuthGuard] },
      { path: 'admins', component: AdministratorsComponent, canActivate: [AuthGuard] },
    ]
  },
  { path: 'info', component: InfoPageComponent, canActivate: [AuthGuard] },
  // { path: 'chat', component: ChatLayoutComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminPageRoutingModule { }
