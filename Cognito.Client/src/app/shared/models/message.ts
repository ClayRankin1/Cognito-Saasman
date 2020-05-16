export interface Message {
    id: number;
    senderId: string;
    senderUserName: string;
    recipientId: number;
    recipientUserName: string;
    Content: string;
    IsRead: boolean;
    dateRead: Date;
    messageSent: Date;
}
