import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AddTripModel } from 'src/app/interfaces/add-trip';

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
    countOfSeats: 0
  };
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }
  addTrip = (form: NgForm) => {
    if(form.valid){
    const url ='https://localhost:6001/api/Trips/'
    this.http.post(url, this.trip)
    
    .subscribe((res)=>{
      this.data = res
      console.log(this.data)
      console.error(res);
    })
  } 
}
}
