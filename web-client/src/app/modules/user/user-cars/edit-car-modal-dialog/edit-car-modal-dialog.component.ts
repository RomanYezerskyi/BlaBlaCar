import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CarType } from 'src/app/core/enums/car-type';
import { CarModel } from 'src/app/core/models/car-models/car-model';
import { CarService } from 'src/app/core/services/car-service/car.service';
import { HttpErrorResponse } from '@angular/common/http';
import { CarUpdateModel } from 'src/app/core/models/car-models/car-update-model';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-edit-car-modal-dialog',
  templateUrl: './edit-car-modal-dialog.component.html',
  styleUrls: ['./edit-car-modal-dialog.component.scss']
})
export class EditCarModalDialogComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  pageIndex: number = 1;
  car: CarModel = {} as CarModel;
  updateCarModel: CarUpdateModel = {} as CarUpdateModel;
  CarFormControl = new FormControl('', [Validators.required]);
  carType = CarType;
  private formData = new FormData();
  images: Array<string> = [];
  fileToUpload: Array<File> = [];
  constructor(private dialogRef: MatDialogRef<EditCarModalDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any,
    private carService: CarService) {
    this.car = data.car;
  }

  ngOnInit(): void {
    this.car.carDocuments.forEach(x => this.images.push(x.technicalPassport));
    this.updateCarModel.id = this.car.id;
    console.log(this.car.carDocuments);
    this.updateCarModel.modelName = this.car.modelName;
    this.updateCarModel.registNum = this.car.registrationNumber;
    this.updateCarModel.carType = this.car.carType;
    this.updateCarModel.seats = this.car.seats;
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  changePage(page: number): void {
    this.pageIndex = page;
  }
  getImages(files: File[]): void {
    this.fileToUpload = files;
  }
  deleteImg(img: string): void {
    if (this.updateCarModel.deletedDocuments == undefined)
      this.updateCarModel.deletedDocuments = [];
    let doc = this.car.carDocuments.find(x => x.technicalPassport == img);
    this.updateCarModel.deletedDocuments.push(doc!);
  }
  updateCar(): void {
    if (this.car.carDocuments == this.updateCarModel.deletedDocuments && this.fileToUpload == null) {
      alert("No files");
      return;
    }
    this.formData.append("Id", this.updateCarModel.id.toString());
    this.formData.append("ModelName", this.updateCarModel.modelName);
    this.formData.append("RegistNum", this.updateCarModel.registNum);
    this.carService.updateCar(this.formData).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error); }
    );
  }
  updateCarDocuments(): void {
    if (this.fileToUpload.length == 0 && this.updateCarModel.deletedDocuments.length == 0) return;
    this.uploadFile();
    this.formData.append("Id", this.updateCarModel.id.toString());
    this.carService.updateCarDocuments(this.formData).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error); }
    );
  }
  uploadFile(): void {
    if (this.fileToUpload.length > 0) {
      for (let item of this.fileToUpload) {
        this.formData.append('techPassportFile', item, item.name);
      }
    }
    if (this.updateCarModel.deletedDocuments?.length > 0) {
      for (let item of this.updateCarModel.deletedDocuments) {
        this.formData.append('DeletedDocuments', item.technicalPassport);
      }
    }
  }
  closeDialog(): void {
    this.dialogRef.close();
  }
}
