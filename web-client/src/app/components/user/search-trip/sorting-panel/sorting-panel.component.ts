import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { TripOrderBy } from 'src/app/enums/trip-order-by';
import { SearchTripModel } from 'src/app/interfaces/trip-interfaces/search-trip-model';

@Component({
  selector: 'app-sorting-panel',
  templateUrl: './sorting-panel.component.html',
  styleUrls: ['./sorting-panel.component.scss']
})
export class SortingPanelComponent implements OnInit {
  @Output() OrderByOutput: EventEmitter<TripOrderBy> = new EventEmitter<TripOrderBy>;
  @Input() trip: SearchTripModel = { orderBy: TripOrderBy.EarliestDepartureTime } as SearchTripModel;
  orderBy = TripOrderBy;
  constructor() { }

  ngOnInit(): void {
  }
  orderByParams() {
    this.OrderByOutput.emit(this.trip.orderBy);
  }
  deleteSelectedSort() {
    this.OrderByOutput.emit(TripOrderBy.EarliestDepartureTime);
  }
}
