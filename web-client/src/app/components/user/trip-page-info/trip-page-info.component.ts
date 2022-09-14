import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TripModel } from 'src/app/interfaces/trip-interfaces/trip-model';
import { DialogBookingConfirmationComponent } from './dialog-booking-confirmation/dialog-booking-confirmation.component';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { CarModel } from 'src/app/interfaces/car-interfaces/car';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { TripService } from 'src/app/services/tripservice/trip.service';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { UserPermissionsTrip } from 'src/app/enums/user-permissions-trip';
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
	constructor(private route: ActivatedRoute, private http: HttpClient,
		private dialog: MatDialog, private sanitizer: DomSanitizer, private tripService: TripService, private imgSanitaze: ImgSanitizerService) { }

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
	private readonly url = 'https://localhost:6001/api/Trips/';
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

		// dialogConfig.disableClose = true;
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
}
