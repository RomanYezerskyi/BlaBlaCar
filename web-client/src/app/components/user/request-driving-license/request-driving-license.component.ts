import { HttpClient, HttpErrorResponse, HttpEventType, HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-request-driving-license',
  templateUrl: './request-driving-license.component.html',
  styleUrls: ['./request-driving-license.component.scss']
})
export class RequestDrivingLicenseComponent implements OnInit {
  private formData = new FormData();
  imgData = "https://localhost:6001/DriverDocuments/images/c0884a6e-26b0-4cd5-9c8c-1e35c5134de9.png";
  @Output() public onUploadFinished = new EventEmitter();
  constructor(private http: HttpClient, private sant: DomSanitizer) { }

  ngOnInit(): void {
  }
  uploadFile = (files: any) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];

    this.formData.append('fileToUpload', fileToUpload, fileToUpload.name);
    console.log(this.formData);
  }
  sanitizeImageUrl(imageUrl: string): SafeUrl {
    return this.sant.bypassSecurityTrustUrl(imageUrl);
  }
  addLicense = () => {
    this.http.post('https://localhost:6001/api/User/license', this.formData)
      .subscribe({
        next: (event: any) => {
          this.imgData = event.res as string;
          console.log(this.imgData);
        },
        error: (err: HttpErrorResponse) => console.log(err)
      });
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
