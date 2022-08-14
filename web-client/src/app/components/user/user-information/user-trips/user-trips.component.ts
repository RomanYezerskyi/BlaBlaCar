import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TripModel } from 'src/app/interfaces/trip';
import { TripUserModel } from 'src/app/interfaces/trip-user-model';

@Component({
  selector: 'app-user-trips',
  templateUrl: './user-trips.component.html',
  styleUrls: ['./user-trips.component.scss']
})
export class UserTripsComponent implements OnInit {
  trips: TripModel[] = [];
  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.getUserTrips();
  }
  deleteUserFromTrip = (userId: string, tripId: number) => {
    const url = 'https://localhost:6001/api/BookedTrip/user';
    let tripUser = {
      userId: userId,
      tripId: tripId
    };
    this.http.delete(url, { body: tripUser })
      .subscribe({
        next: (res: any) => {
          console.log(res);
          this.getUserTrips();
        },
        error: (err: HttpErrorResponse) => console.error(err),
      });
  }
  deleteTrip = (id: number) => {
    const url = 'https://localhost:6001/api/Trips/trip/';
    this.http.delete(url + id)
      .subscribe({
        next: (res: any) => {
          console.log(res);
          this.getUserTrips();
        },
        error: (err: HttpErrorResponse) => console.error(err),
      });
  }
  getUserTrips = () => {
    const url = 'https://localhost:6001/api/Trips/user-trips';
    this.http.get(url)
      .subscribe({
        next: (res: any) => {
          this.trips = res as TripModel[];
          // console.log(this.trips);
          console.log(res);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      });
  }


}
