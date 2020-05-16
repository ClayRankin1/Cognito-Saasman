import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
    selector: 'app-user-panel',
    templateUrl: 'user-panel.component.html',
    styleUrls: ['./user-panel.component.scss'],
})
export class UserPanelComponent implements OnInit {
    username: string;
    userImageURL: string;
    @Input() menuItems: any;
    @Input() menuMode: string;
    initialName: string;

    constructor(private auth: AuthService) {}

    ngOnInit() {
        const currentUser = this.auth.currentUser;
        this.username = currentUser.userName + ' ' + currentUser.userId;
        this.userImageURL = '';
        this.initialName = currentUser.userName.substring(0, 2);
    }
}
