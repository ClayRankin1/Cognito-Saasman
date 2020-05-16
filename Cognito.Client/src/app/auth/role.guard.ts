import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
    providedIn: 'root',
})
export class RoleGuard implements CanActivate {
    constructor(private router: Router, private auth: AuthService) {}

    canActivate(route: ActivatedRouteSnapshot): boolean {
        const allowedRoles = route.data.allowedRoles;
        const allowedDomainRoles = route.data.allowedDomainRoles;

        const hasAllowedRole = allowedRoles ? this.auth.isInAnyRole(allowedRoles) : false;
        const hasAllowedDomainRole = allowedDomainRoles ? this.auth.isInAnyDomainRole(allowedDomainRoles) : false;
        if (hasAllowedRole || hasAllowedDomainRole) {
            return true;
        }

        this.router.navigate(['/login']);
    }
}
