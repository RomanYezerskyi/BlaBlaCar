import { HttpErrorResponse } from '@angular/common/http';
import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { TripOrderBy } from 'src/app/core/enums/trip-order-by';
import { SearchTripModel } from 'src/app/core/models/trip-models/search-trip-model';
import { TripModel } from 'src/app/core/models/trip-models/trip-model';
import { TripsResponseModel } from 'src/app/core/models/trip-models/trips-response-model';
import { ImgSanitizerService } from 'src/app/core/services/image-sanitizer-service/img-sanitizer.service';
import { MapsService } from 'src/app/core/services/maps-service/maps.service';
import { TripService } from 'src/app/core/services/trip-service/trip.service';
import { GeocodingFeatureProperties } from 'src/app/core/models/autocomplete-models/place-suggestion-model';

@Component({
  selector: 'app-trips',
  templateUrl: './trips.component.html',
  styleUrls: ['./trips.component.scss']
})
export class TripsComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  trips: TripsResponseModel = {} as TripsResponseModel;
  trip: SearchTripModel = { orderBy: TripOrderBy.EarliestDepartureTime } as SearchTripModel;
  totalTrips: number = 0;
  private Skip: number = 0;
  private Take: number = 5;
  isTrips: boolean = true;
  isSpinner: boolean = false;
  public isFullListDisplayed: boolean = false;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private tripService: TripService,
    private imgSanitaze: ImgSanitizerService,
    private mapsService: MapsService) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    this.route.queryParams.pipe(takeUntil(this.unsubscribe$)).subscribe(params => {
      if (params['startPlace'] && params['endPlace'] && params['startTime'] && params['seats']) {
        this.trip.startPlace = params['startPlace'];
        this.trip.endPlace = params['endPlace'];
        this.trip.startTime = new Date(params['startTime']);
        this.trip.countOfSeats = params['seats'];
        this.trip.startLat = params['startLat'];
        this.trip.startLon = params['startLon'];
        this.trip.endLat = params['endLat'];
        this.trip.endLon = params['endLon'];
        this.searchTrips();
        this.isSpinner = true;
      };
    });
    this.trips.trips = [];
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  navigateToTripPage(id: number): void {
    const url = this.router.serializeUrl(
      this.router.createUrlTree(['trip-page-info', id], { queryParams: { requestedSeats: this.trip.countOfSeats } })
    );
    window.open(url, '_blank');
  }
  sanitizeUserImg(img: string): SafeUrl {
    return this.imgSanitaze.sanitiizeUserImg(img);
  }
  onScroll(): void {
    this.Skip += this.Take;
    if (this.Skip <= this.trips.trips.length) {
      this.searchTrips();
    }
    else {
      this.isFullListDisplayed = true;
    }
  }

  searchTrips(event?: TripOrderBy): void {
    if (event != null) {
      this.Skip = 0;
      this.Take = 5;
      this.trip.orderBy = event;
    }
    else this.isSpinner = true;
    this.trip.startTime = new Date(this.trip.startTime).toDateString();
    this.trip.skip = this.Skip;
    this.trip.take = this.Take;
    this.tripService.SearchTrip(this.trip).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.isSpinner = false;
        console.log(response);
        if (response != null) {
          response.trips.forEach(x => { x.startLat! > 0 ? this.getPlaces(x) : x; })
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
        else { this.isTrips = false; }
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );

  }
  private getPlaces(trip: TripModel | SearchTripModel) {
    let text = `${trip.startLat}%20${trip.startLon}`;
    this.mapsService.getPlaceName(text).pipe(takeUntil(this.unsubscribe$)).subscribe((data: any) => {
      const placeSuggestions = data.features.map((feature: { properties: GeocodingFeatureProperties; }) => {
        const properties: GeocodingFeatureProperties = (feature.properties as GeocodingFeatureProperties);
        return {
          shortAddress: this.mapsService.generateShortAddress(properties),
          fullAddress: this.mapsService.generateFullAddress(properties),
          data: properties
        }
      });
      trip.startPlace = placeSuggestions[0].data.city;
      console.log(placeSuggestions);
    }, err => {
      console.log(err);
    });
    text = `${trip.endLat}%20${trip.endLon}`;
    this.mapsService.getPlaceName(text).pipe(takeUntil(this.unsubscribe$)).subscribe((data: any) => {
      const placeSuggestions = data.features.map((feature: { properties: GeocodingFeatureProperties; }) => {
        const properties: GeocodingFeatureProperties = (feature.properties as GeocodingFeatureProperties);
        return {
          shortAddress: this.mapsService.generateShortAddress(properties),
          fullAddress: this.mapsService.generateFullAddress(properties),
          data: properties
        }
      });
      trip.endPlace = placeSuggestions[0].data.city;
      console.log(placeSuggestions);
    }, err => {
      console.log(err);
    });
  }
}
