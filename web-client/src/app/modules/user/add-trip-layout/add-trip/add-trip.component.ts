import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CarType } from 'src/app/enums/car-type';
import { AddTripModel } from 'src/app/interfaces/trip-interfaces/add-trip-model';
import { AvailableSeatsModel } from 'src/app/interfaces/trip-interfaces/available-seats-model';
import { CarModel } from 'src/app/interfaces/car-interfaces/car-model';
import { SeatModel } from 'src/app/interfaces/car-interfaces/seat-model';
import { PlaceSuggestionModel } from 'src/app/interfaces/autocomplete-interfaces/place-suggestion-model';
@Component({
  selector: 'app-add-trip',
  templateUrl: './add-trip.component.html',
  styleUrls: ['./add-trip.component.scss'],
})
export class AddTripComponent implements OnInit {
  @Input() trip: AddTripModel = {} as AddTripModel;
  @Output() tripOutput: EventEmitter<AddTripModel> = new EventEmitter<AddTripModel>;
  @Output() pageOutput: EventEmitter<number> = new EventEmitter<number>;
  message: string | undefined;
  invalidForm: boolean | undefined;
  data: any = []
  constructor() { }
  ngOnInit(): void {
  }
  navigateToAvailableSeats() {
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
