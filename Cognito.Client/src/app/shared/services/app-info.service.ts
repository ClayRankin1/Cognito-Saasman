import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class AppInfoService {
    constructor() {}

    public get title() {
        return 'Cognito 3-11-20';
    }
}
