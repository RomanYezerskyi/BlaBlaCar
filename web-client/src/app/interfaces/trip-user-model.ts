import { SeatModel } from "./seat";

export interface TripUserModel {
    id: string,
    seat: SeatModel,
    seatId: string;
}
