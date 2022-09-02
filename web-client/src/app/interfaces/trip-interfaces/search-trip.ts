import { TripOrderBy } from "../../enums/trip-order-by";

export interface SearchTripModel {
    startPlace: string;
    endPlace: string;
    startTime: Date | string;
    countOfSeats: number;
    orderBy?: TripOrderBy;
    take?: number
    skip?: number
}
