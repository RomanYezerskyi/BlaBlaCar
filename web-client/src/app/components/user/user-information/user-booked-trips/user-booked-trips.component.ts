import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { TripModel } from 'src/app/interfaces/trip';
import { TripUserModel } from 'src/app/interfaces/trip-user-model';
import { UserModel } from 'src/app/interfaces/user-model';
import { TripService } from 'src/app/services/tripservice/trip.service';

@Component({
  selector: 'app-user-booked-trips',
  templateUrl: './user-booked-trips.component.html',
  styleUrls: ['./user-booked-trips.component.scss']
})
export class UserBookedTripsComponent implements OnInit {
  trips: TripModel[] = [];
  constructor(private http: HttpClient,
    private router: Router,
    private sanitizer: DomSanitizer,
    private tripService: TripService) { }

  ngOnInit(): void {
    this.getUserBookedTrips();
  }
  sanitaizeImg(img: string): SafeUrl {
    console.log(img);
    return this.sanitizer.bypassSecurityTrustUrl(img);
  }

  getUserBookedTrips = () => {
    this.tripService.getUserBookedTrips().pipe().subscribe(
      response => {
        this.trips = response;
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
    // const url = 'https://localhost:6001/api/BookedTrip/trips';
    // this.http.get(url)
    //   .subscribe({
    //     next: (res: any) => {
    //       this.trips = res as TripModel[];
    //       console.log(this.trips);
    //     },
    //     error: (err: HttpErrorResponse) => console.error(err),
    //   });


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
