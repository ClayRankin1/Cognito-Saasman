import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export abstract class ApiService<T> {
    protected abstract endPoint: string;

    private httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    };

    protected get url(): string {
        return this.baseUrl + this.endPoint;
    }

    protected get baseUrl(): string {
        return environment.activeURL + 'api/';
    }

    constructor(protected http: HttpClient) {}

    private getUrl(id?: number) {
        return id ? this.buildUrl([id]) : this.url;
    }

    protected buildUrl(args: any[]) {
        let finalUrl = this.url;

        for (const arg of args) {
            finalUrl += '/' + encodeURIComponent(arg);
        }
        return finalUrl;
    }

    public getAll(): Observable<T[]> {
        return this.http.get<T[]>(this.getUrl());
    }

    public getById(id: number): Observable<T> {
        return this.http.get<T>(this.getUrl(id));
    }

    public put(id: number, model: any) {
        return this.http.put<any>(this.getUrl(id), model, this.httpOptions);
    }

    public post(model: any) {
        return this.http.post<any>(this.getUrl(), model);
    }

    public delete(id: number) {
        return this.http.delete<any>(this.getUrl(id));
    }
}
