import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { AddTripModel } from 'src/app/interfaces/add-trip';
import { BookedTripModel } from 'src/app/interfaces/booked-trip';
import { SearchTripModel } from 'src/app/interfaces/search-trip';
import { TripModel } from 'src/app/interfaces/trip';
import { TripUserModel } from 'src/app/interfaces/trip-user-model';

@Injectable({
  providedIn: 'root'
})
export class TripService {

  constructor(private http: HttpClient) { }
  deleteBookedSeat(tripUser: TripUserModel): Observable<any> {
    const url = 'https://localhost:6001/api/BookedTrip/seat';
    return this.http.delete(url, { body: tripUser });
  }
  deleteBookedTrip(trip: TripModel): Observable<any> {
    const url = 'https://localhost:6001/api/BookedTrip/trip';
    return this.http.delete(url, { body: trip });
  }
  getUserBookedTrips(): Observable<TripModel[]> {
    const url = 'https://localhost:6001/api/BookedTrip/trips';
    return this.http.get<TripModel[]>(url);
  }

  deleteUserFromTrip(tripUser: { userId: string, tripId: number }): Observable<any> {
    const url = 'https://localhost:6001/api/BookedTrip/user';
    return this.http.delete(url, { body: tripUser });
  }

  bookSeatsInTrip(bookedtrip: BookedTripModel): Observable<any> {
    const url = 'https://localhost:6001/api/BookedTrip';
    return this.http.post(url, bookedtrip, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    });
  }
  //
  getUserTrips(): Observable<TripModel[]> {
    const url = 'https://localhost:6001/api/Trips/user-trips';
    return this.http.get<TripModel[]>(url)
  }
  getTripById(tripId: number): Observable<TripModel> {
    const url = 'https://localhost:6001/api/Trips/'
    return this.http.get<TripModel>(url + tripId);
  }
  SearchTrip(trip: SearchTripModel): Observable<TripModel[]> {
    const url = 'https://localhost:6001/api/Trips/search/'
    return this.http.post<TripModel[]>(url, trip, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    });
  }
  addNewtrip(trip: AddTripModel): Observable<any> {
    const url = 'https://localhost:6001/api/Trips/'
    return this.http.post(url, trip);
  }
  deleteTrip(id: number): Observable<any> {
    const url = 'https://localhost:6001/api/Trips/trip/';
    return this.http.delete(url + id);
  }
}
