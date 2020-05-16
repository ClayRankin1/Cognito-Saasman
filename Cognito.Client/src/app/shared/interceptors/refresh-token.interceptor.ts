import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError, Subject, BehaviorSubject, Subscription } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthService } from '../../auth/auth.service';
import { HttpStatusCodes } from '../../core/constants/http-status-codes';
import { User } from 'src/app/auth/user.model';

@Injectable({
    providedIn: 'root',
})
export class RefreshTokenInterceptor implements HttpInterceptor {
    private accessTokenError$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

    constructor(private authService: AuthService) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError((err) => {
                const { accessToken, refreshToken } = this.authService.currentUser;
                if (err.status === HttpStatusCodes.Unauthorized) {
                    if (!this.accessTokenError$.getValue()) {
                        this.accessTokenError$.next(true);

                        // Call API and get a New Access Token
                        return this.authService.refreshToken({ accessToken, refreshToken }).pipe(
                            switchMap((event: any) => {
                                this.accessTokenError$.next(false);
                                // Clone the request with new Access Token
                                const newRequest = request.clone(this.getNewHeaders());
                                return next.handle(newRequest);
                            }),
                            catchError((er) => {
                                this.authService.logout({ refreshToken });
                                return throwError(er);
                            })
                        );
                    } else {
                        // If it's not the first error, it has to wait until get the access/refresh token
                        return this.waitNewTokens().pipe(
                            switchMap((event: any) => {
                                // Clone the request with new Access Token
                                const newRequest = request.clone(this.getNewHeaders());
                                return next.handle(newRequest);
                            })
                        );
                    }
                } else if (err.status === HttpStatusCodes.Forbidden) {
                    // Logout if Forbidden response - Refresh Token invalid
                    this.authService.logout({ refreshToken });
                }

                // You can return the Object "err" if you want to.
                const error = (err.error && err.error.message) || err.statusText;

                return throwError(error);
            })
        );
    }

    // Wait until get the new access/refresh token
    private waitNewTokens(): Observable<any> {
        const subject = new Subject<any>();
        const waitToken$: Subscription = this.accessTokenError$.subscribe((error: boolean) => {
            if (!error) {
                subject.next();
                waitToken$.unsubscribe();
            }
        });
        return subject.asObservable();
    }

    private getNewHeaders() {
        return {
            setHeaders: {
                Authorization: `Bearer ${this.authService.currentUser.accessToken}`,
            },
        };
    }
}
