import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/core/guards/auth-guard/auth.guard';
import { UserGuard } from 'src/app/core/guards/user-guard/user-exist.guard';
import { PageAccessGuard } from 'src/app/core/guards/page-access/page-access.guard';
import { ChatLayoutComponent } from '../shared/components/chat-layout/chat-layout.component';
import { InfoPageComponent } from '../shared/components/info-page/info-page.component';
import { AddCarComponent } from './add-car/add-car.component';
import { AddTripLayoutComponent } from './add-trip-layout/add-trip-layout.component';
import { SearchTripComponent } from './search-trip/search-trip.component';
import { TripPageInfoComponent } from './trip-page-info/trip-page-info.component';
import { UserBookedTripsComponent } from './user-information/user-booked-trips/user-booked-trips.component';
import { UserInformationComponent } from './user-information/user-information.component';
import { UserTripsComponent } from './user-information/user-trips/user-trips.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserComponent } from './user.component';
import { IsUserGuard } from 'src/app/core/guards/user-guard/isuser.guard';

const routes: Routes = [
  {
    path: '', component: UserComponent,
    children: [
      { path: '', redirectTo: '/user/search', pathMatch: 'full' },
      { path: 'profile', component: UserProfileComponent, canActivate: [AuthGuard, UserGuard, IsUserGuard] },
      {
        path: 'user-information', component: UserInformationComponent, canActivate: [AuthGuard, UserGuard, IsUserGuard],
        children: [
          { path: '', pathMatch: 'full', redirectTo: '/user-information/user-trips' },
          { path: 'booked-trips', component: UserBookedTripsComponent, canActivate: [AuthGuard, UserGuard, IsUserGuard], },
          { path: 'user-trips', component: UserTripsComponent, canActivate: [AuthGuard, UserGuard, IsUserGuard], },
        ]
      },
      { path: 'add-car', component: AddCarComponent, canActivate: [AuthGuard, UserGuard, IsUserGuard] },
      { path: 'trip-page-info/:id', component: TripPageInfoComponent, canActivate: [AuthGuard, UserGuard, IsUserGuard] },
      { path: 'search', component: SearchTripComponent, canActivate: [AuthGuard, UserGuard, IsUserGuard] },

      { path: 'add-trip', component: AddTripLayoutComponent, canActivate: [PageAccessGuard, UserGuard, IsUserGuard] },
      { path: 'info', component: InfoPageComponent, canActivate: [AuthGuard, UserGuard, IsUserGuard] },
      { path: 'chat', component: ChatLayoutComponent, canActivate: [AuthGuard, UserGuard, IsUserGuard] },
      { path: 'info', component: InfoPageComponent, canActivate: [AuthGuard, UserGuard, IsUserGuard] },
      { path: '**', redirectTo: 'info' }
    ]
  },


];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
