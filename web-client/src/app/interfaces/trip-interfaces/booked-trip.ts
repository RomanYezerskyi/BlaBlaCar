import { SeatModel } from "../car-interfaces/seat";

export interface BookedTripModel {
    id: number;
    tripId: number;
    requestedSeats: number;
    bookedSeats: SeatModel[];
}
