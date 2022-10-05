import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/core/guards/auth-guard/auth.guard';
import { UserGuard } from 'src/app/core/guards/user-guard/user.guard';
import { PageAccessGuard } from 'src/app/guards/page-access/page-access.guard';
import { ChatLayoutComponent } from '../shared/components/chat-layout/chat-layout.component';
import { InfoPageComponent } from '../shared/components/info-page/info-page.component';
import { AddCarComponent } from './add-car/add-car.component';
import { AddTripLayoutComponent } from './add-trip-layout/add-trip-layout.component';
import { HomeComponent } from './home/home.component';
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
      { path: 'search', component: SearchTripComponent, canActivate: [AuthGuard, UserGuard] },
      { path: 'home', component: HomeComponent  /*, canActivate: [AuthGuard]*/ },
      { path: 'add-trip', component: AddTripLayoutComponent, canActivate: [PageAccessGuard] },
      { path: 'info', component: InfoPageComponent, canActivate: [AuthGuard] },
    ]
  },
  { path: 'chat', component: ChatLayoutComponent, canActivate: [AuthGuard] },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
