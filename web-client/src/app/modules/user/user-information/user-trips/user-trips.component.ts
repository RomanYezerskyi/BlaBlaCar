import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { UserTrips, UserTripsResponseModel } from 'src/app/core/models/user-models/user-trips-response-model';
import { TripService } from 'src/app/core/services/trip-service/trip.service';
import { ImgSanitizerService } from 'src/app/core/services/image-sanitizer-service/img-sanitizer.service';
import { skip, Subject, Subscription, takeUntil } from 'rxjs';
import { ChatService } from 'src/app/core/services/chat-service/chat.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TripModel } from 'src/app/core/models/trip-models/trip-model';
import { MapsService } from 'src/app/core/services/maps-service/maps.service';
import { GeocodingFeatureProperties } from 'src/app/core/models/autocomplete-models/place-suggestion-model';
import { AlertsComponent } from 'src/app/modules/shared/components/alerts/alerts.component';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-user-trips',
  templateUrl: './user-trips.component.html',
  styleUrls: ['./user-trips.component.scss']
})
export class UserTripsComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  trips: UserTripsResponseModel = { trips: [] = [], totalTrips: 0 };
  public isFullListDisplayed: boolean = false;
  totalTrips: number = 0;
  private Skip: number = 0;
  private Take: number = 5;
  isSpinner: boolean = true;
  constructor(private _snackBar: MatSnackBar,
    private imgSanitaze: ImgSanitizerService, private router: Router,
    private tripService: TripService, private chatService: ChatService, private mapsService: MapsService) { }

  ngOnInit(): void {
    this.getUserTrips();

  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  sanitizeUserImg(img: string): SafeUrl {
    return this.imgSanitaze.sanitiizeUserImg(img);
  }
  getUserTrips(): void {
    console.log(this.Skip);
    if (this.Skip <= this.totalTrips) {
      this.isSpinner = true;
      this.tripService.getUserTrips(this.Take, this.Skip).pipe(takeUntil(this.unsubscribe$)).subscribe(
        response => {
          if (response != null) {
            response.trips.forEach(
              x => {
                if (x.startLat! > 0) {
                  this.getPlaces(x);
                }
              }
            )
            this.trips.trips = this.trips.trips.concat(response.trips);
            console.log(this.trips);
            if (this.totalTrips == 0)
              this.totalTrips = response.totalTrips!;
          }
          this.isSpinner = false;
        },
        (error: HttpErrorResponse) => { console.log(error.error); this.isSpinner = false; }
      );
    }
    else {
      this.isFullListDisplayed = true;
    }
    this.Skip += this.Take;
  }
  deleteUserFromTrip(userId: string, tripId: number): void {
    let tripUser = {
      userId: userId,
      tripId: tripId
    };
    this.tripService.deleteUserFromTrip(tripUser).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.trips.trips.forEach(x => {
          if (x.id == tripId) { x.tripUsers = x.bookedTripUsers.filter(u => u.userId != userId); }
        });
        this.openSnackBar("User removed form trip!");
      },
      (error: HttpErrorResponse) => { console.log(error.error); this.openSnackBar(error.error); }
    );
  }

  deleteTrip(id: number): void {
    this.tripService.deleteTrip(id).pipe().subscribe(
      response => {
        this.trips.trips = this.trips.trips.filter(x => x.id != id);
        this.openSnackBar("Trip delete!");
      },
      (error: HttpErrorResponse) => { console.log(error.error); this.openSnackBar(error.error); }
    );
  }
  isTripCompleted(endDate: Date): boolean {
    if (new Date(endDate) < new Date()) return true;
    return false
  }
  getChat(userId: string) {
    this.chatService.GetPrivateChat(userId).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.router.navigate(['chat'], {
          queryParams: {
            chatId: response
          }
        });
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }

  private openSnackBar(message: string) {
    this._snackBar.open(message, "Close");
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
