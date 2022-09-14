import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { AddTripModel } from 'src/app/interfaces/trip-interfaces/add-trip';
import { BookedTripModel } from 'src/app/interfaces/trip-interfaces/booked-trip';
import { SearchTripModel } from 'src/app/interfaces/trip-interfaces/search-trip';
import { TripsResponseModel } from 'src/app/interfaces/trip-interfaces/trips-response-model';
import { TripModel } from 'src/app/interfaces/trip-interfaces/trip-model';
import { TripUserModel } from 'src/app/interfaces/trip-interfaces/trip-user-model';
import { UserTrips, UserTripsResponse } from 'src/app/interfaces/user-interfaces/user-trips';

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
  getUserBookedTrips(take: number, skip: number): Observable<TripsResponseModel> {
    const url = 'https://localhost:6001/api/BookedTrip/trips/' + take + "/" + skip;
    return this.http.get<TripsResponseModel>(url);
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
  getUserTrips(take: number, skip: number): Observable<UserTripsResponse> {
    const url = 'https://localhost:6001/api/Trips/user-trips/' + take + "/" + skip;
    return this.http.get<UserTripsResponse>(url)
  }
  getTripById(tripId: number): Observable<TripModel> {
    const url = 'https://localhost:6001/api/Trips/'
    return this.http.get<TripModel>(url + tripId);
  }
  SearchTrip(trip: SearchTripModel): Observable<TripsResponseModel> {
    const url = 'https://localhost:6001/api/Trips/search/'
    return this.http.post<TripsResponseModel>(url, trip, {
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
