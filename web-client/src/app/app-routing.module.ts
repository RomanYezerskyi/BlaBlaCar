import { NgModule } from '@angular/core';
import { Router, RouterModule, Routes } from '@angular/router';
import { AddTripComponent } from './components/user/add-trip-layout/add-trip/add-trip.component';
import { DialogBookingConfirmationComponent } from './components/user/trip-page-info/dialog-booking-confirmation/dialog-booking-confirmation.component';
import { HomeComponent } from './components/user/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { SearchTripComponent } from './components/user/search-trip/search-trip.component';
import { TripPageInfoComponent } from './components/user/trip-page-info/trip-page-info.component';
import { AuthGuard } from './guards/auth.guard';
import { AdminPageComponent } from './components/admin-page/admin-page.component';
import { RolesComponent } from './components/admin-page/roles/roles.component';
import { AddTripLayoutComponent } from './components/user/add-trip-layout/add-trip-layout.component';
import { AddAvailableSeatsComponent } from './components/user/add-trip-layout/add-available-seats/add-available-seats.component';
import { AddCarComponent } from './components/user/add-car/add-car.component';
import { RequestDrivingLicenseComponent } from './components/user/request-driving-license/request-driving-license.component';
import { UsersRequestsComponent } from './components/admin-page/users-requests/users-requests.component';
import { UserRequestInfoComponent } from './components/admin-page/user-request-info/user-request-info.component';
import { PendingComponent } from './components/admin-page/users-requests/pending/pending.component';
import { MainInfoComponent } from './components/admin-page/user-request-info/main-info/main-info.component';
import { UserProfileComponent } from './components/user/user-profile/user-profile.component';
import { UserBookedTripsComponent } from './components/user/user-information/user-booked-trips/user-booked-trips.component';
import { UserInformationComponent } from './components/user/user-information/user-information.component';
import { UserCarsComponent } from './components/user/user-cars/user-cars.component';

const routes: Routes = [
  { path: "", redirectTo: "/home", pathMatch: "full" },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'search', component: SearchTripComponent },

  { path: 'trip-page-info/:id', component: TripPageInfoComponent, canActivate: [AuthGuard] },
  { path: 'home', component: HomeComponent  /*, canActivate: [AuthGuard]*/ },

  {
    path: 'admin', component: AdminPageComponent, canActivate: [AuthGuard],
    children: [
      { path: 'roles', component: RolesComponent, canActivate: [AuthGuard] },
      {
        path: 'requests', component: UsersRequestsComponent, canActivate: [AuthGuard],
        children: [
          { path: 'pending', component: PendingComponent, canActivate: [AuthGuard] },


          {
            path: 'info/:id', component: UserRequestInfoComponent, canActivate: [AuthGuard],
            children: [
              { path: 'user', component: MainInfoComponent, canActivate: [AuthGuard] },
            ]
          }
        ]
      },

    ]
  },
  {
    path: 'add-trip', component: AddTripLayoutComponent, canActivate: [AuthGuard],
    children: [
      { path: 'add', component: AddTripComponent, canActivate: [AuthGuard] },
      { path: 'add-seats', component: AddAvailableSeatsComponent, canActivate: [AuthGuard] },
    ]
  },
  { path: 'add-car', component: AddCarComponent, canActivate: [AuthGuard] },
  { path: 'driving', component: RequestDrivingLicenseComponent, canActivate: [AuthGuard] },
  { path: 'cars', component: UserCarsComponent, canActivate: [AuthGuard] },
  { path: 'profile', component: UserProfileComponent, canActivate: [AuthGuard] },
  {
    path: 'user-information', component: UserInformationComponent, canActivate: [AuthGuard],
    children: [
      { path: 'booked-trips', component: UserBookedTripsComponent, canActivate: [AuthGuard], },


    ]
  },


];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
