import { HttpClient, HttpErrorResponse, HttpEventType, HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Subject, takeUntil } from 'rxjs';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { UserService } from 'src/app/services/userservice/user.service';

@Component({
  selector: 'app-request-driving-license',
  templateUrl: './request-driving-license.component.html',
  styleUrls: ['./request-driving-license.component.scss']
})
export class RequestDrivingLicenseComponent implements OnInit, OnDestroy {
  @Output() public onUploadFinished = new EventEmitter();
  private unsubscribe$: Subject<void> = new Subject<void>();
  private formData = new FormData();
  constructor(private http: HttpClient, private imgSanitize: ImgSanitizerService, private userService: UserService) { }

  ngOnInit(): void {
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  uploadFile = (files: any) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload: Array<any> = files;
    for (let item of fileToUpload) {
      this.formData.append('fileToUpload', item, item.name);
    }
  }
  sanitizeImageUrl(imageUrl: string): SafeUrl {
    return this.imgSanitize.sanitiizeImg(imageUrl);
  }
  addLicense = () => {
    this.userService.addDrivingLicense(this.formData).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
}
