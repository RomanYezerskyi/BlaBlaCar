import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BookedTripModel } from 'src/app/interfaces/booked-trip';
import { TripModel } from 'src/app/interfaces/trip';
import { CarModel } from "src/app/interfaces/car";
import { AvailableSeatsType } from 'src/app/enums/available-seats-type';
import { SeatModel } from 'src/app/interfaces/seat';
@Component({
  selector: 'app-dialog-booking-confirmation',
  templateUrl: './dialog-booking-confirmation.component.html',
  styleUrls: ['./dialog-booking-confirmation.component.scss']
})
export class DialogBookingConfirmationComponent implements OnInit {
  seatType = AvailableSeatsType;
  carModel: CarModel = { id: 0, carType: 0, modelName: '', registNum: '', seats: [], carStatus: -1, carDocuments: [] }
  trip: TripModel = {
    id: 0,
    startPlace: '',
    endPlace: '',
    startTime: new Date(),
    endTime: new Date(),
    countOfSeats: 0,
    pricePerSeat: 0,
    description: '',
    userId: '',
    availableSeats: [],
    car: this.carModel,
    tripUsers: []
  };
  requestedSeats = 0;
  bookedtrip: BookedTripModel = { id: 0, bookedSeats: [], requestedSeats: 0, tripId: 0 }
  toggle = false;
  constructor(
    private dialogRef: MatDialogRef<DialogBookingConfirmationComponent>, private http: HttpClient,
    @Inject(MAT_DIALOG_DATA) data: any,) {
    this.trip = data.trip as TripModel;

    this.trip.car.seats.forEach(x => {
      x.isSelected = false;
    });

    this.requestedSeats = data.requestedSeats;
    this.bookedtrip.requestedSeats = this.requestedSeats;
    this.bookedtrip.tripId = this.trip.id;
    console.log(this.requestedSeats)
    console.log("aaaa" + JSON.stringify(this.bookedtrip));
  }

  ngOnInit() {

  }
  cheackSeat(item: SeatModel): boolean {
    let res = this.trip.availableSeats.some(x => {
      if (x.seatId == item.id && x.availableSeatsType == AvailableSeatsType.Booked)
        return true;
      return false;
    });
    return res;
  }
  confirmBook = async () => {
    const url = 'https://localhost:6001/api/BookedTrip'
    await this.http.post(url, this.bookedtrip, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    })
      .subscribe({
        next: (res: any) => {
          alert(res.result)
          this.dialogRef.close();
        },
        error: (err: HttpErrorResponse) => {
          alert(err.error)
          console.error(err)
        },
      })
  }
  close() {
    this.dialogRef.close();
  }
  bookSeat(seatId: number) {

    const seat: SeatModel = { id: seatId, carId: this.trip.car.id, num: 0, tripUsers: [] }
    if (this.bookedtrip.bookedSeats.find(x => x.id == seat.id)) {
      this.bookedtrip.bookedSeats = this.bookedtrip.bookedSeats.filter(x => x.id != seatId);
      this.changeSeatStatus(seatId);
      console.log("bbbb " + JSON.stringify(this.bookedtrip));
      return;
    }
    if (this.bookedtrip.bookedSeats.length == this.requestedSeats) return;
    else {
      this.bookedtrip.bookedSeats.push(seat);
      this.changeSeatStatus(seatId);
      console.log("aaaaa " + JSON.stringify(this.bookedtrip));
    }
  }
  changeSeatStatus(seatId: number) {
    this.trip.car.seats.forEach(seat => {
      if (seat.id == seatId) {
        if (seat.isSelected) { seat.isSelected = false; }
        else if (!seat.isSelected) { seat.isSelected = true; }
        console.log(seat);
      }
    });
  }
}
