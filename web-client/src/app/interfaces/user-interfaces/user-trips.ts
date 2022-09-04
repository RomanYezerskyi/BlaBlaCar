import { AvailableSeatsModel } from "../trip-interfaces/available-seats";
import { BookedTripUsers } from "../trip-interfaces/booked-trip-users";
import { CarModel } from "../car-interfaces/car";
import { UserModel } from "./user-model";

export interface UserTripsResponse {
    trips: Array<UserTrips>;
    totalTrips?: number;
}
export interface UserTrips {
    id: number;
    startPlace: string;
    endPlace: string;
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
    bookedTripUsers: Array<BookedTripUsers>

}
