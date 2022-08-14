import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CarType } from 'src/app/enums/car-type';
import { AddTripModel } from 'src/app/interfaces/add-trip';
import { AddNewCarModel } from 'src/app/interfaces/addnew-car';
import { AvailableSeatsModel } from 'src/app/interfaces/available-seats';
import { CarModel } from 'src/app/interfaces/car';
import { SeatModel } from 'src/app/interfaces/seat';
@Component({
  selector: 'app-add-trip',
  templateUrl: './add-trip.component.html',
  styleUrls: ['./add-trip.component.scss'],
})
export class AddTripComponent implements OnInit {
  message: string | undefined;
  invalidForm: boolean | undefined;
  data: any = []
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

  userCars: CarModel[] | undefined;

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {

    this.getUserCars();
  }
  navigateToAvailableSeats() {
    this.router.navigate(['add-trip/add-seats'], { state: this.trip });
  }

  getUserCars() {
    this.http.get("https://localhost:6001/api/Car")
      .subscribe({
        next: (res) => {
          this.userCars = res as CarModel[];
        }
      });
  }

  addTrip = (form: NgForm) => {
    if (form.valid) {
      if (this.userCars === undefined) {
        alert("Add cars!");
        return;
      }
      const url = 'https://localhost:6001/api/Trips/'
      this.http.post(url, this.trip)
        .subscribe((res) => {
          this.data = res
          console.log(this.data)
          console.error(res);
        })
    }
  }
}
