import { HttpErrorResponse } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { TripOrderBy } from 'src/app/enums/trip-order-by';
import { SearchTripModel } from 'src/app/interfaces/trip-interfaces/search-trip';
import { TripsResponseModel } from 'src/app/interfaces/trip-interfaces/trips-response-model';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { TripService } from 'src/app/services/tripservice/trip.service';

@Component({
  selector: 'app-trips',
  templateUrl: './trips.component.html',
  styleUrls: ['./trips.component.scss']
})
export class TripsComponent implements OnInit {
  trips: TripsResponseModel = {
    trips: [],
    totalTrips: 0
  }
  trip: SearchTripModel = {
    countOfSeats: 1,
    endPlace: '',
    startPlace: '',
    startTime: new Date(),
    orderBy: TripOrderBy.EarliestDepartureTime
  };
  totalTrips = 0;
  private Skip: number = 0;
  private Take: number = 5;
  isTrips = true;
  isSpinner = false;
  public isFullListDisplayed: boolean = false;
  constructor(
    private router: Router,
    private sanitizer: DomSanitizer,
    private route: ActivatedRoute,
    private tripService: TripService,
    private imgSanitaze: ImgSanitizerService) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['startPlace'] && params['endPlace'] && params['startTime'] && params['seats']) {
        this.trip.startPlace = params['startPlace'];
        this.trip.endPlace = params['endPlace'];
        this.trip.startTime = new Date(params['startTime']);
        this.trip.countOfSeats = params['seats'];
        this.searchTrips();
        this.isSpinner = true;
      };
    });


  }
  navigateToTripPage = (id: number) => {
    const url = this.router.serializeUrl(
      this.router.createUrlTree(['trip-page-info', id], { queryParams: { requestedSeats: this.trip.countOfSeats } })
    );

    window.open(url, '_blank');
  }
  sanitizeUserImg(img: string): SafeUrl {
    return this.imgSanitaze.sanitiizeUserImg(img);
  }
  onScroll() {
    this.Skip += this.Take;
    if (this.Skip <= this.totalTrips) {
      this.searchTrips();
    }
    else {
      this.isFullListDisplayed = true;
    }
  }

  searchTrips(event?: TripOrderBy) {
    if (event != null) {
      this.Skip = 0;
      this.Take = 5;
      this.trip.orderBy = event;
    }
    else this.isSpinner = true;
    this.trip.startTime = new Date(this.trip.startTime).toDateString();
    this.trip.skip = this.Skip;
    this.trip.take = this.Take;

    // setTimeout(() => , 1000);
    this.tripService.SearchTrip(this.trip).pipe().subscribe(
      response => {
        this.isSpinner = false;
        if (response != null) {
          console.log(response);
          if (event != undefined) {
            this.trips.trips = response.trips;
            this.totalTrips = response.totalTrips;
          }
          else {
            this.trips.trips = this.trips.trips.concat(response.trips);
          }
          if (response.totalTrips > 0 && this.totalTrips == 0)
            this.totalTrips = response.totalTrips;
        }
        else this.isTrips = false;
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );

  }
}
