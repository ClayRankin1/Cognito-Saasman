import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Domain } from '../schema/domain';
import { Observable } from 'rxjs';
import { Project, User } from '../schema';

@Injectable({
    providedIn: 'root',
})
export class DomainService extends ApiService<Domain> {
    protected endPoint = 'Domains';

    getAdminDomains(): Observable<Domain[]> {
        return this.http.get<Domain[]>(this.buildUrl(['admin']));
    }

    getProjectsByDomainId(domainId: number): Observable<Project[]> {
        return this.http.get<Project[]>(this.buildUrl([domainId, 'projects']));
    }

    getUsersByDomainId(domainId: number): Observable<User[]> {
        return this.http.get<User[]>(this.buildUrl([domainId, 'users']));
    }
}
