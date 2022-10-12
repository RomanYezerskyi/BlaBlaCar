import { HttpClient, HttpErrorResponse, HttpEventType, HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, Inject, OnDestroy, OnInit, Output } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { UpdateModeEnum } from 'chart.js';
import { Subject, takeUntil } from 'rxjs';
import { UpdateUserDocuments } from 'src/app/core/models/user-models/update-user-documents';
import { UserDocumentsModel } from 'src/app/core/models/user-models/user-documents-model';
import { ImgSanitizerService } from 'src/app/core/services/image-sanitizer-service/img-sanitizer.service';
import { UserService } from 'src/app/core/services/user-service/user.service';

@Component({
  selector: 'app-request-driving-license',
  templateUrl: './request-driving-license.component.html',
  styleUrls: ['./request-driving-license.component.scss']
})
export class RequestDrivingLicenseComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  private formData = new FormData();
  private documents: UserDocumentsModel[] = [];
  images: Array<string> = [];
  fileToUpload: Array<File> = [];
  updateModel: UpdateUserDocuments = {} as UpdateUserDocuments;
  constructor(
    private _snackBar: MatSnackBar,
    private imgSanitize: ImgSanitizerService,
    private userService: UserService,
    private dialogRef: MatDialogRef<RequestDrivingLicenseComponent>,
    @Inject(MAT_DIALOG_DATA) data: any) { }

  ngOnInit(): void {
    this.updateModel.deletedDocuments = [];
    this.getUserDocumets();
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  close(): void {
    this.dialogRef.close();
  }
  getUserDocumets(): void {
    this.userService.getUserDrivingDocuments().pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        if (response != null) {
          this.documents = response;
          this.documents.forEach(x => this.images.push(x.drivingLicense));
        }
      },
      (error: HttpErrorResponse) => { console.error(error.error); this.openSnackBar(error.error); }
    );
  }
  deleteImg(img: string): void {
    if (this.updateModel.deletedDocuments == undefined)
      this.updateModel.deletedDocuments = [];
    let doc = this.documents.find(x => x.drivingLicense == img);
    this.updateModel.deletedDocuments.push(doc!);
    console.log(this.updateModel.deletedDocuments);
  }
  getImages(files: File[]): void {
    this.fileToUpload = files;
  }
  uploadFile(): void {
    if (this.fileToUpload.length > 0) {
      for (let item of this.fileToUpload) {
        this.formData.append('DocumentsFile', item, item.name);
      }
    }
    if (this.updateModel.deletedDocuments?.length > 0) {
      for (let item of this.updateModel.deletedDocuments) {
        console.log(item);
        this.formData.append('DeletedDocuments', item.id!);
      }
    }
  }
  sanitizeImageUrl(imageUrl: string): SafeUrl {
    return this.imgSanitize.sanitiizeImg(imageUrl);
  }
  addLicense(): void {
    if (this.fileToUpload.length == 0 && this.updateModel.deletedDocuments.length == 0) return;
    this.uploadFile();
    this.userService.addDrivingLicense(this.formData).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        console.log(response);
        this.openSnackBar("Documents saved!");
      },
      (error: HttpErrorResponse) => { console.error(error.error); this.openSnackBar(error.error) }
    );
  }
  private openSnackBar(message: string) {
    this._snackBar.open(message, "Close");
  }
}
