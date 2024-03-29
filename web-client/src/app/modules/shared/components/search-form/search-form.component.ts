import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TripOrderBy } from 'src/app/core/enums/trip-order-by';
import { PlaceSuggestionModel } from 'src/app/core/models/autocomplete-models/place-suggestion-model';
import { SearchTripModel } from 'src/app/core/models/trip-models/search-trip-model';


@Component({
  selector: 'app-search-form',
  templateUrl: './search-form.component.html',
  styleUrls: ['./search-form.component.scss']
})
export class SearchFormComponent implements OnInit {
  @Input() trip: SearchTripModel = { countOfSeats: 1 } as SearchTripModel;
  dateNow: string = new Date().toISOString();
  counterActive: boolean = false;
  constructor(private router: Router,) { }

  ngOnInit(): void {
  }
  counterAcive(): void {
    this.counterActive = !this.counterActive;
  }
  counterAdd(): void {
    if (this.trip.countOfSeats == 8) return;
    this.trip.countOfSeats = +this.trip.countOfSeats + 1;
  }
  counterRemove(): void {
    if (this.trip.countOfSeats == 1) return;
    this.trip.countOfSeats = +this.trip.countOfSeats - 1;
  }
  search(): void {
    this.router.navigate(['user/search'], {
      queryParams: {
        startPlace: this.trip.startPlace,
        endPlace: this.trip.endPlace,
        startTime: this.trip.startTime,
        seats: this.trip.countOfSeats,
        orderBy: TripOrderBy.EarliestDepartureTime,
        startLat: this.trip.startLat,
        startLon: this.trip.startLon,
        endLat: this.trip.endLat,
        endLon: this.trip.endLon
      }
    });
  }
  startPlaceChanged(value: PlaceSuggestionModel) {
    this.trip.startLat = value.data.lat;
    this.trip.startLon = value.data.lon;
    this.trip.startPlace = value.data.formatted
  }
  endPlaceChanged(value: PlaceSuggestionModel) {
    this.trip.endLat = value.data.lat;
    this.trip.endLon = value.data.lon;
    this.trip.endPlace = value.data.formatted
  }
}


