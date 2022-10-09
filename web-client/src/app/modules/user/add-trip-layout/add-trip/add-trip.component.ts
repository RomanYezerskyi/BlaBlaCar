import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CarType } from 'src/app/core/enums/car-type';
import { AddTripModel } from 'src/app/core/models/trip-models/add-trip-model';
import { AvailableSeatsModel } from 'src/app/core/models/trip-models/available-seats-model';
import { CarModel } from 'src/app/core/models/car-models/car-model';
import { SeatModel } from 'src/app/core/models/car-models/seat-model';
import { PlaceSuggestionModel } from 'src/app/core/models/autocomplete-models/place-suggestion-model';
@Component({
  selector: 'app-add-trip',
  templateUrl: './add-trip.component.html',
  styleUrls: ['./add-trip.component.scss'],
})
export class AddTripComponent implements OnInit {

  @Input() trip: AddTripModel = {} as AddTripModel;
  @Output() tripOutput: EventEmitter<AddTripModel> = new EventEmitter<AddTripModel>;
  @Output() pageOutput: EventEmitter<number> = new EventEmitter<number>;
  dateNow: string = new Date().toISOString();
  message: string | undefined;
  invalidForm: boolean | undefined;
  data: any = []
  constructor() { }
  ngOnInit(): void {
    console.log(this.dateNow);
  }
  navigateToAvailableSeats() {
    console.log(this.trip);
    this.tripOutput.emit(this.trip);
    this.pageOutput.emit(2);
  }
  startPlaceChanged(value: PlaceSuggestionModel) {
    this.trip.startLat = value.data.lat;
    this.trip.startLon = value.data.lon;
  }
  startEndChanged(value: PlaceSuggestionModel) {
    this.trip.endLat = value.data.lat;
    this.trip.endLon = value.data.lon;
  }
}
