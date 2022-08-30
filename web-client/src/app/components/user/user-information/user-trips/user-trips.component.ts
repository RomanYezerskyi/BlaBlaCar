import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { TripModel } from 'src/app/interfaces/trip';
import { TripUserModel } from 'src/app/interfaces/trip-user-model';
import { TripService } from 'src/app/services/tripservice/trip.service';

@Component({
  selector: 'app-user-trips',
  templateUrl: './user-trips.component.html',
  styleUrls: ['./user-trips.component.scss']
})
export class UserTripsComponent implements OnInit {
  trips: TripModel[] = [];
  constructor(private http: HttpClient,
    private router: Router,
    private sanitizer: DomSanitizer,
    private tripService: TripService) { }

  ngOnInit(): void {
    this.getUserTrips();
  }
  sanitaizeImg(img: string): SafeUrl {
    console.log(img);
    return this.sanitizer.bypassSecurityTrustUrl(img);
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
  getUserTrips = () => {
    this.tripService.getUserTrips().pipe().subscribe(
      response => {
        this.trips = response;
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
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


}
