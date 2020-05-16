import { UserRoles } from 'src/app/auth/user.roles.enum';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root'})
export class UserDomainsService {

    getUserEmailsByRole(userDomains, userRole: UserRoles) {
        let emails = '';
        for (const userDomain of userDomains) {
            if (userDomain.roleId as UserRoles === userRole) {
                if (emails) {
                    emails += ', ';
                }

                emails +=  userDomain.user.email;
            }
        }

        return emails;
    }
}
