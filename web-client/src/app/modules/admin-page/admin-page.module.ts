import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminPageRoutingModule } from './admin-page-routing.module';
import { AdminPageComponent } from './admin-page.component';
import { RolesComponent } from './roles/roles.component';
import { UsersRequestsComponent } from './users-requests/users-requests.component';
import { UserRequestInfoComponent } from './user-request-info/user-request-info.component';
import { MainComponent } from './main/main.component';
import { TopUsersListComponent } from './main/top-users-list/top-users-list.component';
import { AdminNotificationsComponent } from './main/admin-notifications/admin-notifications.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { ChartsComponent } from './charts/charts.component';
import { UsersManagementComponent } from './users-management/users-management.component';
import { AdministratorsComponent } from './administrators/administrators.component';
import { SharedModule } from '../shared/shared.module';
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
import { AdminNavbarComponent } from './admin-navbar/admin-navbar.component';
// import { CreateNotificationDialogComponent } from 'src/app/components/create-notification-dialog/create-notification-dialog.component';


@NgModule({
  declarations: [
    AdminPageComponent,
    RolesComponent,
    UsersRequestsComponent,
    UserRequestInfoComponent,
    MainComponent,
    TopUsersListComponent,
    AdminNotificationsComponent,
    AdminPanelComponent,
    ChartsComponent,
    UsersManagementComponent,
    AdministratorsComponent,
    AdminNavbarComponent,
  ],

  imports: [
    CommonModule,
    AdminPageRoutingModule,
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
export class AdminPageModule { }
