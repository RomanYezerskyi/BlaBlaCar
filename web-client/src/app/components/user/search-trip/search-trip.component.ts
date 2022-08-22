import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { SearchTripModel } from 'src/app/interfaces/search-trip';
import { Router } from '@angular/router';
import { TripModel } from 'src/app/interfaces/trip';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
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

  private noOfItemsToShowInitially: number = 5;
  // itemsToLoad - number of new items to be displayed
  private itemsToLoad: number = 5;
  // 18 items loaded for demo purposes
  // List that is going to be actually displayed to user
  public itemsToShow = this.trips.slice(0, this.noOfItemsToShowInitially);
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

    if (this.noOfItemsToShowInitially <= this.trips.length || this.trips.length == 0) {
      console.log("aa");
      this.trip.take = this.itemsToLoad;
      this.trip.skip = this.trips.length == 0 ? 0 : this.noOfItemsToShowInitially;
      if (this.trips.length != 0) {
        this.noOfItemsToShowInitially += this.itemsToLoad;
      }

      let searchTrips = await this.searchTrips(form);
      if (searchTrips != undefined) {
        this.trips = this.trips.concat(searchTrips);
      }
      this.itemsToShow = this.trips.slice(0, this.noOfItemsToShowInitially);
      console.log(this.itemsToShow);
    } else {

      this.isFullListDisplayed = true;
    }
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
