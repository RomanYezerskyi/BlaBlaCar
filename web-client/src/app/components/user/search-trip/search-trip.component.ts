import { HttpClient, HttpErrorResponse, HttpHandler, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { SearchTripModel } from 'src/app/interfaces/search-trip';
import { Router } from '@angular/router';
import { TripModel } from 'src/app/interfaces/trip';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Call } from '@angular/compiler';
export interface GetItems<T> {
  (take: number, skip: number): T[];
}
@Component({
  selector: 'app-search-trip',
  templateUrl: './search-trip.component.html',
  styleUrls: ['./search-trip.component.scss']
})


export class SearchTripComponent implements OnInit {
  invalidForm: boolean | undefined;
  trips: TripModel[] = [];
  trip: SearchTripModel = {
    countOfSeats: 1,
    endPlace: '',
    startPlace: '',
    startTime: new Date('1991.01.01'),

  };
  tripRoute: { id: number } | undefined;

  private Skip: number = 0;
  // itemsToLoad - number of new items to be displayed
  private Take: number = 5;
  // 18 items loaded for demo purposes
  // List that is going to be actually displayed to user
  // No need to call onScroll if full list has already been displayed
  public isFullListDisplayed: boolean = false;

  constructor(private http: HttpClient, private router: Router, private sanitizer: DomSanitizer) { }

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

  navigateToTripPage = (id: number) => {
    const url = this.router.serializeUrl(
      this.router.createUrlTree(['trip-page-info', id], { queryParams: { requestedSeats: this.trip.countOfSeats } })
    );

    window.open(url, '_blank');
  }

  sanitaizeImg(img: string): SafeUrl {

    return this.sanitizer.bypassSecurityTrustUrl(img);
  }
  async onScroll(form: NgForm) {
    this.trip.take = this.Take;
    if (this.trips.length == 0) {
      this.trip.skip = this.Skip;
      let searchTrips = await this.searchTrips(form);
      if (searchTrips != undefined) {
        this.trips = this.trips.concat(searchTrips);
      }
    }
    else if (this.Skip <= this.trips.length) {
      this.trip.skip = this.Skip;
      let searchTrips = await this.searchTrips(form);
      if (searchTrips != undefined) {
        this.trips = this.trips.concat(searchTrips);
      }
    }
    else {

      this.isFullListDisplayed = true;
    }
    this.Skip += this.Take;
  }


  async searchTrips(form: NgForm): Promise<TripModel[] | undefined> {
    if (!form.valid) { return; }
    const url = 'https://localhost:6001/api/Trips/search/'
    return await new Promise<TripModel[]>((resolve, reject) => {
      this.http.post<TripModel[]>(url, this.trip, {
        headers: new HttpHeaders({ "Content-Type": "application/json" })
      }).subscribe({
        next: (res) => {
          console.log(res);
          resolve(res);
        },
        error: (err: HttpErrorResponse) => { console.error(err); reject(err) },
      });
    });

  }


}
