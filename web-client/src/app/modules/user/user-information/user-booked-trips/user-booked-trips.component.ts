import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { TripModel } from 'src/app/core/models/trip-models/trip-model';
import { TripUserModel } from 'src/app/core/models/trip-models/trip-user-model';
import { TripsResponseModel } from 'src/app/core/models/trip-models/trips-response-model';
import { UserModel } from 'src/app/core/models/user-models/user-model';
import { ChatService } from 'src/app/core/services/chat-service/chat.service';
import { ImgSanitizerService } from 'src/app/core/services/image-sanitizer-service/img-sanitizer.service';
import { MapsService } from 'src/app/core/services/maps-service/maps.service';
import { TripService } from 'src/app/core/services/trip-service/trip.service';
import { GeocodingFeatureProperties } from 'src/app/core/models/autocomplete-models/place-suggestion-model';

@Component({
  selector: 'app-user-booked-trips',
  templateUrl: './user-booked-trips.component.html',
  styleUrls: ['./user-booked-trips.component.scss']
})
export class UserBookedTripsComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  trips: TripsResponseModel = { totalTrips: 0, trips: [] = [] };
  public isFullListDisplayed: boolean = false;
  totalTrips!: number;
  private Skip!: number;
  private Take!: number;
  constructor(
    private tripService: TripService, private router: Router,
    private imgSanitaze: ImgSanitizerService, private chatService: ChatService, private mapsService: MapsService) { }

  ngOnInit(): void {
    this.totalTrips = 0;
    this.Skip = 0;
    this.Take = 5;
    this.getUserBookedTrips();
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  sanitizeUserImg(img: string): SafeUrl {
    return this.imgSanitaze.sanitiizeUserImg(img);
  }

  getUserBookedTrips(): void {
    if (this.Skip <= this.totalTrips) {
      this.tripService.getUserBookedTrips(this.Take, this.Skip).pipe(takeUntil(this.unsubscribe$)).subscribe(
        response => {
          console.log(response);
          if (response != null) {
            response.trips.forEach(
              x => {
                if (x.startLat! > 0) {
                  this.getPlaces(x);
                }
              });
            this.trips.trips = this.trips.trips.concat(response.trips);
            if (this.totalTrips == 0)
              this.totalTrips = response.totalTrips!;
          }
        },
        (error: HttpErrorResponse) => { console.log(error.error); }
      );
    }
    else {
      this.isFullListDisplayed = true;
    }
    this.Skip += this.Take;
  }
  deleteBookedTrip(trip: TripModel): void {
    this.tripService.deleteBookedTrip(trip).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.trips.trips = this.trips.trips.filter(x => x.id != trip.id);
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
  }
  deleteBookedSeat(tripUser: TripUserModel): void {
    this.tripService.deleteBookedSeat(tripUser).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.ngOnInit();
        console.log(response)
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
  }
  isTripCompleted(endDate: Date): boolean {

    if (new Date(endDate) < new Date()) return true;
    return false
  }
  getChat(userId: string) {
    this.chatService.GetPrivateChat(userId).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.router.navigate(['/chat'], {
          queryParams: {
            chatId: response
          }
        });
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
  private getPlaces(trip: TripModel) {
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
