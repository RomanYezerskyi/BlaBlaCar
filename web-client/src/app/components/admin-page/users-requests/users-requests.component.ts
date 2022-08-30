import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CarStatus } from 'src/app/interfaces/car-status';
import { UserModel } from 'src/app/interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-status';
import { AdminService } from 'src/app/services/admin/admin.service';

@Component({
  selector: 'app-user-requests',
  templateUrl: './users-requests.component.html',
  styleUrls: ['./users-requests.component.scss']
})
export class UsersRequestsComponent implements OnInit {
  requestsId = 0;
  users: UserModel[] = [];
  userStatus = UserStatus;
  carStatus = CarStatus;
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
    this.getRequests(this.requestsId);
  }
  private getRequests(status: UserStatus) {
    this.adminService.getUserRequests(status).pipe().subscribe(
      response => {
        this.users = response;
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
    // const url = 'https://localhost:6001/api/Admin/requests/'
    // const user = await new Promise<UserModel[]>((resolve, reject) => {
    //   this.http.get<UserModel[]>(url + status)
    //     .subscribe({
    //       next: (res: UserModel[]) => {
    //         resolve(res), console.log(res);
    //       },
    //       error: (err: HttpErrorResponse) => { reject(err), console.error(err) }
    //     });
    // });
  }
  navigateToRequestInfo = (id: string) => {
    this.router.navigate(['admin/requests-info/', id])
  }
}
