import { CarModel } from "./car";
import { RoleModel } from "./role";
import { UserStatus } from "./user-status";

export interface UserModel {
    id:string;
    email: string;
    firstName:string;
    phoneNumber:string;
    roles:any[];
    drivingLicense:string;
    userStatus:UserStatus;
    cars:CarModel[];
}
