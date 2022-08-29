import { HttpClient, HttpErrorResponse, HttpEventType, HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { UserService } from 'src/app/services/userservice/user.service';

@Component({
  selector: 'app-request-driving-license',
  templateUrl: './request-driving-license.component.html',
  styleUrls: ['./request-driving-license.component.scss']
})
export class RequestDrivingLicenseComponent implements OnInit {
  private formData = new FormData();
  @Output() public onUploadFinished = new EventEmitter();
  constructor(private http: HttpClient, private sant: DomSanitizer, private userService: UserService) { }

  ngOnInit(): void {
  }
  uploadFile = (files: any) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload: Array<any> = files;

    console.log(fileToUpload);
    for (let item of fileToUpload) {
      this.formData.append('fileToUpload', item, item.name);
    }
    console.log(this.formData);
  }
  sanitizeImageUrl(imageUrl: string): SafeUrl {
    return this.sant.bypassSecurityTrustUrl(imageUrl);
  }
  addLicense = () => {
    this.userService.addDrivingLicense(this.formData).pipe().subscribe(
      response => {
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
    // this.http.post('https://localhost:6001/api/User/license', this.formData)
    //   .subscribe({
    //     next: (event: any) => {
    //       console.log(event);
    //     },
    //     error: (err: HttpErrorResponse) => console.log(err)
    //   });
  }

  // onSelectNewFile(elemnt: any): void {
  //   if (elemnt.files?.length == 0) return;
  //   this.fileSelected = (elemnt.files as FileList)[0];
  //   this.imageUrl = this.sant.bypassSecurityTrustUrl(window.URL.createObjectURL(this.fileSelected)) as string;

  //   this.convertFileToBase64();
  // }
  // convertFileToBase64 = () => {
  //   let reader = new FileReader();
  //   reader.readAsDataURL(this.fileSelected as Blob);
  //   reader.onloadend = () => {
  //     this.imageBase64 = reader.result as string;
  //   }
  // }
}
