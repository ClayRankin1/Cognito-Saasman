import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { StorageService } from '..';
import { shareReplay, tap } from 'rxjs/operators';

@Injectable({
    providedIn: 'root',
})
export class CacheService {
    private static observableBuffer: Map<string, Observable<any>> = new Map<
        string,
        Observable<any>
    >();
    constructor(private storageService: StorageService) {}

    getCachedResult<T>(
        cacheKey,
        load: () => Observable<T>,
        lifeTimeInMinutes: number
    ): Observable<T> {
        const data = this.storageService.get<T>(cacheKey);

        if (data) {
            return of(data);
        }

        if (CacheService.observableBuffer.has(cacheKey)) {
            return CacheService.observableBuffer.get(cacheKey);
        }

        const lifetimeInMilliseconds = lifeTimeInMinutes * 60 * 1000;
        const observable = load().pipe(
            shareReplay(1),
            tap((result) => {
                this.storageService.set(cacheKey, result, lifetimeInMilliseconds);
            })
        );

        CacheService.observableBuffer.set(cacheKey, observable);
        return observable;
    }
}
