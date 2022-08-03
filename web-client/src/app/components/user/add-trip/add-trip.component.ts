import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CarType } from 'src/app/enums/car-type';
import { AddTripModel } from 'src/app/interfaces/add-trip';
import { AddNewCarModel } from 'src/app/interfaces/addnew-car';
import { CarModel } from 'src/app/interfaces/car';

@Component({
  selector: 'app-add-trip',
  templateUrl: './add-trip.component.html',
  styleUrls: ['./add-trip.component.scss']
})
export class AddTripComponent implements OnInit {
  invalidForm: boolean | undefined;
  data:any = []
  trip: AddTripModel = {
    startPlace: '',
    endPlace: '',
    startTime: new Date(''),
    endTime: new Date(''),
    pricePerSeat: 0,
    description: '',
    countOfSeats: 0,
    carId:0
  };
  carType = CarType;
  userCars: CarModel[] | undefined;
  newCar: AddNewCarModel = {countOfSeats:0, modelName:'', registNum:'', carType: 0};
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    
    this.getUserCars();
    // const cars =  this.http.get("https://localhost:6001/api/Car")
    // .subscribe({
    //   next: (res) => {
    //     this.userCars = res as CarModel[];
    //     console.log(res);
    //   }
    // });
  }
  async getUserCars() {
    await this.http.get("https://localhost:6001/api/Car")
    .subscribe({
      next: (res) => {
        this.userCars = res as CarModel[];
        console.log(res);
      }
    });
  }
  
  addTrip = (form: NgForm) => {
    if(form.valid){
      if(this.userCars === undefined){
        alert("Add cars!");
        return;
      }
    const url ='https://localhost:6001/api/Trips/'
     this.http.post(url, this.trip)
    .subscribe((res)=>{
      this.data = res
      console.log(this.data)
      console.error(res);
      })
    } 
  } 
  addCar = ( form: NgForm) => {
    if (form.valid) {
      this.http.post("https://localhost:6001/api/Car", this.newCar, {
        headers: new HttpHeaders({ "Content-Type": "application/json"})
      })
      .subscribe({
        next: (res) => {
        this.data = res
        console.log(this.data)
        }, 
        error: (err: HttpErrorResponse) => console.error(err)
      })
    } 
  }
}
