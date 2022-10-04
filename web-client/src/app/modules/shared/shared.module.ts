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




@NgModule({
  declarations: [
    ImagesGalleryComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatDialogModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,

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
  exports: [
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
    MatTooltipModule,


    ImagesGalleryComponent
  ]
})
export class SharedModule { }
