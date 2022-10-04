import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { CarType } from 'src/app/enums/car-type';
import { CarModel } from 'src/app/interfaces/car-interfaces/car-model';
import { CarStatus } from 'src/app/interfaces/car-interfaces/car-status';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { CarService } from 'src/app/core/services/car-service/car.service';
import { ImgSanitizerService } from 'src/app/core/services/image-sanitizer-service/img-sanitizer.service';
import { UserService } from 'src/app/core/services/user-service/user.service';
import { EditCarModalDialogComponent } from './edit-car-modal-dialog/edit-car-modal-dialog.component';

@Component({
  selector: 'app-user-cars',
  templateUrl: './user-cars.component.html',
  styleUrls: ['./user-cars.component.scss']
})
export class UserCarsComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  cars: CarModel[] = [];
  carStatus = CarStatus;
  carType = CarType;
  constructor(private imgSanitize: ImgSanitizerService,
    private carService: CarService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.getUserCars()
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  sanitaizeImg(img: string): SafeUrl {
    return this.imgSanitize.sanitiizeImg(img);
  }
  getUserCars(): void {
    this.carService.getUserCars().pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.cars = response;
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error); }
    );
  }
  edit(carId: number): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.data = {
      car: this.cars.find(x => x.id == carId)
    }
    const dRef = this.dialog.open(EditCarModalDialogComponent, dialogConfig);
    // dRef.componentInstance.onSubmitReason.subscribe(() => {
    //   this.searchData();
    // });
  }
  deleteCar(carId: number): void {
    this.carService.deleteCar(carId).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
  }
}
