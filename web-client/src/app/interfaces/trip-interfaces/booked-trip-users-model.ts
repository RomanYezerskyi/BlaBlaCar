import { SeatModel } from "../car-interfaces/seat-model";
import { UserModel } from "../user-interfaces/user-model";

export interface BookedTripUsersModel {
    userId: string;
    user: UserModel;
    seats: SeatModel[];
}
