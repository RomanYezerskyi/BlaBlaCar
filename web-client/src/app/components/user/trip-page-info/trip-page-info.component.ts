import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TripModel } from 'src/app/interfaces/trip';
import { DialogBookingConfirmationComponent } from './dialog-booking-confirmation/dialog-booking-confirmation.component';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { CarModel } from 'src/app/interfaces/car';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { TripService } from 'src/app/services/tripservice/trip.service';
@Component({
	selector: 'app-trip-page-info',
	templateUrl: './trip-page-info.component.html',
	styleUrls: ['./trip-page-info.component.scss', '../search-trip/search-trip.component.scss']
})
export class TripPageInfoComponent implements OnInit {
	private tripId!: number;
	requestedSeats = 0;
	carModel: CarModel = { id: 0, carType: 0, modelName: '', registNum: '', seats: [], carStatus: -1, carDocuments: [] }
	trip: TripModel = {
		id: 0,
		startPlace: '',
		endPlace: '',
		startTime: new Date(),
		endTime: new Date(),
		countOfSeats: 0,
		pricePerSeat: 0,
		description: '',
		userId: '',
		availableSeats: [],
		car: this.carModel,
		tripUsers: [],
		user: { id: '', cars: [], email: '', firstName: '', phoneNumber: '', roles: [], userDocuments: [], userStatus: -1, userImg: '' }
	};

	constructor(private route: ActivatedRoute, private http: HttpClient,
		private dialog: MatDialog, private sanitizer: DomSanitizer, private tripService: TripService) { }

	ngOnInit(): void {
		this.route.params.subscribe(params => {
			this.tripId = params['id'];
		});
		this.route.queryParams.subscribe(params => {
			this.requestedSeats = params['requestedSeats']
		});
		this.searchData();
	}
	sanitaizeImg(img: string): SafeUrl {
		return this.sanitizer.bypassSecurityTrustUrl(img);
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
		// this.http.get(this.url + this.id)
		// 	.subscribe({
		// 		next: (res) => {
		// 			this.data = res as TripModel;
		// 			console.log(this.data);
		// 		},
		// 		error: (err: HttpErrorResponse) => console.error(err),
		// 	});
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
