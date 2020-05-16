import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/auth/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AppInfoService } from '../../services';
import { DxFormComponent } from 'devextreme-angular';
import { switchMap } from 'rxjs/operators';
import { Routes } from '../../constants/routes';

@Component({
    selector: 'app-password-reset-form',
    templateUrl: './password-reset-form.component.html',
    styleUrls: ['./password-reset-form.component.scss'],
})
export class PasswordResetFormComponent implements OnInit {
    @ViewChild(DxFormComponent) form: DxFormComponent;
    title: string;
    data = {
        email: '',
        password: '',
        passwordConfirm: '',
        token: '',
    };
    buttonOptions = {
        text: 'Reset Password',
        type: 'default',
        stylingMode: 'contained',
        width: '100%',
        useSubmitBehavior: true,
    };

    constructor(
        private auth: AuthService,
        private route: ActivatedRoute,
        private router: Router,
        private appInfo: AppInfoService
    ) {
        this.data.token = this.route.snapshot.queryParamMap.get('token');
        this.data.email = this.route.snapshot.queryParamMap.get('email');
    }

    ngOnInit() {
        this.title = this.appInfo.title;
        // todo: FIXME - this check could be done on one place
        const currentUser = this.auth.currentUser;
        if (currentUser) {
            this.router.navigate([Routes.Home]);
        }
    }

    passwordComparison = () => {
        return this.form.instance.option('formData').password;
    };

    onFormSubmit() {
        this.auth
            .resetPassword(this.data)
            .pipe(
                switchMap(() =>
                    this.auth.login({ email: this.data.email, password: this.data.password })
                )
            )
            .subscribe(() => {
                this.router.navigate([Routes.Home]);
            });
    }
}
