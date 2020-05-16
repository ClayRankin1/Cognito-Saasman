import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class DetailsService {
    private items: BehaviorSubject<any[]>;
    public selectedItems$: Observable<any[]>;

    private linked: BehaviorSubject<boolean>;
    public showLinked$: Observable<boolean>;

    constructor() {
        this.items = new BehaviorSubject([]);
        this.selectedItems$ = this.items.asObservable();

        this.linked = new BehaviorSubject(false);
        this.showLinked$ = this.linked.asObservable();
    }

    updateSelectedItems(items: number[]) {
        this.items.next(items);
    }

    toggleShowLinked() {
        this.linked.next(!this.linked.getValue());
    }
}
