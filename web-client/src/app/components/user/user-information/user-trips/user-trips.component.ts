import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { UserTrips, UserTripsResponseModel } from 'src/app/interfaces/user-interfaces/user-trips-response-model';
import { TripService } from 'src/app/services/tripservice/trip.service';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { skip, Subject, takeUntil } from 'rxjs';
import { ChatService } from 'src/app/services/chatservice/chat.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-user-trips',
  templateUrl: './user-trips.component.html',
  styleUrls: ['./user-trips.component.scss']
})
export class UserTripsComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  trips: UserTripsResponseModel = { trips: [] = [], totalTrips: 0 };
  public isFullListDisplayed: boolean = false;
  totalTrips: number = 0;
  private Skip: number = 0;
  private Take: number = 5;
  constructor(
    private imgSanitaze: ImgSanitizerService, private router: Router,
    private tripService: TripService, private chatService: ChatService) { }

  ngOnInit(): void {
    this.getUserTrips();
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  sanitizeUserImg(img: string): SafeUrl {
    return this.imgSanitaze.sanitiizeUserImg(img);
  }
  getUserTrips(): void {
    console.log(this.Skip);
    if (this.Skip <= this.totalTrips) {
      this.tripService.getUserTrips(this.Take, this.Skip).pipe(takeUntil(this.unsubscribe$)).subscribe(
        response => {
          if (response != null) {
            this.trips.trips = this.trips.trips.concat(response.trips);
            console.log(this.trips);
            if (this.totalTrips == 0)
              this.totalTrips = response.totalTrips!;
          }
        },
        (error: HttpErrorResponse) => { console.log(error.error); }
      );
    }
    else {
      this.isFullListDisplayed = true;
    }
    this.Skip += this.Take;
  }
  deleteUserFromTrip(userId: string, tripId: number): void {
    let tripUser = {
      userId: userId,
      tripId: tripId
    };
    this.tripService.deleteUserFromTrip(tripUser).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.ngOnInit();
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
  }

  deleteTrip(id: number): void {
    this.tripService.deleteTrip(id).pipe().subscribe(
      response => {
        this.Skip = 0;
        this.totalTrips = 0;
        this.getUserTrips();

      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );
  }
  isTripCompleted(endDate: Date): boolean {

    if (new Date(endDate) < new Date()) return true;
    return false
  }
  getChat(userId: string) {
    this.chatService.GetPrivateChat(userId).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.router.navigate(['/chat'], {
          queryParams: {
            chatId: response
          }
        });
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
}
