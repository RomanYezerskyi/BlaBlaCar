import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserModel } from 'src/app/interfaces/user-model';

@Component({
  selector: 'app-user-information',
  templateUrl: './user-information.component.html',
  styleUrls: ['./user-information.component.scss']
})
export class UserInformationComponent implements OnInit {
  user: UserModel = {
    id: '', email: '', firstName: '', phoneNumber: '',
    roles: [], cars: [] = [], userDocuments: [] = [], userStatus: -1, trips: [], tripUsers: []
  };
  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    // this.getUser();
  }
  getUser = () => {
    const url = 'https://localhost:6001/api/User';
    this.http.get(url)
      .subscribe({
        next: (res: any) => {
          this.user = res as UserModel;
          console.log(res);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      });
  }
}
