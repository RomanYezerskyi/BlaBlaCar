import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { TripModel } from 'src/app/interfaces/trip-interfaces/trip-model';
import { TripUserModel } from 'src/app/interfaces/trip-interfaces/trip-user-model';
import { TripsResponseModel } from 'src/app/interfaces/trip-interfaces/trips-response-model';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { TripService } from 'src/app/services/tripservice/trip.service';

@Component({
  selector: 'app-user-booked-trips',
  templateUrl: './user-booked-trips.component.html',
  styleUrls: ['./user-booked-trips.component.scss']
})
export class UserBookedTripsComponent implements OnInit {
  trips: TripsResponseModel = {
    trips: [] = [],
    totalTrips: 0
  }
  public isFullListDisplayed: boolean = false;
  totalTrips = 0;
  private Skip: number = 0;
  private Take: number = 5;
  constructor(
    private tripService: TripService,
    private imgSanitaze: ImgSanitizerService) { }

  ngOnInit(): void {
    this.getUserBookedTrips();
  }
  sanitizeUserImg(img: string): SafeUrl {
    return this.imgSanitaze.sanitiizeUserImg(img);
  }

  getUserBookedTrips = () => {
    if (this.Skip <= this.totalTrips) {
      this.tripService.getUserBookedTrips(this.Take, this.Skip).pipe().subscribe(
        response => {
          if (response != null) {
            this.trips.trips = this.trips.trips.concat(response.trips);
            console.log(response)
            if (this.totalTrips == 0)
              this.totalTrips = response.totalTrips!;
          }
        },
        (error: HttpErrorResponse) => { console.log(error.error); }
      );
    }
    else {
      this.isFullListDisplayed = true;
    }
    this.Skip += this.Take;
  }
  deleteBookedTrip = (trip: TripModel) => {
    this.tripService.deleteBookedTrip(trip).pipe().subscribe(
      response => {
        this.ngOnInit();
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
    // const url = 'https://localhost:6001/api/BookedTrip/trip';
    // this.http.delete(url, { body: trip })
    //   .subscribe({
    //     next: (res: any) => {
    //       console.log(res);
    //       this.getUserBookedTrips();
    //     },
    //     error: (err: HttpErrorResponse) => console.error(err),
    //   });
  }
  deleteBookedSeat = (tripUser: TripUserModel) => {
    this.tripService.deleteBookedSeat(tripUser).pipe().subscribe(
      response => {
        this.ngOnInit();
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    )
    // const url = 'https://localhost:6001/api/BookedTrip/seat';

    // this.http.delete(url, { body: tripUser })
    //   .subscribe({
    //     next: (res: any) => {
    //       console.log(res);
    //       this.getUserBookedTrips();
    //     },
    //     error: (err: HttpErrorResponse) => console.error(err),
    //   });


  }
}
