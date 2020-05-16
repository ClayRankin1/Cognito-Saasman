import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ChatService } from './chat.service';
import { AuthService } from '../auth/auth.service';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Component({
    templateUrl: './chat.component.html',
    styleUrls: ['./chat.component.scss'],
})
export class ChatComponent implements OnInit {
    public isConnected: boolean = false;
    public isConnecting: boolean = false;
    public isGettingChannels: boolean = false;
    public chatMessage: string;
    public currentUsername: string;
    public isMemberOfCurrentChannel: boolean = false;

    constructor(
        private chatService: ChatService,
        private auth: AuthService,
        private router: Router
    ) {
        this.currentUsername = this.auth.currentUser.userName;
    }

    ngOnInit() {}
}
