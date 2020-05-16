import { HttpInterceptor, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { HttpRequest, HttpHandler } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { alert } from 'devextreme/ui/dialog';
import { HttpStatusCodes } from '../../core/constants/http-status-codes';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError((err: any) => {
                if (err instanceof HttpErrorResponse) {
                    if (err.status === HttpStatusCodes.BadRequest && err.error.message) {
                        alert(err.error.message, err.error.title);
                    }
                }
                return throwError(err);
            })
        );
    }
}
