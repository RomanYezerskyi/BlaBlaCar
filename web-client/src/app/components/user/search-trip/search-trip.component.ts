import { HttpClient, HttpErrorResponse, HttpHandler, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { SearchTripModel } from 'src/app/interfaces/search-trip';
import { ActivatedRoute, Router } from '@angular/router';
import { TripModel } from 'src/app/interfaces/trip';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Call } from '@angular/compiler';
import { TripService } from 'src/app/services/tripservice/trip.service';
import { Observable } from 'rxjs';
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
  trip: SearchTripModel = {
    countOfSeats: 1,
    endPlace: '',
    startPlace: '',
    startTime: new Date(),
  };
  isParams = false;
  public isFullListDisplayed: boolean = false;
  // totalTrips = 0;
  // private Skip: number = 0;
  // private Take: number = 5;
  constructor(private http: HttpClient,
    private router: Router,
    private sanitizer: DomSanitizer,
    private route: ActivatedRoute,
    private tripService: TripService) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit() {

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

  onScroll() {
    this.tripsComponent.onScroll();
    this.isFullListDisplayed = this.tripsComponent.isFullListDisplayed;
  }

}
