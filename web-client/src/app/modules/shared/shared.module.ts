import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
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
import { LightgalleryModule } from 'lightgallery/angular';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ImagesGalleryComponent } from './components/images-gallery/images-gallery.component';
import { CreateNotificationDialogComponent } from './components/create-notification-dialog/create-notification-dialog.component';
import { NotificationsComponent } from './components/notifications/notifications.component';
import { AlertsComponent } from './components/alerts/alerts.component';
import { InfoPageComponent } from './components/info-page/info-page.component';
import { HttpClientModule } from '@angular/common/http';
import { ChatLayoutComponent } from './components/chat-layout/chat-layout.component';
import { ChatListComponent } from './components/chat-layout/chat-list/chat-list.component';
import { ChatComponent } from './components/chat-layout/chat/chat.component';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SearchFormComponent } from './components/search-form/search-form.component';
import { MapsAutocompleteComponent } from './components/maps-autocomplete/maps-autocomplete.component';




@NgModule({
  declarations: [
    ImagesGalleryComponent,
    CreateNotificationDialogComponent,
    NotificationsComponent,
    NavbarComponent,
    AlertsComponent,
    InfoPageComponent,
    ChatLayoutComponent,
    ChatListComponent,
    ChatComponent,
    SearchFormComponent,
    MapsAutocompleteComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    // HttpClientModule,
    // FormsModule,
    // ReactiveFormsModule,
    // FormsModule,
    // BrowserAnimationsModule,
    RouterModule,
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



    // InfiniteScrollModule,

  ],
  entryComponents: [/*DialogBookingConfirmationComponent*/
    CreateNotificationDialogComponent, /*EditModalDialogComponent,*/ /*EditCarModalDialogComponent,*/ /*RequestDrivingLicenseComponent*/],
  exports: [
    // MatDialogModule,
    // MatToolbarModule,
    // MatButtonModule,
    // MatIconModule,
    // InfiniteScrollModule,
    // MatFormFieldModule,
    // MatInputModule,
    // MatSelectModule,
    // MatRippleModule,
    // NgxMatFileInputModule,
    // MatNativeDateModule,
    // MatDatepickerModule,
    // MatRadioModule,
    // MatProgressSpinnerModule,
    // LightgalleryModule,
    // MatAutocompleteModule,
    // MatTooltipModule,


    ImagesGalleryComponent,
    CreateNotificationDialogComponent,
    NotificationsComponent,
    NavbarComponent,
    AlertsComponent,
    InfoPageComponent,
    ChatLayoutComponent,
    ChatListComponent,
    ChatComponent,
    SearchFormComponent,
    MapsAutocompleteComponent,
  ]
})
export class SharedModule { }
