import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TripModel } from 'src/app/interfaces/trip';
import { TripUserModel } from 'src/app/interfaces/trip-user-model';
import { UserModel } from 'src/app/interfaces/user-model';

@Component({
  selector: 'app-user-booked-trips',
  templateUrl: './user-booked-trips.component.html',
  styleUrls: ['./user-booked-trips.component.scss']
})
export class UserBookedTripsComponent implements OnInit {
  trips: TripModel[] = [];
  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.getUserBookedTrips();
  }

  getUserBookedTrips = () => {
    const url = 'https://localhost:6001/api/User/trips';
    this.http.get(url)
      .subscribe({
        next: (res: any) => {
          this.trips = res as TripModel[];
          console.log(this.trips);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      });


  }
  deleteBookedTrip = (tripUser: TripUserModel[]) => {
    const url = 'https://localhost:6001/api/BookedTrip/';
    console.log(tripUser)
    const options = {
      body: {
        tripUser: JSON.stringify(tripUser)
      },
    };

    this.http.delete(url, { body: tripUser })
      .subscribe({
        next: (res: any) => {
          this.trips = res as TripModel[];
          console.log(this.trips);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      });


  }
  deleteBookedSeat = (id: number) => {
    console.log(id);
    const url = 'https://localhost:6001/api/BookedTrip/';
    let params = new HttpParams();
    params = params.append('id', id);
    this.http.delete(url, { params })
      .subscribe({
        next: (res: any) => {
          this.trips = res as TripModel[];
          console.log(this.trips);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      });


  }
}
