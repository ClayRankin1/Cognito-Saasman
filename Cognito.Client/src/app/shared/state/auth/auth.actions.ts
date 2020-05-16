import { LoginRequest } from 'src/app/auth/auth.service';

// eslint-disable-next-line @typescript-eslint/no-namespace
export namespace Auth {
    export class Login {
        static readonly type = '[Auth] Login';
        constructor(public payload: LoginRequest) {}
    }

    export class Logout {
        static readonly type = '[Auth] Logout';
    }

    export class RefreshTokens {
        static readonly type = '[Auth] Refresh Tokens';
    }
}
