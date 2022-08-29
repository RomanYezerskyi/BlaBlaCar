import { HttpClient, HttpErrorResponse, HttpHandler, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { SearchTripModel } from 'src/app/interfaces/search-trip';
import { ActivatedRoute, Router } from '@angular/router';
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
  private Skip: number = 0;
  private Take: number = 5;
  trip: SearchTripModel = {
    countOfSeats: 1,
    endPlace: '',
    startPlace: '',
    startTime: new Date(),
    skip: this.Skip,
    take: this.Take
  };
  isParams = false;



  public isFullListDisplayed: boolean = false;

  constructor(private http: HttpClient, private router: Router, private sanitizer: DomSanitizer, private route: ActivatedRoute,) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  counterAdd() {
    if (this.trip.countOfSeats == 8) return;
    this.trip.countOfSeats += 1;
  }
  counterRemove() {
    if (this.trip.countOfSeats == 1) return;
    this.trip.countOfSeats -= 1;
  }
  sort() {
    this.router.navigate([], {
      queryParams: {
        orderBy: '123'
      },
      queryParamsHandling: 'merge',
    });
  }
  deleteSelectedSort() {
    this.router.navigate([], {
      queryParams: {
        orderBy: null
      },
      queryParamsHandling: 'merge',
    });
  }

  async ngOnInit() {
    this.route.queryParams.subscribe(params => {
      if (params['startPlace'] && params['endPlace'] && params['startTime'] && params['seats']) {
        this.trip.startPlace = params['startPlace'];
        this.trip.endPlace = params['endPlace'];
        this.trip.startTime = new Date(params['startTime']);
        this.trip.countOfSeats = params['seats'];
        this.isParams = true;
      }
    });
    if (this.isParams) {
      this.trip.take = this.Take;
      this.trip.skip = this.Skip;
      console.log(this.trip);
      var trips = await this.searchTrips();
      this.trips = trips == undefined ? [] : trips;
    }
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
  async onScroll() {
    this.trip.take = this.Take;
    if (this.trips.length == 0) {
      this.trip.skip = this.Skip;
      let searchTrips = await this.searchTrips();
      if (searchTrips != undefined) {
        this.trips = this.trips.concat(searchTrips);
      }
    }
    else if (this.Skip <= this.trips.length) {
      this.trip.skip = this.Skip;
      let searchTrips = await this.searchTrips();
      if (searchTrips != undefined) {
        this.trips = this.trips.concat(searchTrips);
      }
    }
    else {

      this.isFullListDisplayed = true;
    }
    this.Skip += this.Take;
  }


  async searchTrips(): Promise<TripModel[] | undefined> {
    console.log(this.trip);

    const trip = {
      countOfSeats: this.trip.countOfSeats,
      endPlace: this.trip.endPlace,
      startPlace: this.trip.startPlace,
      startTime: new Date(this.trip.startTime).toLocaleDateString(),
      take: this.Take,
      skip: this.Skip
    };
    const url = 'https://localhost:6001/api/Trips/search/'
    return await new Promise<TripModel[]>((resolve, reject) => {
      this.http.post<TripModel[]>(url, trip, {
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
