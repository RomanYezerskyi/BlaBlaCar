import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TripModel } from 'src/app/interfaces/trip';
import { UserModel } from 'src/app/interfaces/user-model';

@Component({
  selector: 'app-user-booked-trips',
  templateUrl: './user-booked-trips.component.html',
  styleUrls: ['./user-booked-trips.component.scss']
})
export class UserBookedTripsComponent implements OnInit {
  @Input() user: UserModel = {
    id: '', email: '', firstName: '', phoneNumber: '',
    roles: [], cars: [] = [], userDocuments: [] = [], userStatus: -1, trips: [], tripUsers: []
  };
  trips: TripModel[] = [] //{
  //   id: 0,
  //   car: {
  //     id: 0, carDocuments: [], carStatus: -1, carType: -1, modelName: '', registNum: '', seats: []
  //   },
  //   availableSeats: [], countOfSeats: 0, description: '', endPlace: '', endTime: new Date(), pricePerSeat: 0, startPlace: '',
  //   startTime: new Date(), userId: ''
  // }
  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
  }

  getUser = () => {
    const url = 'https://localhost:6001/api/User';
    this.user.tripUsers?.forEach(trip => {
      this.http.get(url)
        .subscribe({
          next: (res: any) => {
            this.trips.push(res as TripModel);
            console.log(res);
          },
          error: (err: HttpErrorResponse) => console.error(err),
        });
    })


  }
}
