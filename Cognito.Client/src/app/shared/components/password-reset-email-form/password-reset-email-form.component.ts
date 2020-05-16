import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/auth/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AppInfoService } from '../../services';
import { DxFormComponent } from 'devextreme-angular';
import { Routes } from '../../constants/routes';

@Component({
    selector: 'app-password-reset-email-form',
    templateUrl: './password-reset-email-form.component.html',
    styleUrls: ['./password-reset-email-form.component.scss'],
})
export class PasswordResetEmailFormComponent implements OnInit {
    @ViewChild(DxFormComponent) form: DxFormComponent;
    data = {
        email: '',
    };
    buttonOptions = {
        text: 'Send Reset Link',
        type: 'default',
        stylingMode: 'contained',
        width: '100%',
        useSubmitBehavior: true,
    };
    title: string;

    constructor(
        private auth: AuthService,
        private route: ActivatedRoute,
        private router: Router,
        private appInfo: AppInfoService
    ) {}

    ngOnInit() {
        this.title = this.appInfo.title;
        const currentUser = this.auth.currentUser;
        if (currentUser) {
            this.router.navigate([Routes.Home]);
        }
    }

    onFormSubmit() {
        this.auth
            .resetPasswordEmail(this.data.email)
            .subscribe(() => this.form.instance.resetValues());
    }
}
