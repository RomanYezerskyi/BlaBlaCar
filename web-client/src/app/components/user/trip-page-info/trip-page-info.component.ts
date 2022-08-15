import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TripModel } from 'src/app/interfaces/trip';
import { DialogBookingConfirmationComponent } from './dialog-booking-confirmation/dialog-booking-confirmation.component';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { CarModel } from 'src/app/interfaces/car';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
@Component({
	selector: 'app-trip-page-info',
	templateUrl: './trip-page-info.component.html',
	styleUrls: ['./trip-page-info.component.scss', '../search-trip/search-trip.component.scss']
})
export class TripPageInfoComponent implements OnInit {
	private id!: number;
	requestedSeats = 0;
	carModel: CarModel = { id: 0, carType: 0, modelName: '', registNum: '', seats: [], carStatus: -1, carDocuments: [] }
	data: TripModel = {
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
	private readonly url = 'https://localhost:6001/api/Trips/';
	constructor(private route: ActivatedRoute, private http: HttpClient,
		private dialog: MatDialog, private sanitizer: DomSanitizer) { }

	ngOnInit(): void {
		this.route.params.subscribe(params => {
			this.id = params['id'];
		});
		this.route.queryParams.subscribe(params => {
			this.requestedSeats = params['requestedSeats']
		});
		this.searchData();
	}
	sanitaizeImg(img: string): SafeUrl {
		console.log(img);
		return this.sanitizer.bypassSecurityTrustUrl(img);
	}

	searchData = () => {
		this.http.get(this.url + this.id)
			.subscribe({
				next: (res) => {
					this.data = res as TripModel;
					console.log(this.data);
				},
				error: (err: HttpErrorResponse) => console.error(err),
			});
	}


	openDialog() {

		const dialogConfig = new MatDialogConfig();

		dialogConfig.disableClose = true;
		dialogConfig.autoFocus = true;
		dialogConfig.data = {
			trip: this.data,
			requestedSeats: this.requestedSeats

		};
		this.dialog.open(DialogBookingConfirmationComponent, dialogConfig);
	}
}
