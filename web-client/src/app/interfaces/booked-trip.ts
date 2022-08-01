export interface BookedTripModel {
    id: number;
    tripId: number;
    requestedSeats: number;
    bookedSeats: any[];
}
