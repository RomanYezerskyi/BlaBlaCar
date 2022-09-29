import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TripModel } from 'src/app/interfaces/trip-interfaces/trip-model';
import { DialogBookingConfirmationComponent } from './dialog-booking-confirmation/dialog-booking-confirmation.component';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { CarModel } from 'src/app/interfaces/car-interfaces/car-model';
import { SafeUrl } from '@angular/platform-browser';
import { TripService } from 'src/app/services/tripservice/trip.service';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { UserPermissionsTrip } from 'src/app/enums/user-permissions-trip';
import { ChatService } from 'src/app/services/chatservice/chat.service';
import { Subject, takeUntil } from 'rxjs';
import { MapsService } from 'src/app/services/maps-service/maps.service';
import { GeocodingFeatureProperties } from '../../maps-autocomplete/maps-autocomplete.component';
@Component({
	selector: 'app-trip-page-info',
	templateUrl: './trip-page-info.component.html',
	styleUrls: ['./trip-page-info.component.scss', '../search-trip/search-trip.component.scss']
})
export class TripPageInfoComponent implements OnInit, OnDestroy {
	private unsubscribe$: Subject<void> = new Subject<void>();
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
		private router: Router,
		private mapsService: MapsService) { }

	ngOnInit(): void {
		this.route.params.subscribe(params => {
			this.tripId = params['id'];
		});
		this.route.queryParams.subscribe(params => {
			this.requestedSeats = params['requestedSeats']
		});
		this.searchData();
	}
	ngOnDestroy(): void {
		this.unsubscribe$.next();
		this.unsubscribe$.complete();
	}
	sanitizeUserImg(img: string): SafeUrl {
		return this.imgSanitaze.sanitiizeUserImg(img);
	}

	searchData = () => {
		this.tripService.getTripById(this.tripId).pipe(takeUntil(this.unsubscribe$)).subscribe(
			response => {
				this.getPlaces(response);
				this.trip = response;
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
		dRef.componentInstance.onSubmitReason.pipe(takeUntil(this.unsubscribe$)).subscribe(() => {
			this.searchData();
		});
	}
	getChat(userId: string) {
		this.chatService.GetPrivateChat(userId).pipe(takeUntil(this.unsubscribe$)).subscribe(
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
	private getPlaces(trip: TripModel) {
		let text = `${trip.startLat}%20${trip.startLon}`;
		this.mapsService.getPlaceName(text).pipe(takeUntil(this.unsubscribe$)).subscribe((data: any) => {
			const placeSuggestions = data.features.map((feature: { properties: GeocodingFeatureProperties; }) => {
				const properties: GeocodingFeatureProperties = (feature.properties as GeocodingFeatureProperties);
				return {
					shortAddress: this.mapsService.generateShortAddress(properties),
					fullAddress: this.mapsService.generateFullAddress(properties),
					data: properties
				}
			});
			trip.startPlace = placeSuggestions[0].data.formatted;
			console.log(placeSuggestions);
		}, err => {
			console.log(err);
		});
		text = `${trip.endLat}%20${trip.endLon}`;
		this.mapsService.getPlaceName(text).pipe(takeUntil(this.unsubscribe$)).subscribe((data: any) => {
			const placeSuggestions = data.features.map((feature: { properties: GeocodingFeatureProperties; }) => {
				const properties: GeocodingFeatureProperties = (feature.properties as GeocodingFeatureProperties);
				return {
					shortAddress: this.mapsService.generateShortAddress(properties),
					fullAddress: this.mapsService.generateFullAddress(properties),
					data: properties
				}
			});
			trip.endPlace = placeSuggestions[0].data.formatted;
			console.log(placeSuggestions);
		}, err => {
			console.log(err);
		});
	}
}
