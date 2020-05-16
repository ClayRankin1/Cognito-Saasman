import { Router } from '@angular/router';
import { Injectable, NgZone } from '@angular/core';
import { AuthService } from '../../auth/auth.service';
import { environment } from '../../../environments/environment';
import { alert } from 'devextreme/ui/dialog';

const CHECK_INTERVAL = 1000; // in ms

@Injectable({
    providedIn: 'root',
})
export class AutoLogoutService {
    private lastAction: number;
    private interval: any;
    private resetListener: EventListener;

    constructor(private auth: AuthService, private router: Router, private ngZone: NgZone) {
        this.resetListener = (e: Event) => this.reset();
    }

    initialize() {
        this.reset();
        this.check();
        this.initListener();
        this.initInterval();
    }

    initListener() {
        this.ngZone.runOutsideAngular(() => {
            document.body.addEventListener('click', this.resetListener);
            document.body.addEventListener('mousemove', this.resetListener);
        });
    }

    initInterval() {
        this.ngZone.runOutsideAngular(() => {
            this.interval = setInterval(() => {
                this.check();
            }, CHECK_INTERVAL);
        });
    }

    reset() {
        this.lastAction = Date.now();
    }

    stop() {
        clearInterval(this.interval);
        this.lastAction = Number.MAX_VALUE;
        document.body.removeEventListener('click', this.resetListener);
        document.body.removeEventListener('mousemove', this.resetListener);
    }

    check() {
        const now = Date.now();
        const timeLeft = this.lastAction + environment.autoLogoutInMinutes * 60 * 1000;
        const diff = timeLeft - now;
        const isTimeout = diff < 0;

        this.ngZone.run(() => {
            if (isTimeout) {
                const { refreshToken } = this.auth.currentUser;
                this.auth.logout({ refreshToken });
                this.router.navigate(['/login']);
                alert(
                    'You have been logged out due to inactivity. Log in again to continue.',
                    'Inactivity auto logout'
                );
            }
        });
    }
}
