import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { TripOrderBy } from 'src/app/enums/trip-order-by';
import { SearchTripModel } from 'src/app/interfaces/search-trip';

@Component({
  selector: 'app-sorting-panel',
  templateUrl: './sorting-panel.component.html',
  styleUrls: ['./sorting-panel.component.scss']
})
export class SortingPanelComponent implements OnInit {

  constructor(private router: Router,) { }
  orderBy = TripOrderBy;
  @Input() trip: SearchTripModel = {
    countOfSeats: 1,
    endPlace: '',
    startPlace: '',
    startTime: new Date(),
    orderBy: TripOrderBy.EarliestDepartureTime,
  };
  @Output() OrderByOutput: EventEmitter<TripOrderBy> = new EventEmitter<TripOrderBy>;
  ngOnInit(): void {
    // console.log(this.trip);
  }
  orderByParams() {
    this.OrderByOutput.emit(this.trip.orderBy);
  }
  deleteSelectedSort() {
    this.OrderByOutput.emit(TripOrderBy.EarliestDepartureTime);
  }
}
