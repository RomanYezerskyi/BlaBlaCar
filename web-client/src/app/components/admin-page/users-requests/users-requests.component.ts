import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CarStatus } from 'src/app/interfaces/car-interfaces/car-status';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { UserRequestResponseModel } from 'src/app/interfaces/user-interfaces/user-request-response-model';
import { UserStatus } from 'src/app/interfaces/user-interfaces/user-status';
import { AdminService } from 'src/app/services/admin/admin.service';

@Component({
  selector: 'app-user-requests',
  templateUrl: './users-requests.component.html',
  styleUrls: ['./users-requests.component.scss']
})
export class UsersRequestsComponent implements OnInit {
  requestsId = 0;
  requests: UserRequestResponseModel = {
    users: [] = [],
    totalRequests: 0
  };
  userStatus = UserStatus;
  carStatus = CarStatus;
  totalRequests = 0;
  private Skip: number = 0;
  private Take: number = 5;
  public isFullListDisplayed: boolean = false;
  constructor(private http: HttpClient,
    private router: Router,
    private route: ActivatedRoute,
    private adminService: AdminService) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }
  async ngOnInit() {
    this.route.params.subscribe(params => {
      this.requestsId = params['id'];
    });
    this.getRequests();
  }
  public getRequests() {
    if (this.Skip <= this.totalRequests) {
      this.adminService.getUserRequests(this.requestsId, this.Take, this.Skip).pipe().subscribe(
        response => {
          if (response != null) {
            this.requests.users = this.requests.users.concat(response.users);
            console.log(response);
            if (this.totalRequests == 0)
              this.totalRequests = response.totalRequests!;
          }

        },
        (error: HttpErrorResponse) => { console.error(error.error); }
      );
    }
    else {
      this.isFullListDisplayed = true;
    }
    this.Skip += this.Take;
  }
  navigateToRequestInfo = (id: string) => {
    this.router.navigate(['admin/requests-info/', id])
  }
}
