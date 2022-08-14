import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { SearchTripModel } from 'src/app/interfaces/search-trip';
import { Router } from '@angular/router';
import { TripModel } from 'src/app/interfaces/trip';
@Component({
  selector: 'app-search-trip',
  templateUrl: './search-trip.component.html',
  styleUrls: ['./search-trip.component.scss']
})
export class SearchTripComponent implements OnInit {
  invalidForm: boolean | undefined;
  data: TripModel[] = []
  trip: SearchTripModel = {
    countOfSeats: 1,
    endPlace: '',
    startPlace: '',
    startTime: new Date('1991.01.01'),
  };
  tripRoute: { id: number } | undefined;


  constructor(private http: HttpClient, private router: Router) { }

  counterAdd() {
    if (this.trip.countOfSeats == 8) return;
    this.trip.countOfSeats += 1;
  }
  counterRemove() {
    if (this.trip.countOfSeats == 1) return;
    this.trip.countOfSeats -= 1;
  }

  ngOnInit(): void {

  }
  searchTrips = (form: NgForm) => {
    if (form.valid) {
      const url = 'https://localhost:6001/api/Trips/search/'
      this.http.post(url, this.trip, {
        headers: new HttpHeaders({ "Content-Type": "application/json" })
      })
        .subscribe({
          next: (res) => {
            this.data = res as TripModel[];
            console.log(this.data);
          },
          error: (err: HttpErrorResponse) => console.error(err),
        })
    }
  }

  navigateToTripPage = (id: number) => {
    this.router.navigate(['trip-page-info', id], { queryParams: { requestedSeats: this.trip.countOfSeats } })
  }
}
