import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { SearchTripModel } from 'src/app/interfaces/search-trip';
import {Router} from '@angular/router';
@Component({
  selector: 'app-search-trip',
  templateUrl: './search-trip.component.html',
  styleUrls: ['./search-trip.component.scss']
})
export class SearchTripComponent implements OnInit {
  invalidForm: boolean | undefined;
  data:any = []
  trip: SearchTripModel = {
    countOfSeats: 0,
    endPlace: '',
    startPlace: '',
    startTime: new Date('1991.01.01'),
  };
  tripRoute: { id:number } | undefined;


  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
  
  }
  searchData = (form: NgForm) => {
      if(form.valid){
      const url ='https://localhost:6001/api/Trips/search/'
      this.http.post(url, this.trip, {
        headers: new HttpHeaders({ "Content-Type": "application/json"})
      })
      .subscribe({
        next: (res)=>{
          this.data = res
          console.log(this.data);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      })
    } 
  }

  navigateToTripPage = (id: number) => {  
    this.router.navigate(['trip-page-info', id], {queryParams: {requestedSeats: this.trip.countOfSeats}}) }
}
