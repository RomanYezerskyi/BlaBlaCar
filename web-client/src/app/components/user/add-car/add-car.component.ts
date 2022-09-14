import { HttpErrorResponse, } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CarType } from 'src/app/enums/car-type';
import { AddNewCarModel } from 'src/app/interfaces/car-interfaces/addnew-car';
import { CarService } from 'src/app/services/carservice/car.service';

@Component({
  selector: 'app-add-car',
  templateUrl: './add-car.component.html',
  styleUrls: ['./add-car.component.scss']
})
export class AddCarComponent implements OnInit {
  newCar: AddNewCarModel = {} as AddNewCarModel;
  carType = CarType;
  private formData = new FormData();
  CarFormControl = new FormControl('', [Validators.required]);
  fileControl: FormControl;
  public files: any;
  constructor(
    private carService: CarService,
    private router: Router) {
    this.fileControl = new FormControl(this.files, [
      Validators.required,
    ])
  }

  ngOnInit(): void {
    this.fileControl.valueChanges.subscribe((files: any) => {
      this.uploadFile(files);
    })
  }

  uploadFile = (files: any) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload: Array<any> = files;
    for (let item of fileToUpload) {
      this.formData.append('techPassportFile', item, item.name);
    }
  }

  addCar = (form: NgForm) => {
    if (form.valid) {
      this.formData.append("ModelName", this.newCar.modelName);
      this.formData.append("RegistNum", this.newCar.registNum);
      this.formData.append("CountOfSeats", this.newCar.countOfSeats.toString());
      this.carService.addCar(this.formData).pipe().subscribe(
        response => {
          this.router.navigate(['/profile']);
        },
        (error: HttpErrorResponse) => { console.error(error.error); }
      );
    }
  }
}


