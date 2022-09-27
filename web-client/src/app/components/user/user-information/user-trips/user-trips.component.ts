import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { UserTrips, UserTripsResponseModel } from 'src/app/interfaces/user-interfaces/user-trips-response-model';
import { TripService } from 'src/app/services/tripservice/trip.service';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { skip, Subject, Subscription, takeUntil } from 'rxjs';
import { ChatService } from 'src/app/services/chatservice/chat.service';
import { ActivatedRoute, Router } from '@angular/router';
import { GeocodingFeatureProperties, PlaceSuggestion } from 'src/app/components/maps-autocomplete/maps-autocomplete.component';
import { TripModel } from 'src/app/interfaces/trip-interfaces/trip-model';

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
  private requestSub!: Subscription;
  searchOptions: Subject<PlaceSuggestion[]> = new Subject<PlaceSuggestion[]>();
  constructor(private http: HttpClient,
    private imgSanitaze: ImgSanitizerService, private router: Router,
    private tripService: TripService, private chatService: ChatService) { }

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
      this.tripService.getUserTrips(this.Take, this.Skip).pipe(takeUntil(this.unsubscribe$)).subscribe(
        response => {
          if (response != null) {
            response.trips.forEach(
              x => {
                if (x.startLat! > 0) {
                  this.generateSuggestions(x);
                }
              }
            )
            this.trips.trips = this.trips.trips.concat(response.trips);
            console.log(this.trips);
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
  deleteUserFromTrip(userId: string, tripId: number): void {
    let tripUser = {
      userId: userId,
      tripId: tripId
    };
    this.tripService.deleteUserFromTrip(tripUser).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.ngOnInit();
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
  }

  deleteTrip(id: number): void {
    this.tripService.deleteTrip(id).pipe().subscribe(
      response => {
        this.Skip = 0;
        this.totalTrips = 0;
        this.getUserTrips();

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
  private generateSuggestions(trip: TripModel) {
    const text = `${trip.startLat}%20${trip.startLon}`;
    console.log(text)
    const url = `https://api.geoapify.com/v1/geocode/search?text=${text}&apiKey=32849d6b3d3b480c9a60be1ce5891252`;

    if (this.requestSub) {
      this.requestSub.unsubscribe();
    }

    this.requestSub = this.http.get(url).subscribe((data: any /*GeoJSON.FeatureCollection*/) => {
      const placeSuggestions = data.features.map((feature: { properties: GeocodingFeatureProperties; }) => {
        const properties: GeocodingFeatureProperties = (feature.properties as GeocodingFeatureProperties);

        return {
          shortAddress: this.generateShortAddress(properties),
          fullAddress: this.generateFullAddress(properties),
          data: properties
        }
      });
      console.log(placeSuggestions);
      trip.startPlace = placeSuggestions[0].data.formatted;
      this.searchOptions.next(placeSuggestions.length ? placeSuggestions : null);
    }, err => {
      console.log(err);
    });
  }
  private generateShortAddress(properties: GeocodingFeatureProperties): string {
    let shortAddress = properties.name;

    if (!shortAddress && properties.street && properties.housenumber) {
      // name is not set for buildings
      shortAddress = `${properties.street} ${properties.housenumber}`;
    }

    shortAddress += (properties.postcode && properties.city) ? `, ${properties.postcode}-${properties.city}` : '';
    shortAddress += (!properties.postcode && properties.city && properties.city !== properties.name) ? `, ${properties.city}` : '';
    shortAddress += (properties.country && properties.country !== properties.name) ? `, ${properties.country}` : '';

    return shortAddress;
  }
  private generateFullAddress(properties: GeocodingFeatureProperties): string {
    let fullAddress = properties.name;
    fullAddress += properties.street ? `, ${properties.street}` : '';
    fullAddress += properties.housenumber ? ` ${properties.housenumber}` : '';
    fullAddress += (properties.postcode && properties.city) ? `, ${properties.postcode}-${properties.city}` : '';
    fullAddress += (!properties.postcode && properties.city && properties.city !== properties.name) ? `, ${properties.city}` : '';
    fullAddress += properties.state ? `, ${properties.state}` : '';
    fullAddress += (properties.country && properties.country !== properties.name) ? `, ${properties.country}` : '';
    return fullAddress;
  }
}
