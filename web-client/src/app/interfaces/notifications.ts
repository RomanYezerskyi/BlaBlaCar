export interface NotificationsModel {
    id?: string;
    text: string;
    userId: string;
    notificationStatus?: number;
    readNotificationStatus: number;
    createdAt: Date;
}
