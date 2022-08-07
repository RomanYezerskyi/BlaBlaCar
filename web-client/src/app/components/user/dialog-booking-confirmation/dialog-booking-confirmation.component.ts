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
  carModel: CarModel = { id:0, carType:0, modelName:'',registNum:'', seats:[],carStatus:-1,techPassport:'' }
  trip: TripModel = { 
    id:0, 
    startPlace:'',
    endPlace:'',
    startTime: new Date(),
    endTime: new Date(),
    countOfSeats:0,
    pricePerSeat:0,
    description:'',
    userId:'',
    availableSeats: [],
    car: this.carModel
   };
  requestedSeats = 0;
  bookedtrip: BookedTripModel ={ id:0, bookedSeats:[], requestedSeats:0, tripId:0 }
  constructor(
      private dialogRef: MatDialogRef<DialogBookingConfirmationComponent>,private http: HttpClient,
      @Inject(MAT_DIALOG_DATA) data: any,) {
      this.trip = data.trip as TripModel;
      this.requestedSeats = data.requestedSeats;
      this.bookedtrip.requestedSeats = this.requestedSeats;
      this.bookedtrip.tripId = this.trip.id;
      console.log(this.requestedSeats)
      console.log("aaaa"+ JSON.stringify(this.bookedtrip));
  }

  ngOnInit() {
 
  }
  confirmBook = async () => {
    const url ='https://localhost:6001/api/BookedTrip'
    await this.http.post(url, this.bookedtrip, {
      headers: new HttpHeaders({ "Content-Type": "application/json"})
    })
    .subscribe({
      next: (res)=>{
        console.log(res);
      },
      error: (err: HttpErrorResponse) => console.error(err),
    })
  } 
  save() {
      this.dialogRef.close();
  }
  close() {
      this.dialogRef.close();
  }
  bookSeat(seatId:number){
    const seat: SeatModel = { id:seatId, carId:this.trip.car.id, num:0,tripUsers:[] }
    if(this.bookedtrip.bookedSeats.find(x=>x.id == seat.id)){
      this.bookedtrip.bookedSeats.splice(this.bookedtrip.bookedSeats.indexOf(seat));
      console.log("bbbb " + JSON.stringify(this.bookedtrip));
    }
    else{
      this.bookedtrip.bookedSeats.push(seat);
      console.log("aaaaa " + JSON.stringify(this.bookedtrip));
    }
  }
}
