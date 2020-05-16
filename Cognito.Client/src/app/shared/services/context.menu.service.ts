import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { UserRoles } from 'src/app/auth/user.roles.enum';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ContextMenuService {
    constructor(private router: Router, private auth: AuthService) {}
    createRouteItem(
        text: string,
        route: string,
        allowedUserRoles: UserRoles[] = null,
        allowedDomainRoles: UserRoles[] = null
    ): any {
        return this.createActionItem(
            text,
            () => {
                this.router.navigate([route]);
            },
            allowedUserRoles,
            allowedDomainRoles
        );
    }

    createActionItem(
        text: string,
        onClick: () => void = null,
        allowedUserRoles: UserRoles[] = null,
        allowedDomainRoles: UserRoles[] = null
    ): any {
        return this.createMenuItem(text, null, onClick, allowedUserRoles, allowedDomainRoles);
    }

    createParentItem(
        text: string,
        items: any[],
        allowedUserRoles: UserRoles[] = null,
        allowedDomainRoles: UserRoles[] = null
    ): any {
        return this.createMenuItem(text, items, null, allowedUserRoles, allowedDomainRoles);
    }

    createMenuItem(
        text: string,
        items: any[],
        onClick: () => void = null,
        allowedUserRoles: UserRoles[] = null,
        allowedDomainRoles: UserRoles[] = null
    ): any {
        const hasAllowedRole = allowedUserRoles || allowedDomainRoles;
        const hasAllowedUserRole = allowedUserRoles ? this.auth.isInAnyRole(allowedUserRoles) : false;
        const hasAllowedDomainRole = allowedDomainRoles ? this.auth.isInAnyDomainRole(allowedDomainRoles) : false;
        let visible = !hasAllowedRole || hasAllowedUserRole || hasAllowedDomainRole;

        if (items && visible) {
            visible = items.some((i) => i.visible);
        }

        return { text, items, onClick, visible };
    }
}
