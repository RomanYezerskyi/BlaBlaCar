import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { LightGallery } from 'lightgallery/lightgallery';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import lgZoom from 'lightgallery/plugins/zoom';
import { FormControl, Validators } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
@Component({
  selector: 'app-images-gallery',
  templateUrl: './images-gallery.component.html',
  styleUrls: ['./images-gallery.component.scss']
})
export class ImagesGalleryComponent implements OnInit {
  @Input() images: Array<string> = [];
  @Input() editMode: boolean = false;
  @Input() sendImages!: Observable<void>;
  @Output() addedFile: EventEmitter<File[]> = new EventEmitter<File[]>;
  @Output() removeImg: EventEmitter<string> = new EventEmitter<string>;
  fileControl!: FormControl;
  private files: any;
  private fileToUpload: Array<{ File: File, id: string }> = [];
  editImages: boolean = false;
  private lightGallery!: LightGallery;
  private needRefresh = false;
  settings = {
    counter: true,
    plugins: [lgZoom],
  };

  constructor(private imgSanitizer: ImgSanitizerService) { }

  ngOnInit(): void {
    this.fileControl = new FormControl(this.files, [
      Validators.required,
    ])
    this.fileControl.valueChanges.subscribe((files: any) => {
      this.uploadFile(files);
    })
  }
  ngAfterViewChecked(): void {
    if (this.needRefresh) {
      console.log("refresh");
      this.lightGallery.refresh();
      this.needRefresh = false;
    }
  }
  deleteImg(img: string) {
    if (this.fileToUpload.some(x => x.id == img)) {
      this.fileToUpload = this.fileToUpload.filter(x => x.id != img);
      let sendFiles: File[] = [];
      this.fileToUpload.forEach(x => sendFiles.push(x.File));
      this.images = this.images.filter(x => x != img);
      this.addedFile.emit(sendFiles);
    }
    else {
      this.images = this.images.filter(x => x != img);
      console.log(this.images);
      this.removeImg.emit(img);
    }
    this.needRefresh = true;
  }
  sanitizeImg(img: string): SafeUrl {
    return this.imgSanitizer.sanitiizeImg(img);
  }
  onInit = (detail: any): void => {
    this.lightGallery = detail.instance;
  }
  edit() {
    this.editImages = !this.editImages;
  }
  uploadFile = (files: any) => {
    if (files.length === 0) {
      return;
    }
    for (let item of files) {
      let reader = new FileReader();
      reader.readAsDataURL(item);
      reader.onload = () => {
        this.needRefresh = true;
        this.images.push(reader.result as string);
        this.fileToUpload.push({ File: item, id: reader.result as string });
      };
      reader.onloadend = () => {
        let sendFiles: File[] = [];
        this.fileToUpload.forEach(x => sendFiles.push(x.File));
        this.addedFile.emit(sendFiles);
      }
    }

  }

}
