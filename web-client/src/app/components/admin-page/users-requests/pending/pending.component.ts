import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CarStatus } from 'src/app/interfaces/car-status';
import { UserModel } from 'src/app/interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-status';

@Component({
  selector: 'app-pending',
  templateUrl: './pending.component.html',
  styleUrls: ['./pending.component.scss']
})
export class PendingComponent implements OnInit {
  requests: any;
  users: UserModel[] = [];
  userStatus = UserStatus;
  carStatus = CarStatus;
  constructor(private http: HttpClient, private router: Router) { }
  ngOnInit(): void {
    this.getRequests();
  }
  getRequests = () => {
    const url = 'https://localhost:6001/api/Admin/requests/'
    this.http.get(url + this.userStatus.Pending)
      .subscribe({
        next: (res) => {
          this.users = res as UserModel[];
          console.log(this.users);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      })
  }
  navigateToRequestInfo = (id: string) => {
    this.router.navigate(['admin/requests/info/', id])
  }

}
