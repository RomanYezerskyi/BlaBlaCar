import { HttpClient, HttpErrorResponse, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Component, EventEmitter, Inject, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BookedTripModel } from 'src/app/interfaces/trip-interfaces/booked-trip-model';
import { TripModel } from 'src/app/interfaces/trip-interfaces/trip-model';
import { CarModel } from "src/app/interfaces/car-interfaces/car-model";
import { AvailableSeatsType } from 'src/app/enums/available-seats-type';
import { SeatModel } from 'src/app/interfaces/car-interfaces/seat-model';
import { TripService } from 'src/app/services/tripservice/trip.service';
import { Subject, takeUntil } from 'rxjs';
@Component({
  selector: 'app-dialog-booking-confirmation',
  templateUrl: './dialog-booking-confirmation.component.html',
  styleUrls: ['./dialog-booking-confirmation.component.scss']
})
export class DialogBookingConfirmationComponent implements OnInit, OnDestroy {
  @Output() onSubmitReason = new EventEmitter();
  private unsubscribe$: Subject<void> = new Subject<void>();
  seatType = AvailableSeatsType;
  carModel: CarModel = {} as CarModel;
  trip: TripModel = {} as TripModel;
  requestedSeats = 0;
  bookedtrip: BookedTripModel = { bookedSeats: [], id: 0, requestedSeats: 1, tripId: 0 };

  constructor(
    private dialogRef: MatDialogRef<DialogBookingConfirmationComponent>, private http: HttpClient,
    @Inject(MAT_DIALOG_DATA) data: any, private tripService: TripService) {
    this.trip = data.trip as TripModel;

    this.trip.car.seats.forEach(x => {
      x.isSelected = false;
    });
    this.requestedSeats = data.requestedSeats;
    this.bookedtrip.requestedSeats = this.requestedSeats;
    this.bookedtrip.tripId = this.trip.id;
  }

  ngOnInit() {
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  cheackSeat(item: SeatModel): boolean {
    let res = this.trip.availableSeats.some(x => {
      if (x.seatId == item.id && x.availableSeatsType == AvailableSeatsType.Booked)
        return true;
      return false;
    });
    return res;
  }
  confirmBook(): void {
    if (this.requestedSeats != this.bookedtrip.bookedSeats.length) {
      return;
    }
    console.log(this.bookedtrip)
    this.tripService.bookSeatsInTrip(this.bookedtrip).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.dialogRef.close();
        alert(response);
        console.log(response);
      },
      (error: HttpErrorResponse) => { alert(error.error); console.log(error.error); }
    )
    this.onSubmitReason.emit();
  }
  close(): void {
    this.dialogRef.close();
  }
  bookSeat(seatId: number): void {
    const seat: SeatModel = { id: seatId, carId: this.trip.car.id, seatNumber: 0, tripUsers: [] }
    if (this.bookedtrip.bookedSeats.find(x => x.id == seat.id)) {
      this.bookedtrip.bookedSeats = this.bookedtrip.bookedSeats.filter(x => x.id != seatId);
      this.changeSeatStatus(seatId);
      return;
    }
    if (this.bookedtrip.bookedSeats.length == this.requestedSeats) return;
    else {
      this.bookedtrip.bookedSeats.push(seat);
      this.changeSeatStatus(seatId);
    }
  }
  changeSeatStatus(seatId: number): void {
    this.trip.car.seats.forEach(seat => {
      if (seat.id == seatId) {
        if (seat.isSelected) { seat.isSelected = false; }
        else if (!seat.isSelected) { seat.isSelected = true; }
        console.log(seat);
      }
    });
  }
}
