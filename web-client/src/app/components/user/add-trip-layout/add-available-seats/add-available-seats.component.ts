import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CarType } from 'src/app/enums/car-type';
import { AddAvailableSeats } from 'src/app/interfaces/add-available-seats';
import { AddTripModel } from 'src/app/interfaces/add-trip';
import { CarModel } from 'src/app/interfaces/car';
import { CarStatus } from 'src/app/interfaces/car-status';
import { SeatModel } from 'src/app/interfaces/seat';
import { TripModel } from 'src/app/interfaces/trip';
import { SharedDataService } from 'src/app/services/shared-data.service';

@Component({
  selector: 'app-add-available-seats',
  templateUrl: './add-available-seats.component.html',
  styleUrls: ['./add-available-seats.component.scss', '.././add-trip-layout.component.scss'],
})
export class AddAvailableSeatsComponent implements OnInit {
  invalidForm: boolean | undefined;
  @Input() trip: AddTripModel = {
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
  @Input() userCars: CarModel[] = [];
  @Output() tripOutput: EventEmitter<AddTripModel> = new EventEmitter<AddTripModel>;
  userCar: CarModel = { id: 0, carType: -1, modelName: '', registNum: '', seats: [], carStatus: -1, carDocuments: [] };
  private readonly url = 'https://localhost:6001/api/Trips/';
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    // this.trip = history.state;
    // console.log(this.trip);

    console.log(this.trip);
    // this.getUserCar()
  }

  getUserCar() {
    this.http.get("https://localhost:6001/api/Car/" + this.trip.carId)
      .subscribe({
        next: (res) => {
          this.userCar = res as CarModel;
          console.log(this.userCar);
        }
      });
  }

  addSeat(seatId: number) {
    const seat: AddAvailableSeats = { seatId: seatId }
    if (this.trip.availableSeats.find(x => x.seatId == seat.seatId)) {
      this.trip.availableSeats.splice(this.trip.availableSeats.indexOf(seat));
      console.log("bbbb " + JSON.stringify(this.trip.availableSeats));
    }
    else {
      this.trip.availableSeats.push(seat);
      console.log("aaaaa " + JSON.stringify(this.trip.availableSeats));
    }
    this.tripOutput.emit(this.trip);
  }
  addTrip = (form: NgForm) => {
    if (form.valid) {
      if (this.userCar === undefined) {
        alert("Add cars!");
        return;
      }
      console.log(this.trip);
      const url = 'https://localhost:6001/api/Trips/'
      this.http.post(url, this.trip)
        .subscribe((res) => {
          console.error(res);
        })
    }
  }

}
