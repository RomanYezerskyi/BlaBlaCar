import { UserModel } from "../user-interfaces/user-model";
import { Message } from "./message";
import { UsersInChats } from "./users-in-chats";

export interface Chat {
    id: string
    chatName?: string,
    chatImage?: string,
    messages?: Array<Message>,
    users?: Array<UsersInChats>,
    chatType?: number,
    createdAt?: Date,
}
