import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './user.component';
import { AuthGuard } from 'src/app/core/guards/auth-guard/auth.guard';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { SharedModule } from '../shared/shared.module';
import { UserCarsComponent } from './user-cars/user-cars.component';
import { EditCarModalDialogComponent } from './user-cars/edit-car-modal-dialog/edit-car-modal-dialog.component';
import { AddCarComponent } from './add-car/add-car.component';
import { UserTripsComponent } from './user-information/user-trips/user-trips.component';
import { UserBookedTripsComponent } from './user-information/user-booked-trips/user-booked-trips.component';
import { UserInformationComponent } from './user-information/user-information.component';
import { TripPageInfoComponent } from './trip-page-info/trip-page-info.component';
import { DialogBookingConfirmationComponent } from './trip-page-info/dialog-booking-confirmation/dialog-booking-confirmation.component';
import { SortingPanelComponent } from './search-trip/sorting-panel/sorting-panel.component';
import { TripsComponent } from './search-trip/trips/trips.component';
import { SearchTripComponent } from './search-trip/search-trip.component';
import { AddTripComponent } from './add-trip-layout/add-trip/add-trip.component';
import { AddTripLayoutComponent } from './add-trip-layout/add-trip-layout.component';
import { AddAvailableSeatsComponent } from './add-trip-layout/add-available-seats/add-available-seats.component';
import { PageAccessGuard } from 'src/app/core/guards/page-access/page-access.guard';
import { UserGuard } from 'src/app/core/guards/user-guard/user.guard';
import { RequestDrivingLicenseComponent } from './user-profile/request-driving-license/request-driving-license.component';
import { EditModalDialogComponent } from './user-profile/edit-modal-dialog/edit-modal-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatNativeDateModule, MatRippleModule } from '@angular/material/core';
import { NgxMatFileInputModule } from '@angular-material-components/file-input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatRadioModule } from '@angular/material/radio';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { LightgalleryModule } from 'lightgallery/angular';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UserNavbarComponent } from './user-navbar/user-navbar.component';



@NgModule({
  declarations: [
    UserComponent,
    UserTripsComponent,
    UserBookedTripsComponent,
    UserInformationComponent,
    UserProfileComponent,
    RequestDrivingLicenseComponent,
    UserCarsComponent,
    EditCarModalDialogComponent,
    AddCarComponent,
    TripPageInfoComponent,
    DialogBookingConfirmationComponent,
    SortingPanelComponent,
    TripsComponent,
    SearchTripComponent,
    AddTripComponent,
    AddTripLayoutComponent,
    AddAvailableSeatsComponent,
    EditModalDialogComponent,
    UserNavbarComponent,
  ],
  providers: [AuthGuard, PageAccessGuard, UserGuard],
  entryComponents: [
    DialogBookingConfirmationComponent,
    EditCarModalDialogComponent,
    RequestDrivingLicenseComponent,
    EditModalDialogComponent,
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    SharedModule,

    FormsModule,
    ReactiveFormsModule,
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

  ]
})
export class UserModule { }
