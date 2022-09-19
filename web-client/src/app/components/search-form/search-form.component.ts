import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TripOrderBy } from 'src/app/enums/trip-order-by';
import { SearchTripModel } from 'src/app/interfaces/trip-interfaces/search-trip-model';

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
        startPlace: this.trip.startPlace,
        endPlace: this.trip.endPlace,
        startTime: this.trip.startTime,
        seats: this.trip.countOfSeats,
        orderBy: TripOrderBy.EarliestDepartureTime,
      }
    });
  }

}


