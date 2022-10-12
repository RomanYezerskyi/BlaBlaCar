import { HttpErrorResponse } from '@angular/common/http';
import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject, takeUntil } from 'rxjs';
import { FeedBackModel } from 'src/app/core/models/notifications-models/feed-back-model';
import { NotificationsService } from 'src/app/core/services/notifications-service/notifications.service';

@Component({
  selector: 'app-feed-backs',
  templateUrl: './feed-backs.component.html',
  styleUrls: ['./feed-backs.component.scss']
})
export class FeedBacksComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  private take = 5;
  private skip = 0;
  feedBacks: FeedBackModel[] = [];
  constructor(
    private notificationService: NotificationsService,
    private dialogRef: MatDialogRef<FeedBacksComponent>,
    @Inject(MAT_DIALOG_DATA) data: any,) { }
  ngOnInit(): void {
    this.getFeedBacks();
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  getFeedBacks(): void {
    if (this.skip <= this.feedBacks.length) {
      this.notificationService.getUserFeedBacks(this.take, this.skip)
        .pipe(takeUntil(this.unsubscribe$)).subscribe(
          response => {
            this.feedBacks = response;
          },
          (error: HttpErrorResponse) => { console.log(error.error); }
        );
      this.skip += this.take;
    }
  }
  closeDialog(): void {
    this.dialogRef.close();
  }
  loadMore(): void {
    this.getFeedBacks();
  }
}
