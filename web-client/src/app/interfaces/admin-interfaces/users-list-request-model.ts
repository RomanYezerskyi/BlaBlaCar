import { UserListOrderby } from "src/app/enums/user-list-orderby";

export interface UsersListRequestModel {
    take: number;
    skip: number;
    orderBy: UserListOrderby
}
