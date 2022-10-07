import { NgModule } from '@angular/core';
import { Router, RouterModule, Routes } from '@angular/router';
// import { HomeComponent } from './components/user/home/home.component';
// import { LoginComponent } from './components/login/login.component';
// import { RegisterComponent } from './components/register/register.component';
// import { SearchTripComponent } from './components/user/search-trip/search-trip.component';
// import { TripPageInfoComponent } from './components/user/trip-page-info/trip-page-info.component';
import { AuthGuard } from './core/guards/auth-guard/auth.guard';
// import { AdminPageComponent } from './components/admin-page/admin-page.component';
// import { RolesComponent } from './components/admin-page/roles/roles.component';
// import { AddTripLayoutComponent } from './components/user/add-trip-layout/add-trip-layout.component';
// import { AddAvailableSeatsComponent } from './components/user/add-trip-layout/add-available-seats/add-available-seats.component';
// import { AddCarComponent } from './components/user/add-car/add-car.component';
import { RequestDrivingLicenseComponent } from './modules/user/user-profile/request-driving-license/request-driving-license.component';
// import { UsersRequestsComponent } from './components/admin-page/users-requests/users-requests.component';
// import { UserRequestInfoComponent } from './components/admin-page/user-request-info/user-request-info.component';
import { UserProfileComponent } from './modules/user/user-profile/user-profile.component';
// import { UserBookedTripsComponent } from './components/user/user-information/user-booked-trips/user-booked-trips.component';
// import { UserInformationComponent } from './components/user/user-information/user-information.component';
// import { UserCarsComponent } from './components/user/user-cars/user-cars.component';
// import { UserTripsComponent } from './components/user/user-information/user-trips/user-trips.component';
import { PageAccessGuard } from './core/guards/page-access/page-access.guard';
// import { InfoPageComponent } from './components/info-page/info-page.component';
import { AdminGuard } from './core/guards/admin-guard/admin.guard';
// import { MainComponent } from './components/admin-page/main/main.component';
// import { ChartsComponent } from './components/admin-page/charts/charts.component';
// import { UsersManagementComponent } from './components/admin-page/users-management/users-management.component';
// import { ChatLayoutComponent } from './components/chat-layout/chat-layout.component';
// import { AdministratorsComponent } from './components/admin-page/administrators/administrators.component';
import { UserGuard } from './core/guards/user-guard/user.guard';
import { InfoPageComponent } from './modules/shared/components/info-page/info-page.component';

const routes: Routes = [
  { path: "", redirectTo: "user/home", pathMatch: "full" },
  // { path: 'login', component: LoginComponent },
  // { path: 'register', component: RegisterComponent },


  // { path: 'trip-page-info/:id', component: TripPageInfoComponent, canActivate: [AuthGuard] },
  // { path: 'home', component: HomeComponent  /*, canActivate: [AuthGuard]*/ },



  // { path: 'cars', component: UserCarsComponent, canActivate: [AuthGuard] },

  // { path: 'chat', component: ChatLayoutComponent, canActivate: [AuthGuard] },
  // { path: 'info', component: InfoPageComponent, canActivate: [AuthGuard] },
  // {
  //   path: 'admin', component: AdminPageComponent, canActivate: [AuthGuard],
  //   children: [
  //     { path: 'roles', component: RolesComponent, canActivate: [AuthGuard] },
  //     { path: 'requests/:id', component: UsersRequestsComponent, canActivate: [AuthGuard], },
  //     { path: 'requests-info/:id', component: UserRequestInfoComponent, canActivate: [AuthGuard] },
  //     { path: 'main-info', component: MainComponent, canActivate: [AuthGuard] },
  //     { path: 'charts', component: ChartsComponent, canActivate: [AuthGuard] },
  //     { path: 'users', component: UsersManagementComponent, canActivate: [AuthGuard] },
  //     { path: 'chat', component: ChatLayoutComponent, canActivate: [AuthGuard] },
  //     { path: 'admins', component: AdministratorsComponent, canActivate: [AuthGuard] },
  //   ]
  // },
  {
    path: 'user',
    loadChildren: () => import('./modules/user/user.module').then(m => m.UserModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'admin',
    loadChildren: () => import('./modules/admin-page/admin-page.module').then(m => m.AdminPageModule),
    canActivate: [AuthGuard, AdminGuard]
  },
  {
    path: 'auth',
    loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule)
  },
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
