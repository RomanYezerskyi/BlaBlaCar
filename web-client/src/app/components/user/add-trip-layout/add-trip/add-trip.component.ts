import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CarType } from 'src/app/enums/car-type';
import { AddTripModel } from 'src/app/interfaces/add-trip';
import { AddNewCarModel } from 'src/app/interfaces/addnew-car';
import { AvailableSeatsModel } from 'src/app/interfaces/available-seats';
import { CarModel } from 'src/app/interfaces/car';
import { SeatModel } from 'src/app/interfaces/seat';
import { SharedDataService } from 'src/app/services/shared-data.service';
@Component({
  selector: 'app-add-trip',
  templateUrl: './add-trip.component.html',
  styleUrls: ['./add-trip.component.scss'],
})
export class AddTripComponent implements OnInit {
  message: string | undefined;
  invalidForm: boolean | undefined;
  data: any = []
  @Input() trip: AddTripModel = {
    startPlace: '',
    endPlace: '',
    startTime: new Date(''),
    endTime: new Date(''),
    pricePerSeat: 0,
    description: '',
    countOfSeats: 0,
    carId: 0,
    availableSeats: []
  };
  @Output() tripOutput: EventEmitter<AddTripModel> = new EventEmitter<AddTripModel>;
  @Output() pageOutput: EventEmitter<number> = new EventEmitter<number>;

  constructor(private http: HttpClient, private router: Router, private sharedDataService: SharedDataService) { }

  ngOnInit(): void {

  }
  navigateToAvailableSeats() {
    this.tripOutput.emit(this.trip);
    this.pageOutput.emit(2);
  }

}
