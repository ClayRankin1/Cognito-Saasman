import { State, Selector, Action, StateContext } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { AuthService, TokensResponse } from 'src/app/auth/auth.service';
import { tap } from 'rxjs/operators';
import JwtDecode from 'jwt-decode';
import { ClaimTypes } from 'src/app/auth/claim-types';
import { Observable } from 'rxjs';
import { UserRoles } from 'src/app/auth/user.roles.enum';
import { AuthStateModel } from '../../models/state/auth-state.model';
import { Auth } from './auth.actions';
import { DecodedToken } from 'src/app/auth/user.model';

const defaultState: AuthStateModel = {
    userId: 0,
    email: null,
    userName: null,
    domainId: 0,
    roles: null,
    accessToken: null,
    refreshToken: null,
};

@State<AuthStateModel>({
    name: 'auth',
    defaults: defaultState,
})
@Injectable()
export class AuthState {
    constructor(private authService: AuthService) {}

    @Selector()
    static isAuthenticated(state: AuthStateModel): boolean {
        return !!state.accessToken;
    }

    @Selector()
    static accessToken(state: AuthStateModel): string {
        return state.accessToken;
    }

    @Selector()
    static refreshToken(state: AuthStateModel): string {
        return state.refreshToken;
    }

    @Selector()
    static userId(state: AuthStateModel): number {
        return state.userId;
    }

    @Selector()
    static userName(state: AuthStateModel): string {
        return state.userName;
    }

    @Selector()
    static domainId(state: AuthStateModel): number {
        return state.domainId;
    }

    @Selector()
    static isInRole(state: AuthStateModel): (role: UserRoles) => boolean {
        return (role: UserRoles): boolean => {
            if (Array.isArray(state.roles)) {
                return state.roles.some((r) => r === role);
            }
            return state.roles === role;
        };
    }

    @Selector()
    static isInAnyRole(state: AuthStateModel): (role: UserRoles[]) => boolean {
        return (role: UserRoles[]): boolean => role.some((r) => this.isInRole(state)(r));
    }

    // TODO: Needs to be implemented after Mateus' latest changes
    // @Selector()
    // static isInDomainRole(state: AuthStateModel): (role: UserRoles) => boolean {
    //     return (role: UserRoles): boolean => {
    //         if (Array.isArray(state.domainRoles)) {
    //             return state.domainRoles.some((r) => r.roleId === role);
    //         }
    //         return state.domainRoles === role;
    //     };
    // }

    // @Selector()
    // static isInAnyDomainRole(state: AuthStateModel): (role: UserRoles[]) => boolean {
    //     return (role: UserRoles[]): boolean => role.some((r) => this.isInDomainRole(state)(r));
    // }

    @Action(Auth.Login)
    login(ctx: StateContext<AuthStateModel>, action: Auth.Login): Observable<TokensResponse> {
        return this.authService.login(action.payload).pipe(
            tap((result: TokensResponse) => {
                const decodedToken = JwtDecode<DecodedToken>(result.accessToken);
                ctx.patchState({
                    userId: +decodedToken[ClaimTypes.UserId],
                    email: decodedToken[ClaimTypes.Email],
                    userName: decodedToken[ClaimTypes.UserName],
                    domainId: +decodedToken[ClaimTypes.DomainId],
                    roles: decodedToken[ClaimTypes.Roles],
                    accessToken: result.accessToken,
                    refreshToken: result.refreshToken,
                });
            })
        );
    }

    @Action(Auth.Logout)
    logout(ctx: StateContext<AuthStateModel>): Observable<{}> {
        const { refreshToken } = ctx.getState();
        return this.authService.logout({ refreshToken }).pipe(
            tap(() => {
                ctx.setState(defaultState);
            })
        );
    }

    @Action(Auth.RefreshTokens)
    refreshToken(ctx: StateContext<AuthStateModel>): Observable<TokensResponse> {
        const { accessToken, refreshToken } = ctx.getState();
        return this.authService.refreshToken({ accessToken, refreshToken }).pipe(
            tap((result: TokensResponse) =>
                ctx.patchState({
                    accessToken: result.accessToken,
                    refreshToken: result.refreshToken,
                })
            )
        );
    }
}
