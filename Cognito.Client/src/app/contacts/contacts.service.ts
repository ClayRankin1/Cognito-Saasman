import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root',
})
export class ContactsDataService {
    url: string;
    constructor(private http: HttpClient) {
        this.url = environment.activeURL + 'api/Contacts';
    }
    LinkContacts(contactID: number, actID: number, matterID: number, domainID: number) {
        return this.http.post(`${this.url}/LinkContacts`, {
            contactID,
            actID,
            matterID,
            domainID,
        });
    }
}
