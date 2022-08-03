import { SeatModel } from "./seat";

export interface BookedTripModel {
    id: number;
    tripId: number;
    requestedSeats: number;
    bookedSeats: SeatModel[];
}
