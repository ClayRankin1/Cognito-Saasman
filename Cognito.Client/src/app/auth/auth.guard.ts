import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
    providedIn: 'root',
})
export class AuthGuard implements CanActivate {
    constructor(private router: Router, private auth: AuthService) {}

    canActivate(_route: ActivatedRouteSnapshot, _state: RouterStateSnapshot): boolean {
        const currentUser = this.auth.currentUser;

        if (currentUser) {
            return true;
        }

        this.router.navigate(['/login']);
    }
}
