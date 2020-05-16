import { ClaimTypes } from './claim-types';
import { DomainRole } from './domain-role';

export interface DecodedToken {
    nameid: string;
    email: string;
    mementousername: string;
    mementodomainid: string;
    role: string[];
    nbf: number;
    exp: number;
    iat: number;
}

export class User {
    public readonly userId: number;
    public readonly email: string;
    public readonly userName: string;
    public readonly domainId: number;
    public readonly roles: string[];
    public readonly domainRoles: DomainRole[];
    public readonly accessToken: string;
    public readonly refreshToken: string;

    constructor(decodedToken: DecodedToken, accessToken: string, refreshToken: string) {
        this.userId = +decodedToken[ClaimTypes.UserId];
        this.email = decodedToken[ClaimTypes.Email];
        this.userName = decodedToken[ClaimTypes.UserName];
        this.domainId = +decodedToken[ClaimTypes.DomainId];
        this.roles = decodedToken[ClaimTypes.Roles];
        this.domainRoles = JSON.parse(decodedToken[ClaimTypes.DomainRoles]);
        this.accessToken = accessToken;
        this.refreshToken = refreshToken;
    }
}
