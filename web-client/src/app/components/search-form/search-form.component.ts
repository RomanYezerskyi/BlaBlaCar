import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TripOrderBy } from 'src/app/enums/trip-order-by';
import { SearchTripModel } from 'src/app/interfaces/trip-interfaces/search-trip-model';
import { PlaceSuggestion } from '../maps-autocomplete/maps-autocomplete.component';

@Component({
  selector: 'app-search-form',
  templateUrl: './search-form.component.html',
  styleUrls: ['./search-form.component.scss']
})
export class SearchFormComponent implements OnInit {
  @Input() trip: SearchTripModel = { countOfSeats: 1 } as SearchTripModel;
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
    this.router.navigate(['search'], {
      queryParams: {
        startPlace: "Lviv", //this.trip.startPlace,
        endPlace: "Kyiv",//this.trip.endPlace,
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
  startPlaceChanged(value: PlaceSuggestion) {
    this.trip.startLat = value.data.lat;
    this.trip.startLon = value.data.lon;
    console.log(value);
  }
  endPlaceChanged(value: PlaceSuggestion) {
    this.trip.endLat = value.data.lat;
    this.trip.endLon = value.data.lon;
    console.log(value);
  }
}


