import { CarModel } from "../car-interfaces/car";
import { RoleModel } from "../role";
import { TripModel } from "../trip-interfaces/trip";
import { UserDocuments } from "./user-documents";
import { UserStatus } from "./user-status";

export interface UserModel {
    id: string;
    email: string;
    firstName: string;
    phoneNumber: string;
    userImg?: string
    roles: any[];
    userDocuments: UserDocuments[];
    userStatus: UserStatus;
    cars: CarModel[];
    trips?: TripModel[];
    tripUsers?: TripModel[];
    createdAt?: Date;
}
