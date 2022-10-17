import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { CarStatus } from 'src/app/core/models/car-models/car-status';
import { UserModel } from 'src/app/core/models/user-models/user-model';
import { UserRequestResponseModel } from 'src/app/core/models/user-models/user-request-response-model';
import { UserStatus } from 'src/app/core/models/user-models/user-status';
import { AdminService } from 'src/app/core/services/admin/admin.service';
import { ImgSanitizerService } from 'src/app/core/services/image-sanitizer-service/img-sanitizer.service';

@Component({
  selector: 'app-user-requests',
  templateUrl: './users-requests.component.html',
  styleUrls: ['./users-requests.component.scss']
})
export class UsersRequestsComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  requestsId: number = 0;
  requests: UserRequestResponseModel = { users: [] = [], totalRequests: 0 };
  userStatus = UserStatus;
  carStatus = CarStatus;
  totalRequests: number = 0;
  private Skip: number = 0;
  private Take: number = 5;
  isFullListDisplayed: boolean = false;
  displayedColumns: string[] = ['Img', 'CreatedAt', 'Name', 'Phone', 'Email', 'User documents', 'Cars',];
  isSpinner: boolean = false;
  noData = false;
  constructor(
    private sanitizeImgService: ImgSanitizerService,
    private router: Router,
    private route: ActivatedRoute,
    private adminService: AdminService) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }
  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.requestsId = params['id'];
    });
    this.getRequests();
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public getRequests(): void {
    if (this.Skip <= this.totalRequests) {
      this.isSpinner = true;
      this.adminService.getUserRequests(this.requestsId, this.Take, this.Skip).subscribe(
        response => {
          if (response != null) {
            this.requests.users = this.requests.users.concat(response.users);

            if (this.totalRequests == 0)
              this.totalRequests = response.totalRequests!;

            this.noData = false;
          }
          else {
            if (this.totalRequests === 0) this.noData = true;
          }
          this.isSpinner = false;
        },
        (error: HttpErrorResponse) => { console.error(error.error); this.isSpinner = false; }
      );
    }
    else {
      this.isFullListDisplayed = true;
    }
    this.Skip += this.Take;
  }
  navigateToRequestInfo(id: string): void {
    this.router.navigate(['admin/requests-info/', id])
  }
  sanitizeImg(img: string): SafeUrl {
    return this.sanitizeImgService.sanitiizeUserImg(img);
  }
}
