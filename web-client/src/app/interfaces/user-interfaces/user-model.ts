import { CarModel } from "../car-interfaces/car-model";
import { RoleModel } from "../role-model";
import { TripModel } from "../trip-interfaces/trip-model";
import { UserDocumentsModel } from "./user-documents-model";
import { UserStatus } from "./user-status";

export interface UserModel {
    id: string;
    email: string;
    firstName: string;
    phoneNumber: string;
    userImg?: string
    roles: any[];
    userDocuments: UserDocumentsModel[];
    userStatus: UserStatus;
    cars: CarModel[];
    trips?: TripModel[];
    tripUsers?: TripModel[];
    createdAt?: Date;

}
