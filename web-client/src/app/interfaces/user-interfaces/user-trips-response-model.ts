import { AvailableSeatsModel } from "../trip-interfaces/available-seats-model";
import { BookedTripUsersModel } from "../trip-interfaces/booked-trip-users-model";
import { CarModel } from "../car-interfaces/car-model";
import { UserModel } from "./user-model";

export interface UserTripsResponseModel {
    trips: Array<UserTrips>;
    totalTrips?: number;
}
export interface UserTrips {
    id: number;
    startPlace: string;
    endPlace: string;
    startLat?: number;
    startLon?: number;
    endLat?: number;
    endLon?: number;
    startTime: Date;
    endTime: Date;
    pricePerSeat: number;
    description: string;
    countOfSeats: number;
    userId: string;
    availableSeats: AvailableSeatsModel[];
    car: CarModel
    user?: UserModel;
    tripUsers: any[];
    bookedTripUsers: Array<BookedTripUsersModel>

}
