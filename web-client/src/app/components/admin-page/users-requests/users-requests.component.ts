import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CarStatus } from 'src/app/interfaces/car-status';
import { UserModel } from 'src/app/interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-status';

@Component({
  selector: 'app-user-requests',
  templateUrl: './users-requests.component.html',
  styleUrls: ['./users-requests.component.scss']
})
export class UsersRequestsComponent implements OnInit {
  requests: any;
  users: UserModel[] = [];
  userStatus = UserStatus;
  carStatus = CarStatus;
  constructor(private http: HttpClient, private router: Router) { }
  ngOnInit(): void {
  }
  getRequests = () => {
    const url = 'https://localhost:6001/api/Admin/requests/'
    this.http.get(url + 1)
      .subscribe({
        next: (res) => {
          this.users = res as UserModel[];
          console.log(this.users);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      })
  }
  navigateToRequestInfo = (id: string) => {
    this.router.navigate(['/admin/info/', id])
  }
}
