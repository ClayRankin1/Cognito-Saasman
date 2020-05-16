import { EventEmitter, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { PaginatedResult } from '../shared/models/pagination';
import { Message } from '../shared/models/message';
import { HttpParams, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root',
})
export class ChatService {
    constructor(private http: HttpClient, private router: Router) {}

    getMessages(id: string, page?: number, itemsPerPage?: number, messageContainer?: string) {
        const paginatedResult = new PaginatedResult<Message[]>();

        let params = new HttpParams();
        params = params.append('MessageContainer', messageContainer);

        if (page !== null && itemsPerPage !== null) {
            params.append('pageNumber', page.toString());
            params.append('pageSize', itemsPerPage.toString());
        }

        return this.http
            .get<Message[]>(`${environment.activeURL}api/users/${id}/message`, {
                observe: 'response',
                params,
            })
            .pipe(
                map((res) => {
                    paginatedResult.result = res.body;

                    if (res.headers.get('Pagination') !== null) {
                        paginatedResult.pagination = JSON.parse(res.headers.get('Pagination'));
                    }

                    return paginatedResult;
                })
            );
    }
}
