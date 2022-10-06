import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/core/guards/auth-guard/auth.guard';
import { UserGuard } from 'src/app/core/guards/user-guard/user.guard';
import { PageAccessGuard } from 'src/app/guards/page-access/page-access.guard';
import { ChatLayoutComponent } from '../shared/components/chat-layout/chat-layout.component';
import { InfoPageComponent } from '../shared/components/info-page/info-page.component';
import { AddCarComponent } from './add-car/add-car.component';
import { AddTripLayoutComponent } from './add-trip-layout/add-trip-layout.component';
// import { HomeComponent } from './home/home.component';
import { SearchTripComponent } from './search-trip/search-trip.component';
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
      { path: '', redirectTo: '/user/search', pathMatch: 'full' },
      { path: 'profile', component: UserProfileComponent, canActivate: [AuthGuard, UserGuard] },
      {
        path: 'user-information', component: UserInformationComponent, canActivate: [AuthGuard, UserGuard],
        children: [
          { path: '', pathMatch: 'full', redirectTo: '/user-information/user-trips' },
          { path: 'booked-trips', component: UserBookedTripsComponent, canActivate: [AuthGuard, UserGuard], },
          { path: 'user-trips', component: UserTripsComponent, canActivate: [AuthGuard, UserGuard], },
        ]
      },
      { path: 'add-car', component: AddCarComponent, canActivate: [AuthGuard, UserGuard] },
      { path: 'trip-page-info/:id', component: TripPageInfoComponent, canActivate: [AuthGuard, UserGuard] },
      { path: 'search', component: SearchTripComponent, canActivate: [AuthGuard, UserGuard] },

      { path: 'add-trip', component: AddTripLayoutComponent, canActivate: [PageAccessGuard, UserGuard] },
      { path: 'info', component: InfoPageComponent, canActivate: [AuthGuard, UserGuard] },
      { path: 'chat', component: ChatLayoutComponent, canActivate: [AuthGuard, UserGuard] },
      { path: 'info', component: InfoPageComponent, canActivate: [AuthGuard, UserGuard] },
      { path: '**', redirectTo: 'info' }
    ]
  },


];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
