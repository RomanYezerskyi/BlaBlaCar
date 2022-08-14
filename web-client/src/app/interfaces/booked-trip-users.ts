import { SeatModel } from "./seat";
import { UserModel } from "./user-model";

export interface BookedTripUsers {
    userId: string;
    user: UserModel;
    seats: SeatModel[];
}
