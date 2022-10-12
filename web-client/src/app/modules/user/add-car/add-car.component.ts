import { HttpErrorResponse, } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, NgForm, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { CarType } from 'src/app/core/enums/car-type';
import { AddNewCarModel } from 'src/app/core/models/car-models/addnew-car-model';
import { CarService } from 'src/app/core/services/car-service/car.service';

@Component({
  selector: 'app-add-car',
  templateUrl: './add-car.component.html',
  styleUrls: ['./add-car.component.scss']
})
export class AddCarComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  newCar: AddNewCarModel = {} as AddNewCarModel;
  carType = CarType;
  private formData = new FormData();
  CarFormControl = new FormControl('', [Validators.required]);
  fileControl: FormControl;
  public files: any;
  constructor(
    private _snackBar: MatSnackBar,
    private carService: CarService,
    private router: Router) {
    this.fileControl = new FormControl(this.files, [
      Validators.required,
    ])
  }

  ngOnInit(): void {
    this.fileControl.valueChanges.pipe(takeUntil(this.unsubscribe$)).subscribe((files: any) => {
      this.uploadFile(files);
    })
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  uploadFile(files: any): void {
    if (files.length === 0) {
      return;
    }
    let fileToUpload: Array<any> = files;
    for (let item of fileToUpload) {
      this.formData.append('techPassportFile', item, item.name);
    }
  }

  addCar(form: NgForm): void {
    if (form.valid) {
      this.formData.append("ModelName", this.newCar.modelName);
      this.formData.append("RegistrationNumber", this.newCar.registNum);
      this.formData.append("CountOfSeats", this.newCar.countOfSeats.toString());
      this.carService.addCar(this.formData).pipe(takeUntil(this.unsubscribe$)).subscribe(
        response => {
          this.openSnackBar("Car saved!")
          this.router.navigate(['/profile']);
        },
        (error: HttpErrorResponse) => { console.error(error.error); }
      );
    }
  }
  private openSnackBar(message: string) {
    this._snackBar.open(message, "Close");
  }
}


