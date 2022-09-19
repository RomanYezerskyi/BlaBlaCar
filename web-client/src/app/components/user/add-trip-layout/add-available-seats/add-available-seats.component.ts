import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CarType } from 'src/app/enums/car-type';
import { AddAvailableSeatsModel } from 'src/app/interfaces/trip-interfaces/add-available-seats-model';
import { AddTripModel } from 'src/app/interfaces/trip-interfaces/add-trip-model';
import { CarModel } from 'src/app/interfaces/car-interfaces/car-model';
import { TripService } from 'src/app/services/tripservice/trip.service';
import { Subject, takeUntil } from 'rxjs';
@Component({
  selector: 'app-add-available-seats',
  templateUrl: './add-available-seats.component.html',
  styleUrls: ['./add-available-seats.component.scss', '.././add-trip-layout.component.scss'],
})
export class AddAvailableSeatsComponent implements OnInit, OnDestroy {
  @Input() trip: AddTripModel = { availableSeats: [] = [], };
  @Input() userCars: CarModel[] = [];
  @Output() tripOutput: EventEmitter<AddTripModel> = new EventEmitter<AddTripModel>;
  private unsubscribe$: Subject<void> = new Subject<void>();
  userCar: CarModel = {} as CarModel;
  constructor(private tripService: TripService) { }

  ngOnInit(): void {
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  setSelectedCar(carId: number): void {
    this.trip.availableSeats = [];
    this.userCars.forEach(car => {
      if (carId == car.id) {
        car.seats.forEach(seat => {
          let availableSeat: AddAvailableSeatsModel = { seatId: seat.id };
          this.trip.availableSeats.push(availableSeat);
          console.log(this.trip.availableSeats);
          seat.isSelected = true;
        });
      }
    });
  }
  addSeat(seatId: number): void {
    console.log(seatId);
    let seat: AddAvailableSeatsModel = { seatId: seatId }
    if (this.trip.availableSeats!.find(availableSeat => availableSeat.seatId == seat.seatId)) {
      this.trip.availableSeats = this.trip.availableSeats!.filter(x => x.seatId != seatId);
      this.changeSeatStatus(seatId);
    }
    else {
      this.trip.availableSeats!.push(seat);
      this.changeSeatStatus(seatId);
    }
  }
  changeSeatStatus(seatId: number): void {
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


  addTrip(form: NgForm): void {
    if (form.valid) {
      if (this.userCar === undefined) {
        alert("Add cars!");
        return;
      }
      this.tripService.addNewtrip(this.trip).pipe(takeUntil(this.unsubscribe$)).subscribe(
        response => {
          console.log(response)
        },
        (error: HttpErrorResponse) => { console.error(error.error); }
      )
    }
  }

}
