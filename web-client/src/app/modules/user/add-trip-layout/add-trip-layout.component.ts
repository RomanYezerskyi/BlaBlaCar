import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AddTripModel } from 'src/app/core/models/trip-models/add-trip-model';
import { CarModel } from 'src/app/core/models/car-models/car-model';
import { CarService } from 'src/app/core/services/car-service/car.service';
import { Subject, takeUntil } from 'rxjs';

import * as L from 'leaflet';
import '@geoapify/leaflet-address-search-plugin';
import { environment } from 'src/environments/environment';
import 'mapbox-gl-leaflet';
import { PlaceSuggestionModel } from 'src/app/core/models/autocomplete-models/place-suggestion-model';
import { AddAvailableSeatsModel } from 'src/app/core/models/trip-models/add-available-seats-model';
import { FormBuilder, NgForm, Validators } from '@angular/forms';
import { TripService } from 'src/app/core/services/trip-service/trip.service';

@Component({
  selector: 'app-add-trip-layout',
  templateUrl: './add-trip-layout.component.html',
  styleUrls: ['./add-trip-layout.component.scss']
})
export class AddTripLayoutComponent implements OnInit, OnDestroy, AfterViewInit {
  private unsubscribe$: Subject<void> = new Subject<void>();
  userCar: CarModel = {} as CarModel;
  trip: AddTripModel = { availableSeats: [] = [] } as AddTripModel;
  userCars: CarModel[] = [];
  dateNow: string = new Date().toISOString();
  private startMap!: L.Map;
  private startMarker!: L.Marker
  private endMap!: L.Map;
  private endMarker!: L.Marker
  private myIcon = L.icon({
    iconUrl: `${environment.geoapifyMarkerPoin}${environment.geoapifyFirstApiKey}`,
    iconSize: [25, 45],
  });
  startPlaceIs: string | null = null;
  endPlaceIs: string | null = null;
  firstFormGroup = this._formBuilder.group({
    startPlace: ['', Validators.required],
  });
  secondFormGroup = this._formBuilder.group({
    endPlace: ['', Validators.required],
  });
  showSuccess!: boolean;
  showError!: boolean;
  errorMessage!: string;
  constructor(
    private carService: CarService,
    private tripService: TripService,
    private _formBuilder: FormBuilder,
    private router: Router) {
  }

  ngOnInit(): void {
    this.getUserCars();
  }

  ngAfterViewInit() {
    this.createStartMap();
    this.createEndMap();

  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  private createStartMap(): void {
    this.startMap = L.map('map').setView([51.505, -0.09], 15);
    L.tileLayer(`${environment.geoapifyTileLayer}${environment.geoapifyFirstApiKey}`, {
      maxZoom: 20,
      attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(this.startMap);
  }
  private createEndMap(): void {
    this.endMap = L.map('map-end').setView([51.505, -0.09], 15);
    L.tileLayer(`${environment.geoapifyTileLayer}${environment.geoapifyFirstApiKey}`, {
      maxZoom: 20,
      attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(this.endMap);
  }
  startPlaceChanged(value: PlaceSuggestionModel): void {
    this.trip.startLat = value.data.lat;
    this.trip.startLon = value.data.lon;
    this.startPlaceIs = value.data.formatted
    this.startMap.panTo(new L.LatLng(this.trip.startLat, this.trip.startLon));

    this.startMap.eachLayer((layer) => {
      if (layer instanceof L.Marker) this.startMap.removeLayer(this.startMarker);
    })
    this.startMarker = L.marker([this.trip.startLat, this.trip.startLon], { icon: this.myIcon }).addTo(this.startMap);

  }
  endPlaceChanged(value: PlaceSuggestionModel) {
    this.trip.endLat = value.data.lat;
    this.trip.endLon = value.data.lon;
    this.endPlaceIs = value.data.formatted
    this.endMap.panTo(new L.LatLng(this.trip.endLat, this.trip.endLon));
    this.endMap.eachLayer((layer) => {
      if (layer instanceof L.Marker) this.endMap.removeLayer(this.endMarker);
    })
    this.endMarker = L.marker([this.trip.endLat, this.trip.endLon], { icon: this.myIcon }).addTo(this.endMap);
  }

  addTrip(): void {
    if (this.userCar === undefined) {
      alert("Add cars!");
      return;
    }
    this.trip.startTime = this.trip.startTime?.toLocaleString();
    this.trip.endTime = this.trip.endTime?.toLocaleString();
    this.tripService.addNewtrip(this.trip).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.showSuccess = true;
      },
      (error: HttpErrorResponse) => {
        console.error(error.error); this.showError = true;
        this.errorMessage = error.error;
      }
    )

  }

  getUserCars(): void {
    this.carService.getUserCars().pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.userCars = response;
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
  setSelectedCar(carId: number): void {
    this.trip.availableSeats = [];
    this.userCars.forEach(car => {
      if (carId == car.id) {
        car.seats.forEach(seat => {
          let availableSeat: AddAvailableSeatsModel = { seatId: seat.id };
          this.trip.availableSeats.push(availableSeat);
          seat.isSelected = true;
        });
      }
    });
  }
  addSeat(seatId: number): void {
    let seat: AddAvailableSeatsModel = { seatId: seatId }
    if (this.trip.availableSeats!.find(availableSeat => availableSeat.seatId == seat.seatId)) {
      this.trip.availableSeats = this.trip.availableSeats!.filter(x => x.seatId != seatId);
      this.changeSeatStatus(seatId);
    }
    else {
      this.trip.availableSeats!.push(seat);
      this.changeSeatStatus(seatId);
    }
  }

  async reload(url: string): Promise<boolean> {
    await this.router.navigateByUrl('/', { skipLocationChange: true });
    return this.router.navigateByUrl(url);
  }
  private changeSeatStatus(seatId: number): void {
    this.userCars.forEach(car => {
      if (this.trip.carId == car.id) {
        car.seats.forEach(seat => {
          if (seat.id == seatId) {
            if (seat.isSelected) { seat.isSelected = false; }
            else if (!seat.isSelected) { seat.isSelected = true; }
          }
        });
      }
    });
  }


}
