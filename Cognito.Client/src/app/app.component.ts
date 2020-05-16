import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { AuthService } from '../app/auth/auth.service';
import { Subscription } from 'rxjs';
import { User } from './auth/user.model';
import { UploaderCommService } from './shared/services/uploader-comm.service';
import { DxDrawerComponent } from 'devextreme-angular';
import { AutoLogoutService } from './shared/services/auto-logout.service';
import { Actions, ofActionSuccessful } from '@ngxs/store';
import { Auth } from './shared/state/auth/auth.actions';
import { Router } from '@angular/router';
import { Routes } from './shared/constants/routes';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit, OnDestroy {
    isOpened: Boolean = false;
    isDrawerOpened: boolean = false;
    positionModes: string[] = ['left', 'right'];
    showModes: string[] = ['push', 'shrink', 'overlap'];
    text: string;
    selectedOpenMode: string = 'shrink';
    selectedPosition: string = 'left';
    selectedRevealMode: string = 'slide';
    menuVisible: boolean;
    userObs: Subscription;
    user: User;
    subscription: Subscription;
    message: string;

    @ViewChild(DxDrawerComponent, { static: true }) mydrawer: DxDrawerComponent;

    constructor(
        public uploaderCommService: UploaderCommService,
        private auth: AuthService,
        private autoLogoutService: AutoLogoutService,
        private router: Router,
        private actions: Actions
    ) {}

    ngOnInit() {
        this.actions.pipe(ofActionSuccessful(Auth.Login)).subscribe(() => {
            this.router.navigate([Routes.Home]);
        });

        this.menuVisible = true;
        this.userObs = this.auth.currentUserObservable.subscribe((user) => {
            this.user = user;
            if (this.user) {
                this.autoLogoutService.initialize();
            } else {
                this.autoLogoutService.stop();
            }
        });

        this.subscription = this.uploaderCommService.getClickCall().subscribe((message) => {
            this.mydrawer.instance.toggle();
        });
    }

    ngOnDestroy() {
        this.userObs.unsubscribe();
    }
}
