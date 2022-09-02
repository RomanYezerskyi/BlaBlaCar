import { UserModel } from "../user-interfaces/user-model";

export interface AdminStatistics {
    usersStatisticsCount: string[];
    usersDateTime: Array<Date>;

    carsStatisticsCount: string[];
    carsDateTime: Array<Date>;

    tripsStatisticsCount: string[];
    tripsDateTime: Array<Date>;

    weekStatisticsTripsCount: string[];
    weekTripsDateTime: Array<Date>;
}
