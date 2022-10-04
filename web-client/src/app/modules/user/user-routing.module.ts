import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/core/guards/auth-guard/auth.guard';
import { AddCarComponent } from './add-car/add-car.component';
import { TripPageInfoComponent } from './trip-page-info/trip-page-info.component';
import { UserBookedTripsComponent } from './user-information/user-booked-trips/user-booked-trips.component';
import { UserInformationComponent } from './user-information/user-information.component';
import { UserTripsComponent } from './user-information/user-trips/user-trips.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserComponent } from './user.component';

const routes: Routes = [
  {
    path: '', component: UserComponent,
    children: [
      { path: 'profile', component: UserProfileComponent, canActivate: [AuthGuard] },
      {
        path: 'user-information', component: UserInformationComponent, canActivate: [AuthGuard],
        children: [
          { path: '', pathMatch: 'full', redirectTo: '/user-information/user-trips' },
          { path: 'booked-trips', component: UserBookedTripsComponent, canActivate: [AuthGuard], },
          { path: 'user-trips', component: UserTripsComponent, canActivate: [AuthGuard], },
        ]
      },
      { path: 'add-car', component: AddCarComponent, canActivate: [AuthGuard] },
      { path: 'trip-page-info/:id', component: TripPageInfoComponent, canActivate: [AuthGuard] },
    ]
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
