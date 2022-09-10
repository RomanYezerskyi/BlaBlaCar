import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SafeUrl } from '@angular/platform-browser';
import { CarType } from 'src/app/enums/car-type';
import { AddNewCarModel } from 'src/app/interfaces/car-interfaces/addnew-car';
import { CarModel } from 'src/app/interfaces/car-interfaces/car';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { LightgalleryModule } from 'lightgallery/angular/lightgallery-angular';
import lgZoom from 'lightgallery/plugins/zoom';
import { BeforeSlideDetail } from 'lightgallery/lg-events';
import { LightGallery } from 'lightgallery/lightgallery';
import { Subject } from 'rxjs';
import { flush } from '@angular/core/testing';
import { CarService } from 'src/app/services/carservice/car.service';
import { HttpErrorResponse } from '@angular/common/http';
@Component({
  selector: 'app-edit-car-modal-dialog',
  templateUrl: './edit-car-modal-dialog.component.html',
  styleUrls: ['./edit-car-modal-dialog.component.scss']
})
export class EditCarModalDialogComponent implements OnInit {
  car: CarModel = {} as CarModel;
  fileControl: FormControl;
  CarFormControl = new FormControl('', [Validators.required]);
  files: any;
  carType = CarType;
  private formData = new FormData();
  lightGallery!: LightGallery;
  private needRefresh = false;
  settings = {
    counter: true,
    plugins: [lgZoom],
  };
  editImages = false;
  images: Array<string> = [];
  fileToUpload: Array<File> = [];
  constructor(private dialogRef: MatDialogRef<EditCarModalDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any, private imgSanitizer: ImgSanitizerService,
    private carService: CarService) {
    this.fileControl = new FormControl(this.files, [
      Validators.required,
    ])
    this.car = data.car;
  }

  ngOnInit(): void {
    this.car.carDocuments.forEach(x => this.images.push(x.techPassport));
    this.fileControl.valueChanges.subscribe((files: any) => {
      this.uploadFile(files);
    })
  }
  edit() {
    this.editImages = !this.editImages;
  }
  onInit = (detail: any): void => {
    this.lightGallery = detail.instance;
  };
  ngAfterViewChecked(): void {
    if (this.needRefresh) {
      this.lightGallery.refresh();

      this.needRefresh = false;
    }
  }
  sanitizeImg(img: string): SafeUrl {
    return this.imgSanitizer.sanitiizeImg(img);
  }
  getImages(files: File[]) {
    this.fileToUpload = files;
    console.log(this.fileToUpload);
  }
  deleteImg(img: string) {
    this.carService.deleteDoc(img).subscribe(
      response => {
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.log(error); }
    );
    console.log(img);
  }
  uploadFile = (files: any) => {
    if (files.length === 0) {
      return;
    }
    // let fileToUpload: Array<any> = files;
    for (let item of this.fileToUpload) {
      this.formData.append('techPassportFile', item, item.name);
    }
    this.needRefresh = true;
  }

}
