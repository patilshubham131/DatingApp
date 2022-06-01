    export interface Message {
        id: number;
        senderId: number;
        senderUserName: string;
        senderPhotoUrl: string;
        recipientUsername: string;
        recipientId: number;
        recipentPhotoUrl: string;
        content: string;
        dateRead?: Date;
        messageSent: Date;
    }


