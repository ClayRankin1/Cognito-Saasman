import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { User } from './user.model';
import { tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import JwtDecode from 'jwt-decode';
import { StorageService } from '../core/services';
import { UserRoles } from './user.roles.enum';
import { Router } from '@angular/router';

export interface TokensResponse {
    accessToken: string;
    refreshToken: string;
}

export interface ResetPasswordRequest {
    email: string;
    password: string;
    token: string;
}

export interface LoginRequest {
    email: string;
    password: string;
}

export interface LogoutRequest {
    refreshToken: string;
}

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    private user: BehaviorSubject<User>;
    private user$: Observable<User>;

    constructor(
        private http: HttpClient,
        private storageService: StorageService,
        private router: Router
    ) {
        this.user = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
        this.user$ = this.user.asObservable();
    }

    // todo: FIXME - Move user things into CurrentUserService
    public get currentUser(): User {
        return this.user.value;
    }

    public get currentUserObservable(): Observable<User> {
        return this.user$;
    }

    private get baseUrl(): string {
        return `${environment.activeURL}api/auth/`;
    }

    login({ email, password }: LoginRequest): Observable<any> {
        return this.http
            .post(`${this.baseUrl}login`, {
                email,
                password,
            })
            .pipe(
                tap((tokens: TokensResponse) => {
                    this.handleTokensResponse(tokens);
                })
            );
    }

    resetPassword(data: ResetPasswordRequest): Observable<any> {
        return this.http.post(`${this.baseUrl}password-reset`, { ...data });
    }

    resetPasswordEmail(email: string): Observable<any> {
        return this.http.post(`${this.baseUrl}password-reset-email`, { email });
    }

    refreshToken({ accessToken, refreshToken }: TokensResponse): Observable<any> {
        return this.http
            .post(`${this.baseUrl}refresh`, {
                accessToken,
                refreshToken,
            })
            .pipe(
                tap((tokens: TokensResponse) => {
                    this.handleTokensResponse(tokens);
                })
            );
    }

    // TODO: Add API call to invalidate refresh token
    logout({ refreshToken }: LogoutRequest): Observable<any> {
        this.storageService.clearAll();
        this.user.next(null);
        this.router.navigate(['/login']);
        return new Observable();
    }

    isInRole(role: UserRoles): boolean {
        if (!this.currentUser) {
            return false;
        }
        if (Array.isArray(this.currentUser.roles)) {
            return this.currentUser.roles.some((r) => r === UserRoles[role]);
        } else {
            return this.currentUser.roles === UserRoles[role];
        }
    }

    isInDomainRole(role: UserRoles): boolean {
        return this.currentUser?.domainRoles?.some((r) => r.roleId === role);
    }

    isAdminForDomain(domainId: number): boolean {
        return this.currentUser?.domainRoles?.some(
            (r) => r.roleId === UserRoles.DomainAdmin && r.domainId === domainId
        );
    }

    isInAnyRole(role: UserRoles[]): boolean {
        return role.some((r) => this.isInRole(r));
    }

    isInAnyDomainRole(role: UserRoles[]): boolean {
        return role.some((r) => this.isInDomainRole(r));
    }

    private handleTokensResponse(tokens: TokensResponse): void {
        const decoded = JwtDecode(tokens.accessToken);
        const user = new User(decoded, tokens.accessToken, tokens.refreshToken);

        // todo: FIXME - Use StorageService without accessing localStorage directly and use constants for keys
        localStorage.setItem('currentUser', JSON.stringify(user));
        localStorage.setItem('domainID', user.domainId.toString());
        this.user.next(user);
    }
}
