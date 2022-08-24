import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/user/home/home.component';
import { JwtModule } from "@auth0/angular-jwt";
import { AuthGuard } from './guards/auth.guard';
import { SearchTripComponent } from './components/user/search-trip/search-trip.component';
import { AddTripComponent } from './components/user/add-trip-layout/add-trip/add-trip.component';
import { RegisterComponent } from './components/register/register.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { TripPageInfoComponent } from './components/user/trip-page-info/trip-page-info.component';
import { DialogBookingConfirmationComponent } from './components/user/trip-page-info/dialog-booking-confirmation/dialog-booking-confirmation.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDialogModule } from '@angular/material/dialog';
import { AddTripLayoutComponent } from './components/user/add-trip-layout/add-trip-layout.component';
import { AddCarComponent } from './components/user/add-car/add-car.component';
import { AddAvailableSeatsComponent } from './components/user/add-trip-layout/add-available-seats/add-available-seats.component';
import { RequestDrivingLicenseComponent } from './components/user/request-driving-license/request-driving-license.component';
import { AdminPageComponent } from './components/admin-page/admin-page.component';
import { RolesComponent } from './components/admin-page/roles/roles.component';
import { UsersRequestsComponent } from './components/admin-page/users-requests/users-requests.component';
import { UserRequestInfoComponent } from './components/admin-page/user-request-info/user-request-info.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { PendingComponent } from './components/admin-page/users-requests/pending/pending.component';
import { MainInfoComponent } from './components/admin-page/user-request-info/main-info/main-info.component';
import { DrivingLicenseRequestComponent } from './components/admin-page/user-request-info/driving-license-request/driving-license-request.component';
import { CarRequestsComponent } from './components/admin-page/user-request-info/car-requests/car-requests.component';
import { UserProfileComponent } from './components/user/user-profile/user-profile.component';
import { UserBookedTripsComponent } from './components/user/user-information/user-booked-trips/user-booked-trips.component';
import { UserInformationComponent } from './components/user/user-information/user-information.component';
import { UserCarsComponent } from './components/user/user-cars/user-cars.component';
import { UserTripsComponent } from './components/user/user-information/user-trips/user-trips.component';
import { InfoPageComponent } from './guards/info-page/info-page.component';
import { NotificationsComponent } from './components/notifications/notifications.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

export function tokenGetter() {
  return localStorage.getItem("jwt");
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    SearchTripComponent,
    AddTripComponent,
    RegisterComponent,
    NavbarComponent,
    TripPageInfoComponent,
    DialogBookingConfirmationComponent,
    AddTripLayoutComponent,
    AddCarComponent,
    AddAvailableSeatsComponent,
    RequestDrivingLicenseComponent,
    AdminPageComponent,
    RolesComponent,
    UsersRequestsComponent,
    UserRequestInfoComponent,
    PendingComponent,
    MainInfoComponent,
    DrivingLicenseRequestComponent,
    CarRequestsComponent,
    UserProfileComponent,
    UserBookedTripsComponent,
    UserInformationComponent,
    UserCarsComponent,
    UserTripsComponent,
    InfoPageComponent,
    NotificationsComponent,

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    FormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:6001", "localhost:5001"],
        disallowedRoutes: []
      }
    }),
    BrowserAnimationsModule,
    MatDialogModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    InfiniteScrollModule
  ],
  providers: [AuthGuard],
  bootstrap: [AppComponent],
  entryComponents: [DialogBookingConfirmationComponent, UserProfileComponent],
  exports: [
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
  ]
})
export class AppModule { }
