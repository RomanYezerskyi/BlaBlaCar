import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { SearchTripModel } from 'src/app/core/models/trip-models/search-trip-model';
import { ActivatedRoute, Router } from '@angular/router';
import { TripsComponent } from './trips/trips.component';

@Component({
  selector: 'app-search-trip',
  templateUrl: './search-trip.component.html',
  styleUrls: ['./search-trip.component.scss']
})


export class SearchTripComponent implements OnInit {
  @ViewChild(TripsComponent)
  tripsComponent!: TripsComponent;
  invalidForm: boolean | undefined;
  trip: SearchTripModel = { countOfSeats: 1 } as SearchTripModel;
  isParams = false;
  public isFullListDisplayed: boolean = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['startPlace'] && params['endPlace'] && params['startTime'] && params['seats']) {
        this.trip.startPlace = params['startPlace'];
        this.trip.endPlace = params['endPlace'];
        this.trip.startTime = new Date(params['startTime']);
        this.trip.countOfSeats = params['seats'];
        if (params['orderBy']) {
          this.trip.orderBy = params['orderBy'];
        }
        this.isParams = true;
      }
    });
    console.log(this.trip);
  }

  onScroll(): void {
    this.tripsComponent.onScroll();
    this.isFullListDisplayed = this.tripsComponent.isFullListDisplayed;
  }
}
