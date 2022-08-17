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

  async ngOnInit() {
    // this.trip = history.state;
    // console.log(this.trip);

    console.log(this.trip);
    // this.userCars = await this.getUserCar();
  }
  setSelectedCar(carId: number) {
    this.userCars.forEach(car => {
      if (carId == car.id) {
        car.seats.forEach(seat => {
          let availableSeat: AddAvailableSeats = { seatId: seat.id };
          this.trip.availableSeats.push(availableSeat);
          seat.isSelected = true;
        });
        console.log(car);
      }
    });
  }
  addSeat(seatId: number) {
    console.log(seatId);
    let seat: AddAvailableSeats = { seatId: seatId }
    if (this.trip.availableSeats.find(availableSeat => availableSeat.seatId == seat.seatId)) {
      this.trip.availableSeats = this.trip.availableSeats.filter(x => x.seatId != seatId);
      this.changeSeatStatus(seatId);
      console.log("bbbb " + JSON.stringify(this.trip.availableSeats));
    }
    else {
      this.trip.availableSeats.push(seat);
      this.changeSeatStatus(seatId);
      console.log("aaaaa " + JSON.stringify(this.trip.availableSeats));
    }
  }
  changeSeatStatus(seatId: number) {
    this.userCars.forEach(car => {
      if (this.trip.carId == car.id) {
        car.seats.forEach(seat => {
          if (seat.id == seatId) {
            if (seat.isSelected) { seat.isSelected = false; }
            else if (!seat.isSelected) { seat.isSelected = true; }
          }
        });
      }
    });
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
