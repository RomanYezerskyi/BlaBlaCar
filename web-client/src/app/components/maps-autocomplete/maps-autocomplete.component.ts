import { Component, Output, EventEmitter, OnDestroy, OnInit } from '@angular/core';
// import { MatOptionSelectionChange } from '@angular/material/se';
import { Subject, Subscription, takeUntil } from 'rxjs';
import { FormControl } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { MatOptionSelectionChange } from '@angular/material/core';
import { MapsService } from 'src/app/services/maps-service/maps.service';


export interface PlaceSuggestion {
  shortAddress: string;
  fullAddress: string;
  data: GeocodingFeatureProperties;
}

export interface GeocodingFeatureProperties {
  name: string;
  address_line2: string
  address_line1: string
  country: string;
  state: string;
  postcode: string;
  city: string;
  street: string;
  housenumber: string;
  lat: number;
  lon: number;
  formatted: string;
}
@Component({
  selector: 'app-maps-autocomplete',
  templateUrl: './maps-autocomplete.component.html',
  styleUrls: ['./maps-autocomplete.component.scss']
})
export class MapsAutocompleteComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  @Output()
  locationChange: EventEmitter<PlaceSuggestion> = new EventEmitter<PlaceSuggestion>();
  searchOptions: Subject<PlaceSuggestion[]> = new Subject<PlaceSuggestion[]>();
  inputFieldFormControl: FormControl = new FormControl();

  private choosenOption!: PlaceSuggestion;

  private userInputTimeout!: number;
  constructor(private mapsService: MapsService) {
    this.inputFieldFormControl.valueChanges.pipe(takeUntil(this.unsubscribe$)).subscribe((value) => {
      if (this.userInputTimeout) {
        window.clearTimeout(this.userInputTimeout);
      }

      if (this.choosenOption && this.choosenOption.shortAddress === value) {
        this.searchOptions.next([]);
        return;
      }

      if (!value || value.length < 3) {
        // do not need suggestions until for less than 3 letters
        this.searchOptions.next([]);
        return;
      }

      this.userInputTimeout = window.setTimeout(() => {
        this.generateSuggestions(value);
      }, 300);
    });
  }

  ngOnInit(): void {
  }
  ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  private generateSuggestions(text: string) {
    this.mapsService.getPlace(text).pipe(takeUntil(this.unsubscribe$)).subscribe((data: any /*GeoJSON.FeatureCollection*/) => {
      const placeSuggestions = data.features.map((feature: { properties: GeocodingFeatureProperties; }) => {
        const properties: GeocodingFeatureProperties = (feature.properties as GeocodingFeatureProperties);

        return {
          shortAddress: this.mapsService.generateShortAddress(properties),
          fullAddress: this.mapsService.generateFullAddress(properties),
          data: properties
        }
      });
      console.log(placeSuggestions);
      this.searchOptions.next(placeSuggestions.length ? placeSuggestions : null);
    }, err => {
      console.log(err);
    });

  }

  public optionSelectionChange(option: PlaceSuggestion, event: MatOptionSelectionChange) {
    if (event.isUserInput) {
      this.choosenOption = option;
      this.locationChange.emit(option);
    }
  }
}
