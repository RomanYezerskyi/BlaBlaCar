import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
// import { LoginComponent } from './components/login/login.component';
// import { HomeComponent } from './components/user/home/home.component';
import { JwtModule } from "@auth0/angular-jwt";
import { AuthGuard } from './core/guards/auth-guard/auth.guard';
// import { SearchTripComponent } from './components/user/search-trip/search-trip.component';
// import { AddTripComponent } from './components/user/add-trip-layout/add-trip/add-trip.component';
// import { RegisterComponent } from './components/register/register.component';
// import { NavbarComponent } from './components/navbar/navbar.component';
// import { TripPageInfoComponent } from './components/user/trip-page-info/trip-page-info.component';
// import { DialogBookingConfirmationComponent } from './components/user/trip-page-info/dialog-booking-confirmation/dialog-booking-confirmation.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatNativeDateModule, MatRippleModule } from '@angular/material/core';
import { MatDialogModule } from '@angular/material/dialog';
// import { AddTripLayoutComponent } from './components/user/add-trip-layout/add-trip-layout.component';
// import { AddCarComponent } from './components/user/add-car/add-car.component';
// import { AddAvailableSeatsComponent } from './components/user/add-trip-layout/add-available-seats/add-available-seats.component';
import { RequestDrivingLicenseComponent } from './modules/user/user-profile/request-driving-license/request-driving-license.component';
// import { AdminPageComponent } from './components/admin-page/admin-page.component';
// import { RolesComponent } from './components/admin-page/roles/roles.component';
// import { UsersRequestsComponent } from './components/admin-page/users-requests/users-requests.component';
// import { UserRequestInfoComponent } from './components/admin-page/user-request-info/user-request-info.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { UserProfileComponent } from './modules/user/user-profile/user-profile.component';
// import { UserBookedTripsComponent } from './components/user/user-information/user-booked-trips/user-booked-trips.component';
// import { UserInformationComponent } from './components/user/user-information/user-information.component';
// import { UserCarsComponent } from './components/user/user-cars/user-cars.component';
// import { UserTripsComponent } from './components/user/user-information/user-trips/user-trips.component';
// import { InfoPageComponent } from './components/info-page/info-page.component';
// import { NotificationsComponent } from './components/notifications/notifications.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { NgxMatFileInputModule } from '@angular-material-components/file-input';
// import { SearchFormComponent } from './components/search-form/search-form.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
// import { SortingPanelComponent } from './components/user/search-trip/sorting-panel/sorting-panel.component';
import { MatRadioModule } from '@angular/material/radio';
// import { TripsComponent } from './components/user/search-trip/trips/trips.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
// import { MainComponent } from './components/admin-page/main/main.component';
// import { TopUsersListComponent } from './components/admin-page/main/top-users-list/top-users-list.component';
// import { AdminNotificationsComponent } from './components/admin-page/main/admin-notifications/admin-notifications.component';
import { AdminGuard } from './core/guards/admin-guard/admin.guard';
import { PageAccessGuard } from './guards/page-access/page-access.guard';
// import { CreateNotificationDialogComponent } from './components/create-notification-dialog/create-notification-dialog.component';
// import { AdminPanelComponent } from './components/admin-page/admin-panel/admin-panel.component';
// import { ChartsComponent } from './components/admin-page/charts/charts.component';
// import { UsersManagementComponent } from './components/admin-page/users-management/users-management.component';
// import { ChatLayoutComponent } from './components/chat-layout/chat-layout.component';
// import { ChatListComponent } from './components/chat-layout/chat-list/chat-list.component';
// import { ChatComponent } from './components/chat-layout/chat/chat.component';
// import { AdministratorsComponent } from './components/admin-page/administrators/administrators.component';
import { UserGuard } from './core/guards/user-guard/user.guard';
import { EditModalDialogComponent } from './modules/user/user-profile/edit-modal-dialog/edit-modal-dialog.component';
// import { EditCarModalDialogComponent } from './components/user/user-cars/edit-car-modal-dialog/edit-car-modal-dialog.component';
import { LightgalleryModule } from 'lightgallery/angular';
// import { ImagesGalleryComponent } from './components/images-gallery/images-gallery.component';
import { AlertsComponent } from './modules/shared/components/alerts/alerts.component';
import { UserService } from './core/services/user-service/user.service';
// import { MapsAutocompleteComponent } from './components/maps-autocomplete/maps-autocomplete.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatTooltipModule } from '@angular/material/tooltip';
import { SharedModule } from './modules/shared/shared.module';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
// import { ImagesGalleryComponent } from './components/images-gallery/images-gallery.component';
export function tokenGetter() {
  return localStorage.getItem("jwt");
}

@NgModule({
  declarations: [
    AppComponent,
    // LoginComponent,
    // HomeComponent,
    // SearchTripComponent,
    // AddTripComponent,
    // RegisterComponent,
    // NavbarComponent,
    // TripPageInfoComponent,
    // DialogBookingConfirmationComponent,
    // AddTripLayoutComponent,
    // // AddCarComponent,
    // AddAvailableSeatsComponent,
    // RequestDrivingLicenseComponent,
    // AdminPageComponent,
    // RolesComponent,
    // UsersRequestsComponent,
    // UserRequestInfoComponent,
    // UserProfileComponent,
    // UserBookedTripsComponent,
    // UserInformationComponent,
    // UserCarsComponent,
    // UserTripsComponent,
    // InfoPageComponent,
    // NotificationsComponent,
    // SearchFormComponent,

    // MainComponent,
    // TopUsersListComponent,
    // AdminNotificationsComponent,
    // CreateNotificationDialogComponent,
    // AdminPanelComponent,
    // ChartsComponent,
    // UsersManagementComponent,
    // ChatLayoutComponent,
    // ChatListComponent,
    // ChatComponent,
    // AdministratorsComponent,
    // EditModalDialogComponent,
    // EditCarModalDialogComponent,
    // ImagesGalleryComponent,

    // MapsAutocompleteComponent,

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
    InfiniteScrollModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatRippleModule,
    NgxMatFileInputModule,
    MatNativeDateModule,
    MatDatepickerModule,
    MatRadioModule,
    MatProgressSpinnerModule,
    LightgalleryModule,
    MatAutocompleteModule,
    MatTooltipModule

  ],
  providers: [AuthGuard, AdminGuard, PageAccessGuard, UserGuard, UserService],
  bootstrap: [AppComponent],
  // entryComponents: [/*DialogBookingConfirmationComponent*/
  //   /*CreateNotificationDialogComponent*/, /*EditModalDialogComponent,*/ /*EditCarModalDialogComponent,*/ /*RequestDrivingLicenseComponent*/],
  exports: [
    // MatToolbarModule,
    // MatButtonModule,
    // MatIconModule,
    // MatFormFieldModule,
    // MatInputModule,
    // MatSelectModule,
    // MatRippleModule,
  ]
})
export class AppModule { }
