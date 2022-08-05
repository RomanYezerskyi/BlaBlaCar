import { NgModule } from '@angular/core';
import { Router, RouterModule, Routes } from '@angular/router';
import { AddTripComponent } from './components/user/add-trip-layout/add-trip/add-trip.component';
import { DialogBookingConfirmationComponent } from './components/user/dialog-booking-confirmation/dialog-booking-confirmation.component';
import { HomeComponent } from './components/user/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { SearchTripComponent } from './components/user/search-trip/search-trip.component';
import { TripPageInfoComponent } from './components/user/trip-page-info/trip-page-info.component';
import { AuthGuard } from './guards/auth.guard';
import { AdminPageComponent } from './components/admin/admin-page/admin-page.component';
import { RolesComponent } from './components/admin/roles/roles.component';
import { AddTripLayoutComponent } from './components/user/add-trip-layout/add-trip-layout.component';
import { AddAvailableSeatsComponent } from './components/user/add-trip-layout/add-available-seats/add-available-seats.component';

const routes: Routes = [
  { path: "" , redirectTo: "/app-root", pathMatch: "full"},
  { path: 'login', component: LoginComponent},
  { path: 'register', component: RegisterComponent},
  { path: 'search', component: SearchTripComponent},
 
  { path: 'trip-page-info/:id', component: TripPageInfoComponent, canActivate: [AuthGuard] },
  // { path: '', component: DialogBookingConfirmationComponent, canActivate: [AuthGuard] },
  { path: 'home', component: HomeComponent  /*, canActivate: [AuthGuard]*/},

  {
    path: 'admin', component: AdminPageComponent, canActivate: [AuthGuard],
    children: [
      { path: 'roles', component: RolesComponent , canActivate: [AuthGuard]},
    ]
  },
  { path: 'add-trip', component: AddTripLayoutComponent, 
    children:[
      { path: 'add', component: AddTripComponent, canActivate: [AuthGuard]},
      { path: 'add-seats', component: AddAvailableSeatsComponent, canActivate: [AuthGuard]},
    ]
  },

];
  

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
