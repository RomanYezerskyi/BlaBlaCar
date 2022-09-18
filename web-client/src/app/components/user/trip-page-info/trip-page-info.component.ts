import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TripModel } from 'src/app/interfaces/trip-interfaces/trip-model';
import { DialogBookingConfirmationComponent } from './dialog-booking-confirmation/dialog-booking-confirmation.component';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { CarModel } from 'src/app/interfaces/car-interfaces/car';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { TripService } from 'src/app/services/tripservice/trip.service';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { UserPermissionsTrip } from 'src/app/enums/user-permissions-trip';
import { ChatService } from 'src/app/services/chatservice/chat.service';
@Component({
	selector: 'app-trip-page-info',
	templateUrl: './trip-page-info.component.html',
	styleUrls: ['./trip-page-info.component.scss', '../search-trip/search-trip.component.scss']
})
export class TripPageInfoComponent implements OnInit {
	private tripId!: number;
	requestedSeats = 0;
	carModel: CarModel = {} as CarModel;
	trip: TripModel = {} as TripModel;
	userPermission = UserPermissionsTrip;
	constructor(
		private route: ActivatedRoute,
		private dialog: MatDialog,
		private tripService: TripService,
		private imgSanitaze: ImgSanitizerService,
		private chatService: ChatService,
		private router: Router) { }

	ngOnInit(): void {
		this.route.params.subscribe(params => {
			this.tripId = params['id'];
		});
		this.route.queryParams.subscribe(params => {
			this.requestedSeats = params['requestedSeats']
		});
		this.searchData();
	}
	sanitizeUserImg(img: string): SafeUrl {
		return this.imgSanitaze.sanitiizeUserImg(img);
	}

	searchData = () => {
		this.tripService.getTripById(this.tripId).pipe().subscribe(
			response => {
				this.trip = response;
				console.log(response)
			},
			(error: HttpErrorResponse) => { console.log(error.error); }
		);
	}


	openDialog() {
		const dialogConfig = new MatDialogConfig();

		dialogConfig.autoFocus = true;
		dialogConfig.data = {
			trip: this.trip,
			requestedSeats: this.requestedSeats
		};
		const dRef = this.dialog.open(DialogBookingConfirmationComponent, dialogConfig);

		dRef.componentInstance.onSubmitReason.subscribe(() => {
			this.searchData();
		});
	}
	getChat(userId: string) {
		this.chatService.createPrivateChat(userId).pipe().subscribe(
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
