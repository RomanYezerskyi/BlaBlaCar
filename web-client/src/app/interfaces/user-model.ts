import { CarModel } from "./car";
import { RoleModel } from "./role";
import { UserDocuments } from "./user-documents";
import { UserStatus } from "./user-status";

export interface UserModel {
    id: string;
    email: string;
    firstName: string;
    phoneNumber: string;
    roles: any[];
    userDocuments: UserDocuments[];
    userStatus: UserStatus;
    cars: CarModel[];
}
