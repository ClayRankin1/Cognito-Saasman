import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { User } from '../auth/user.model';

@Injectable({
    providedIn: 'root',
})
export class TeamDataService {
    private readonly baseUrl: string;

    constructor(private http: HttpClient) {
        this.baseUrl = `${environment.activeURL}api/tasks`;
    }

    GetOwners(key: number, domainId: number) {
        return this.http.get<IOwner>(`${this.baseUrl}/GetOwners`, {
            params: { matterId: `${key}` },
        });
    }

    GetTeams(key: number) {
        return this.http.get<User>(`${environment.activeURL}api/matters/${key}/team`);
    }

    GetTeamsAll(key: number) {
        return this.http.get<User>(`${environment.activeURL}api/domains/${key}/team`);
    }

    InsertAccruedTime(fromTime: string, toTime: string, actId: number) {
        return this.http.post<any>(`${environment.activeURL}api/accruedTimes`, {
            taskId: actId,
            from: fromTime,
            to: toTime,
        });
    }

    menuActionComplete(actId: number) {
        return this.http.put(`${this.baseUrl}/complete`, { taskId: actId });
    }

    // TODO: timestamps would be better than a string for movedate
    menuActionMove(actId: number, movedate: string) {
        return this.http.put<any>(`${this.baseUrl}/move`, {
            taskId: actId,
            nextDate: movedate,
        });
    }

    menuActionAdvance() {
        return this.http.put(`${this.baseUrl}/advance`, {});
    }
}

export interface IWorker {
    id: number;
    firstName: string;
}

export interface IOwner {
    ownerId: number;
    fullName: string;
}
