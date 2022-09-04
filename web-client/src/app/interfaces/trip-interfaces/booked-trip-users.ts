import { SeatModel } from "../car-interfaces/seat";
import { UserModel } from "../user-interfaces/user-model";

export interface BookedTripUsers {
    userId: string;
    user: UserModel;
    seats: SeatModel[];
}
