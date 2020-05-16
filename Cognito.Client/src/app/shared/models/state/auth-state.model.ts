import { UserRoles } from 'src/app/auth/user.roles.enum';

export interface AuthStateModel {
    userId: number;
    email: string;
    userName: string;
    domainId: number;
    roles: UserRoles[];
    accessToken: string;
    refreshToken: string;
}
