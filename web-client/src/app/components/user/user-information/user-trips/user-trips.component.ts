import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { TripsResponseModel } from 'src/app/interfaces/trip-interfaces/trips-response-model';
import { TripModel } from 'src/app/interfaces/trip-interfaces/trip-model';
import { TripUserModel } from 'src/app/interfaces/trip-interfaces/trip-user-model';
import { UserTrips, UserTripsResponse } from 'src/app/interfaces/user-interfaces/user-trips';
import { TripService } from 'src/app/services/tripservice/trip.service';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';

@Component({
  selector: 'app-user-trips',
  templateUrl: './user-trips.component.html',
  styleUrls: ['./user-trips.component.scss']
})
export class UserTripsComponent implements OnInit {
  // trips: UserTrips = {
  //   trips: [],
  //   totalTrips: 0
  // }
  trips: UserTripsResponse = {
    trips: [] = [],
    totalTrips: 0
  }
  public isFullListDisplayed: boolean = false;
  totalTrips = 0;
  private Skip: number = 0;
  private Take: number = 5;
  constructor(
    private imgSanitaze: ImgSanitizerService,
    private tripService: TripService) { }

  ngOnInit(): void {
    this.getUserTrips();
  }
  sanitizeUserImg(img: string): SafeUrl {
    return this.imgSanitaze.sanitiizeUserImg(img);
  }
  onScroll() {

  }
  getUserTrips = () => {

    console.log(this.Skip);
    if (this.Skip <= this.totalTrips) {
      this.tripService.getUserTrips(this.Take, this.Skip).pipe().subscribe(
        response => {
          if (response != null) {
            this.trips.trips = this.trips.trips.concat(response.trips);
            console.log(this.trips);
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
    // const url = 'https://localhost:6001/api/Trips/user-trips';
    // this.http.get(url)
    //   .subscribe({
    //     next: (res: any) => {
    //       this.trips = res as TripModel[];
    //       // console.log(this.trips);
    //       console.log(res);
    //     },
    //     error: (err: HttpErrorResponse) => console.error(err),
    //   });
  }
  deleteUserFromTrip = (userId: string, tripId: number) => {
    let tripUser = {
      userId: userId,
      tripId: tripId
    };
    this.tripService.deleteUserFromTrip(tripUser).pipe().subscribe(
      response => {
        this.ngOnInit();
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
    // this.http.delete(url, { body: tripUser })
    //   .subscribe({
    //     next: (res: any) => {
    //       console.log(res);
    //       this.getUserTrips();
    //     },
    //     error: (err: HttpErrorResponse) => console.error(err),
    //   });
  }

  deleteTrip = (id: number) => {
    this.tripService.deleteTrip(id).pipe().subscribe(
      response => {
        this.ngOnInit();
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
    // const url = 'https://localhost:6001/api/Trips/trip/';
    // this.http.delete(url + id)
    //   .subscribe({
    //     next: (res: any) => {
    //       console.log(res);
    //       this.getUserTrips();
    //     },
    //     error: (err: HttpErrorResponse) => console.error(err),
    //   });
  }



}
