import { AvailableSeatsModel } from "./available-seats-model";
import { BookedTripUsersModel } from "./booked-trip-users-model";
import { CarModel } from "../car-interfaces/car-model";
import { TripUserModel } from "./trip-user-model";
import { UserModel } from "../user-interfaces/user-model";
import { UserPermissionsTrip } from "src/app/enums/user-permissions-trip";

export interface TripModel {
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
    bookedTripUsers?: BookedTripUsersModel[];
    TotalTrips?: number;
    userPermission?: UserPermissionsTrip;
}
