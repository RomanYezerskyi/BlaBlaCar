import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CarType } from 'src/app/enums/car-type';
import { CarModel } from 'src/app/interfaces/car-interfaces/car';
import { CarService } from 'src/app/services/carservice/car.service';
import { HttpErrorResponse } from '@angular/common/http';
import { CarUpdateModel } from 'src/app/interfaces/car-interfaces/car-update-model';

@Component({
  selector: 'app-edit-car-modal-dialog',
  templateUrl: './edit-car-modal-dialog.component.html',
  styleUrls: ['./edit-car-modal-dialog.component.scss']
})
export class EditCarModalDialogComponent implements OnInit {
  pageIndex = 1;
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
    this.car.carDocuments.forEach(x => this.images.push(x.techPassport));
    this.updateCarModel.id = this.car.id;
    console.log(this.car.id);
    this.updateCarModel.modelName = this.car.modelName;
    this.updateCarModel.registNum = this.car.registNum;
    this.updateCarModel.carType = this.car.carType;
    this.updateCarModel.seats = this.car.seats;
  }
  changePage(page: number) {
    this.pageIndex = page;
  }
  getImages(files: File[]) {
    this.fileToUpload = files;
    console.log(this.fileToUpload);

  }
  deleteImg(img: string) {
    if (this.updateCarModel.deletedDocuments == undefined)
      this.updateCarModel.deletedDocuments = [];
    let doc = this.car.carDocuments.find(x => x.techPassport == img);
    console.log(doc);
    this.updateCarModel.deletedDocuments.push(doc!);
  }
  updateCar() {
    if (this.car.carDocuments == this.updateCarModel.deletedDocuments && this.fileToUpload == null) {
      alert("No files");
      return;
    }
    this.formData.append("Id", this.updateCarModel.id.toString());
    this.formData.append("ModelName", this.updateCarModel.modelName);
    this.formData.append("RegistNum", this.updateCarModel.registNum);
    this.carService.updateCar(this.formData).pipe().subscribe(
      response => {
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error); }
    );
  }
  updateCarDocuments() {
    if (this.fileToUpload.length == 0 && this.updateCarModel.deletedDocuments.length == 0) return;
    this.uploadFile();
    this.formData.append("Id", this.updateCarModel.id.toString());
    this.carService.updateCarDocuments(this.formData).pipe().subscribe(
      response => {
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error); }
    );
  }
  uploadFile = () => {
    if (this.fileToUpload.length > 0) {
      for (let item of this.fileToUpload) {
        this.formData.append('techPassportFile', item, item.name);
      }
    }
    if (this.updateCarModel.deletedDocuments?.length > 0) {
      for (let item of this.updateCarModel.deletedDocuments) {
        this.formData.append('DeletedDocuments', item.techPassport);
      }
    }
  }
  closeDialog() {
    this.dialogRef.close();
  }
}
