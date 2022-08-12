import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserModel } from 'src/app/interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-status';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  user: UserModel = {
    id: '', email: '', firstName: '', phoneNumber: '',
    roles: [], cars: [] = [], userDocuments: [] = [], userStatus: -1, trips: [] = [], userTrips: [] = []
  };
  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.getUser();
  }
  getUser = () => {
    const url = 'https://localhost:6001/api/User';
    this.http.get(url)
      .subscribe({
        next: (res: any) => {
          this.user = res as UserModel;
          console.log(this.user);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      });
  }

}