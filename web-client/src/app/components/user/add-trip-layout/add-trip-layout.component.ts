import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AddTripModel } from 'src/app/interfaces/trip-interfaces/add-trip-model';
import { CarModel } from 'src/app/interfaces/car-interfaces/car-model';
import { CarService } from 'src/app/services/carservice/car.service';
import { Subject, takeUntil } from 'rxjs';

export enum Menu {
  Info = 1,
  Car = 2
}
@Component({
  selector: 'app-add-trip-layout',
  templateUrl: './add-trip-layout.component.html',
  styleUrls: ['./add-trip-layout.component.scss']
})
export class AddTripLayoutComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  trip: AddTripModel = {} as AddTripModel;
  menu = Menu;
  userCars: CarModel[] = [];
  page = false;
  check = Menu.Info;
  constructor(private carService: CarService) { }

  ngOnInit(): void {
    this.getUserCars();
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  changePage(item: Menu): void {
    this.check = item as number;
  }
  getOutPut(event: AddTripModel): void {
    this.trip = event;
  }
  getPagePut(event: number): void {
    this.check = Menu.Car;
  }
  getUserCars(): void {
    this.carService.getUserCars().pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.userCars = response;
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
}
