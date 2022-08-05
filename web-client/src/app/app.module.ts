import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from "@angular/common/http";
import { FormsModule,ReactiveFormsModule } from "@angular/forms";
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/user/home/home.component';
import { JwtModule } from "@auth0/angular-jwt";
import { AuthGuard } from './guards/auth.guard';
import { SearchTripComponent } from './components/user/search-trip/search-trip.component';
import { AddTripComponent } from './components/user/add-trip-layout/add-trip/add-trip.component';
import { RegisterComponent } from './components/register/register.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { TripPageInfoComponent } from './components/user/trip-page-info/trip-page-info.component';
import { DialogBookingConfirmationComponent } from './components/user/dialog-booking-confirmation/dialog-booking-confirmation.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDialogModule } from '@angular/material/dialog';
import { AdminPageComponent } from './components/admin/admin-page/admin-page.component';
import { RolesComponent } from './components/admin/roles/roles.component';
import { AddTripLayoutComponent } from './components/user/add-trip-layout/add-trip-layout.component';
import { AddCarComponent } from './components/user/add-trip-layout/add-car/add-car.component';
import { AddAvailableSeatsComponent } from './components/user/add-trip-layout/add-available-seats/add-available-seats.component';

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
    AdminPageComponent,
    RolesComponent,
    AddTripLayoutComponent,
    AddCarComponent,
    AddAvailableSeatsComponent,
   
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
    MatDialogModule
  ],
  providers: [AuthGuard],
  bootstrap: [AppComponent],
  entryComponents: [DialogBookingConfirmationComponent]
})
export class AppModule { }
