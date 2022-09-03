import { UserModel } from "./user-interfaces/user-model";

export interface NotificationsModel {
    id?: string;
    text: string;
    userId?: string;
    user?: UserModel
    notificationStatus?: number;
    readNotificationStatus?: number;
    createdAt?: Date;
}
