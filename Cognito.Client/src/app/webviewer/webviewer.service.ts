import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root',
})
export class WebViewerDataService {
    url: string;

    constructor(private http: HttpClient) {
        this.url = environment.activeURL + 'api/Details';
    }

    insertExtractionToDetails = (
        extracted: string,
        actId: number,
        matterId: number,
        source: string,
        ownerId: number,
        domainId: number
    ) => {
        return this.http.post(`${this.url}/Extraction`, {
            extracted,
            actId,
            matterId,
            source,
            ownerId,
            domainId,
        });
    };
}
