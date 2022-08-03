import { RoleModel } from "./role";

export interface UserModel {
    id:string;
    email: string;
    firstName:string;
    phoneNumber:string;
    roles:any[];
}
