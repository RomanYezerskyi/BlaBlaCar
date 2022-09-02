import { UserListOrderby } from "src/app/enums/user-list-orderby";

export interface UsersListRequest {
    take: number;
    skip: number;
    orderBy: UserListOrderby
}
