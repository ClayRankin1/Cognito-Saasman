import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { AppInfoService } from '../../services';
import { Routes } from '../../constants/routes';
import { finalize } from 'rxjs/operators';
import { AutoLogoutService } from '../../services/auto-logout.service';

@Component({
    selector: 'app-login-form',
    templateUrl: './login-form.component.html',
    styleUrls: ['./login-form.component.scss'],
})
export class LoginFormComponent implements OnInit {
    data = {
        email: '',
        password: '',
    };
    next: string;
    title: string;
    isLoading = false;
    isInvalidLogin = false;

    constructor(
        private auth: AuthService,
        private route: ActivatedRoute,
        private router: Router,
        private appInfo: AppInfoService,
        private autoLogoutService: AutoLogoutService
    ) {}

    ngOnInit() {
        this.title = this.appInfo.title;
        const currentUser = this.auth.currentUser;
        if (currentUser) {
            this.router.navigate([Routes.Home]);
        }

        this.autoLogoutService.stop();
        this.next = this.route.snapshot.queryParamMap.get('next') || Routes.Home;
    }

    onFormSubmit() {
        this.isLoading = true;
        this.auth
            .login({ email: this.data.email, password: this.data.password })
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe({
                next: () => {
                    this.router.navigate([this.next]);
                },
                error: () => {
                    this.isInvalidLogin = true;
                },
            });
    }
}
