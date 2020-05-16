import { Injectable } from '@angular/core';
import { Subject, BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class AdminService {
    private _title = new BehaviorSubject<string>('');

    get title$() {
        return this._title.asObservable();
    }

    setTitle(title: string) {
        this._title.next(title);
    }
}
