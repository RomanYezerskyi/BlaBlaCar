import { HttpClient, HttpErrorResponse, HttpHandler, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { SearchTripModel } from 'src/app/interfaces/search-trip';
import { ActivatedRoute, Router } from '@angular/router';
import { TripModel } from 'src/app/interfaces/trip';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Call } from '@angular/compiler';
import { TripService } from 'src/app/services/tripservice/trip.service';
import { Observable } from 'rxjs';
import { SearchTripsResponseModel } from 'src/app/interfaces/search-trips-response-model';
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
  // trips: TripModel[] = [];
  totalTrips = 0;
  trips: SearchTripsResponseModel = {
    trips: [],
    totalTrips: 0
  }
  private Skip: number = 0;
  private Take: number = 5;
  trip: SearchTripModel = {
    countOfSeats: 1,
    endPlace: '',
    startPlace: '',
    startTime: new Date(),
  };
  isParams = false;
  public isFullListDisplayed: boolean = false;
  constructor(private http: HttpClient,
    private router: Router,
    private sanitizer: DomSanitizer,
    private route: ActivatedRoute,
    private tripService: TripService) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
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
    console.log(this.trip.startTime)
    if (this.isParams) {
      console.log(this.trip);
      this.searchTrips();
    }
  }

  navigateToTripPage = (id: number) => {
    const url = this.router.serializeUrl(
      this.router.createUrlTree(['trip-page-info', id], { queryParams: { requestedSeats: this.trip.countOfSeats } })
    );

    window.open(url, '_blank');
  }

  sanitaizeImg(img: string): SafeUrl | undefined {
    if (img != null)
      return this.sanitizer.bypassSecurityTrustUrl(img);
    return undefined;
  }
  async onScroll() {
    this.Skip += this.Take;
    if (this.Skip <= this.totalTrips) {
      this.searchTrips();
    }
    else {
      this.isFullListDisplayed = true;
    }

  }
  searchTrips() {
    const request = {
      countOfSeats: this.trip.countOfSeats,
      endPlace: this.trip.endPlace,
      startPlace: this.trip.startPlace,
      startTime: new Date(this.trip.startTime).toDateString(),
      take: this.Take,
      skip: this.Skip
    };
    this.tripService.SearchTrip(request).pipe().subscribe(
      response => {
        console.log(response);
        this.trips.trips = this.trips.trips.concat(response.trips);
        if (response.totalTrips > 0 && this.totalTrips == 0)
          this.totalTrips = response.totalTrips;

      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    )
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
  // async searchTrips(): Promise<TripModel[] | undefined> {
  //   console.log(this.trip);

  //   const trip = {
  //     countOfSeats: this.trip.countOfSeats,
  //     endPlace: this.trip.endPlace,
  //     startPlace: this.trip.startPlace,
  //     startTime: new Date(this.trip.startTime).toDateString() + 'Z',
  //     take: this.Take,
  //     skip: this.Skip
  //   };
  //   console.log(trip.startTime);
  //   const url = 'https://localhost:6001/api/Trips/search/'
  //   return await new Promise<TripModel[]>((resolve, reject) => {
  //     this.http.post<TripModel[]>(url, trip, {
  //       headers: new HttpHeaders({ "Content-Type": "application/json" })
  //     }).subscribe({
  //       next: (res) => {
  //         console.log(res);
  //         resolve(res);
  //       },
  //       error: (err: HttpErrorResponse) => { console.error(err); reject(err) },
  //     });
  //   });

  // }


}
