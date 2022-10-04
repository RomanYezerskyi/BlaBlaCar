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



@NgModule({
  declarations: [
    UserComponent,
    UserTripsComponent,
    UserBookedTripsComponent,
    UserInformationComponent,
    UserProfileComponent,
    UserCarsComponent,
    EditCarModalDialogComponent,
    AddCarComponent,
    TripPageInfoComponent,
    DialogBookingConfirmationComponent
  ],
  providers: [AuthGuard],
  entryComponents: [DialogBookingConfirmationComponent, EditCarModalDialogComponent],
  imports: [
    CommonModule,
    UserRoutingModule,
    SharedModule,

  ]
})
export class UserModule { }
