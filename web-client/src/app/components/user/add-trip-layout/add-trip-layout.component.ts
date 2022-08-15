import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AddTripModel } from 'src/app/interfaces/add-trip';
import { CarModel } from 'src/app/interfaces/car';

@Component({
  selector: 'app-add-trip-layout',
  templateUrl: './add-trip-layout.component.html',
  styleUrls: ['./add-trip-layout.component.scss']
})
export class AddTripLayoutComponent implements OnInit {
  trip: AddTripModel = {
    startPlace: '',
    endPlace: '',
    startTime: new Date(''),
    endTime: new Date(''),
    pricePerSeat: 0,
    description: '',
    countOfSeats: 0,
    carId: 0,
    availableSeats: []
  };

  userCars: CarModel[] = [];

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.getUserCars();
  }
  getUserCars() {
    this.http.get("https://localhost:6001/api/Car")
      .subscribe({
        next: (res) => {
          this.userCars = res as CarModel[];
        },
        error: (err: HttpErrorResponse) => console.log(err)
      });
  }




}
